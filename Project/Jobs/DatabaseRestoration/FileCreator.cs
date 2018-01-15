namespace DatabaseRestoration
{
    using DatabaseRestoration.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class FileCreator
    {
        private static string trimmed = "";
        private static string storedProcedureName = "";

        public static void CreateModel(string iNamespace, string iName, List<Column> properties)
        {
            trimmed = iName.ToLower().EndsWith("s") ? iName.Substring(0, iName.Length - 1) : iName;

            string path = ConfigurationManager.AppSettings["dataAccessProject"] + "Models\\" + trimmed + ".cs";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("namespace {0}", iNamespace);
                file.WriteLine("{");
                file.WriteWithTab(1, "using System;");
                file.WriteWithTab(1, "using System.Collections.Generic;");
                file.WriteWithTab(1, "using System.Linq;");
                file.WriteWithTab(1, "using System.Text;");
                file.WriteWithTab(1, "using System.Threading.Tasks;");
                file.WriteLine();
                file.WriteWithTab(1, "public class {0}", trimmed);
                file.WriteWithTab(1, "{");
                for (int i = 0; i < properties.Count; i++)
                {
                    string dataType = TranslateSqlTypeToCsharp(properties[i].DataType);
                    file.Write(extensions.CreateTabs(2) + "public " + dataType + " " + properties[i].ColumnName + " { get; ");
                    file.WriteLine("set; }");
                }
                file.WriteWithTab(1, "}");
                file.WriteLine("}");
            }
        }

        public static void CreateLayer(string iNamespace, string iLayer, string iName, List<Column> properties)
        {
            string path = ConfigurationManager.AppSettings["dataAccessProject"] + "Layers\\" + iName + "DataAccess.cs";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            trimmed = iName.ToLower().EndsWith("s") ? iName.Substring(0, iName.Length - 1) : iName;
            storedProcedureName = GetStoredProcedureName(trimmed);

            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("namespace {0}", iNamespace);
                file.WriteLine("{");
                file.WriteWithTab(1, "using System;");
                file.WriteWithTab(1, "using System.Linq;");
                file.WriteWithTab(1, "using System.Data;");
                file.WriteWithTab(1, "using AutoMapping;");
                file.WriteWithTab(1, "using CommandExecution;");
                file.WriteWithTab(1, "using System.Data.SqlClient;");
                file.WriteWithTab(1, "using System.Collections.Generic;");
                file.WriteWithTab(1, "using {0}.Models;", iNamespace);
                file.WriteLine();
                file.WriteWithTab(1, "public class {0}{1} : CommandExecutor {2}", trimmed, iLayer, Environment.NewLine);
                file.WriteWithTab(1, "{");
                file.WriteWithTab(2, "public {0}{1}(string iConnectionString, string iLogFile)", trimmed, iLayer);
                file.WriteWithTab(3, ": base(iConnectionString, iLogFile)");
                file.WriteWithTab(3, "{");
                file.WriteWithTab(3, "}");

                file.WriteLine();
                WriteViewAll(file, iName);
                WriteViewById(file, iName);
                WriteCreate(file, iName, properties);
                WriteUpdate(file, iName, properties);
                WriteDelete(file, iName);


                file.WriteWithTab(1, "}");
                file.WriteLine("}");
                file.Close();
            }
        }

        private static void WriteViewAll(StreamWriter file, string iName)
        {
            file.WriteWithTab(2, "public List<{0}> ViewAll{1}()", trimmed, iName);
            file.WriteWithTab(2, "{");
            file.WriteWithTab(3, "DataTable lQueryResult = ExecuteProcedureQuery(\"VIEW_ALL_{0}\");", storedProcedureName);
            file.WriteWithTab(3, "return AutoMap<{0}>.FromDataTable(lQueryResult).ToList();", trimmed);
            file.WriteWithTab(2, "}");
            file.WriteLine();
        }

        private static void WriteViewById(StreamWriter file, string iName)
        {
            file.WriteWithTab(2, "public {0} View{0}ById(Int64 {1})", trimmed, trimmed + "Id");
            file.WriteWithTab(2, "{");
            file.WriteWithTab(3, "{0} response = null;", trimmed);
            file.WriteWithTab(3, "DataTable lQueryResult = ExecuteProcedureQuery(\"VIEW_{0}_BY_ID\",", storedProcedureName);
            file.WriteWithTab(4, "new SqlParameter(\"@{0}\", {0}),", trimmed + "Id");
            file.WriteWithTab(4, "iCommandTimeout: 30);");
            file.WriteWithTab(3, "if (lQueryResult.Rows.Count > 0)");
            file.WriteWithTab(3, "{");
            file.WriteWithTab(4, "response = AutoMap<{0}>.FromDataRow(lQueryResult.Rows[0]);", trimmed);
            file.WriteWithTab(3, "}");
            file.WriteWithTab(3, "return response;");
            file.WriteWithTab(2, "}");
            file.WriteLine();
        }

        private static void WriteCreate(StreamWriter file, string iName, List<Column> objectProps)
        {
            file.WriteWithTab(2, "public bool CreateNew{0}({0} {1})", trimmed, trimmed.ToLower());
            file.WriteWithTab(2, "{");
            file.WriteWithTab(3, "return ExecuteProcedureNonQuery(\"CREATE_NEW_{0}\",", storedProcedureName);
            file.WriteWithTab(4, "new List<SqlParameter>()");
            file.WriteWithTab(4, "{");
            for (int i = 0; i < objectProps.Count; i++)
            {
                if (!objectProps[i].IsPrimaryKey)
                {
                    file.WriteWithTab(5, "new SqlParameter(\"@{0}\", {1}.{0}){2}", objectProps[i].ColumnName, trimmed.ToLower(), i == objectProps.Count ? "" : ",");
                }
            }
            file.WriteWithTab(4, "},");
            file.WriteWithTab(4, "iCommandTimeout: 60);");
            file.WriteWithTab(2, "}");
            file.WriteLine();
        }

        private static void WriteUpdate(StreamWriter file, string iName, List<Column> objectProps)
        {
            file.WriteWithTab(2, "public bool Update{0}({0} {1})", trimmed, trimmed.ToLower());
            file.WriteWithTab(2, "{");
            file.WriteWithTab(3, "return ExecuteProcedureNonQuery(\"UPDATE_{0}\",", storedProcedureName);
            file.WriteWithTab(4, "new List<SqlParameter>()");
            file.WriteWithTab(4, "{");
            for (int i = 0; i < objectProps.Count; i++)
            {
                file.WriteWithTab(5, "new SqlParameter(\"@{0}\", {1}.{0}){2}", objectProps[i].ColumnName, trimmed.ToLower(), i == objectProps.Count ? "" : ",");
            }
            file.WriteWithTab(4, "},");
            file.WriteWithTab(4, "iCommandTimeout: 60);");
            file.WriteWithTab(2, "}");
            file.WriteLine();
        }

        private static void WriteDelete(StreamWriter file, string iName)
        {
            file.WriteWithTab(2, "public bool Delete{0}(Int64 {0}Id)", trimmed);
            file.WriteWithTab(2, "{");
            file.WriteWithTab(3, "return ExecuteProcedureNonQuery(\"DELETE_{0}\",", storedProcedureName);
            file.WriteWithTab(4, "new SqlParameter(\"@{0}Id\", {0}Id), 30);", trimmed);
            file.WriteWithTab(2, "}");
            file.WriteLine();
        }

        private static string GetStoredProcedureName(string table)
        {
            StringBuilder sb = new StringBuilder();
            char[] characters = table.ToArray();
            if (characters.Count() > 1)
            {
                for (int i = 0; i < characters.Count(); i++)
                {
                    if (Char.IsUpper(characters[i]) && i > 0)
                    {
                        sb.Append("_");
                    }
                    sb.Append(characters[i].ToString().ToUpper());
                }
            }
            return sb.ToString();
        }

        private static string TranslateSqlTypeToCsharp(string iSqlType)
        {
            iSqlType = iSqlType.ToLower();
            string oType = "";
            if (iSqlType.Contains("tinyint"))
            {
                oType = "Int16";
            }
            else if (iSqlType.Contains("bigint"))
            {
                oType = "Int64";
            }
            else if (iSqlType.StartsWith("decimal"))
            {
                oType = "decimal";
            }
            else if (iSqlType.StartsWith("varchar"))
            {
                oType = "string";
            }
            else if (iSqlType.StartsWith("nvarchar"))
            {
                oType = "string";
            }
            else if (iSqlType.StartsWith("xml"))
            {
                oType = "string";
            }
            else if (iSqlType.StartsWith("datetime"))
            {
                oType = "DateTime";
            }
            else if (iSqlType.StartsWith("uniqueidentifier"))
            {
                oType = "Guid";
            }
            else if (iSqlType.StartsWith("varbinary"))
            {
                oType = "byte[]";
            }
            return oType;
        }
    }
}
