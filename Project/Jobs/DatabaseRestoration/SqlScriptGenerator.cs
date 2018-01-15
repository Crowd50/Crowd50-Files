namespace DatabaseRestoration
{
    using DatabaseRestoration.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class SqlScriptGenerator
    {
        public static string CreateTable(string iTableName, List<Column> iColumnInfo)
        {
            return String.Format("CREATE TABLE {0}", iTableName);
        }
        public static string DropTable(string iTableName)
        {
            return String.Format("DROP TABLE {0}", iTableName);
        }

        public static string DropAndReCreate(string iTableName, List<Column> iColumnInfo)
        {
            throw new NotImplementedException();
        }



        #region Generate Script Methods
        public static string GenerateScript(ScriptType iTypeOfScript, string iTableName, List<Column> iColumnInfo)
        {
            string oResponse = "";
            switch (iTypeOfScript)
            {
                case ScriptType.Create:
                    oResponse = GenerateCreateScript(iTableName, iColumnInfo);
                    break;
                case ScriptType.Read:
                    oResponse = GenerateReadScript(iTableName, iColumnInfo);
                    break;
                case ScriptType.Update:
                    oResponse = GenerateUpdateScript(iTableName, iColumnInfo);
                    break;
                case ScriptType.Delete:
                    oResponse = GenerateDeleteScript(iTableName, iColumnInfo);
                    break;
                case ScriptType.ViewById:
                    oResponse = GenerateViewByIdScript(iTableName, iColumnInfo);
                    break;
                default:
                    break;
            }
            return oResponse;
        }

        public static string GenerateScript<T>(ScriptType iTypeOfScript, string iTableName)
        {
            string oResponse = "";
            switch (iTypeOfScript)
            {
                case ScriptType.Create:
                    oResponse = GenerateCreateScript<T>(iTableName);
                    break;
                case ScriptType.Read:
                    oResponse = GenerateReadScript<T>(iTableName);
                    break;
                case ScriptType.Update:
                    oResponse = GenerateUpdateScript<T>(iTableName);
                    break;
                case ScriptType.Delete:
                    oResponse = GenerateDeleteScript<T>(iTableName);
                    break;
                case ScriptType.ViewById:
                    oResponse = GenerateViewByIdScript<T>(iTableName);
                    break;
                default:
                    break;
            }
            return oResponse;
        }
        #endregion

        public static string GenerateStoredProcedureScript<T>(ScriptType iTypeOfScript, string iStoredProcedureName, string iTableName)
        {
            List<string> propertyNames = typeof(T).GetProperties().Select(x => x.Name).ToList();
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("CREATE PROCEDURE {0}{1}", iStoredProcedureName, Environment.NewLine));

            if (iTypeOfScript != ScriptType.Read)
            {
                sb.Append("(");
                if (iTypeOfScript == ScriptType.Delete || iTypeOfScript == ScriptType.ViewById)
                {
                    string probableId = propertyNames.Where(x => x.ToLower().Contains("id")).FirstOrDefault();
                }
                sb.Append(")");
            }

            sb.Append(String.Format("AS{0}", Environment.NewLine));
            sb.Append(String.Format("BEGIN{0}", Environment.NewLine));

            //Code for sp.
            sb.Append(GenerateScript<T>(iTypeOfScript, iTableName));

            sb.Append(String.Format("END{0}", Environment.NewLine));
            return sb.ToString();
        }

        public static string GenerateStoredProcedureScript(ScriptType iTypeOfScript, string iStoredProcedureName, string iTableName, List<Column> iColumnInfo)
        {
            iStoredProcedureName = (iStoredProcedureName.EndsWith("s", StringComparison.InvariantCultureIgnoreCase)) ? iStoredProcedureName.Substring(0, iStoredProcedureName.Length - 1) : iStoredProcedureName;

            string commandDetails = GenerateScript(iTypeOfScript, iTableName, iColumnInfo);

            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("CREATE PROCEDURE {0}{1}", iStoredProcedureName, Environment.NewLine));
            if (iTypeOfScript != ScriptType.Read)
            {
                //vbid, delete - ID only
                //update - all
                //insert - all but id

                sb.Append(String.Format("({0}", Environment.NewLine));

                int pos = 0;
                foreach (Column col in iColumnInfo)
                {
                    if ((col.IsPrimaryKey && (iTypeOfScript == ScriptType.ViewById || iTypeOfScript == ScriptType.Update || iTypeOfScript == ScriptType.Delete)) ||
                        !col.IsPrimaryKey && (iTypeOfScript == ScriptType.Update || iTypeOfScript == ScriptType.Create))
                    {
                        sb.Append(String.Format("@{0} {1}{2}{3}", col.ColumnName, col.DataType, iTypeOfScript == ScriptType.Delete || iTypeOfScript == ScriptType.ViewById || pos == iColumnInfo.Count - 1 ? "" : ",", Environment.NewLine));
                    }
                    pos++;
                }

                sb.Append(String.Format("){0}", Environment.NewLine));
            }
            else
            {
                sb.Append(Environment.NewLine);
            }
            sb.Append(String.Format("AS{0}", Environment.NewLine));
            sb.Append(String.Format("BEGIN{0}{0}", Environment.NewLine));

            sb.Append(String.Format("{0}{1}", commandDetails, Environment.NewLine));

            sb.Append(Environment.NewLine);

            sb.Append(String.Format("END{0}", Environment.NewLine));
            return sb.ToString();
        }

        public static string GenerateTableQueryScript()
        {
            return String.Format(@"
                    SELECT NAME
                    FROM SYS.tables
                    WHERE LOWER(NAME) NOT LIKE 'sysdiagrams'
                    ORDER BY OBJECT_ID ASC");
        }

        public static string GenerateDataTypeQueryScript(string iTableName)
        {
            return String.Format(@"SELECT c.name 'Column Name',
		CASE
				WHEN T.name LIKE 'binary%' THEN CONCAT('BINARY(', CASE WHEN C.MAX_LENGTH < 0 THEN 'MAX' ELSE CONCAT('', C.MAX_LENGTH, '') END, ')')
				WHEN T.name LIKE 'char%' THEN CONCAT('CHAR(', CASE WHEN C.MAX_LENGTH < 0 THEN 'MAX' ELSE CONCAT('', C.MAX_LENGTH, '') END, ')')
				WHEN T.name LIKE 'datetime2%' THEN CONCAT('DATETIME2(', C.scale, ')')
				WHEN T.name LIKE 'datetimeoffset%' THEN CONCAT('DATETIMEOFFSET(', C.scale, ')')
				WHEN T.name LIKE 'decimal%' THEN CONCAT('DECIMAL(', C.precision, ',', C.scale, ')')
				WHEN T.name LIKE 'nchar%' THEN CONCAT('NCHAR(', CASE WHEN C.MAX_LENGTH < 0 THEN 'MAX' ELSE CONCAT('', C.MAX_LENGTH / 2, '') END, ')')
				WHEN T.name LIKE 'numeric%' THEN CONCAT('NUMERIC(', C.precision, ',', C.scale, ')')
				WHEN T.name LIKE 'nvarchar%' THEN CONCAT('NVARCHAR(', CASE WHEN C.MAX_LENGTH < 0 THEN 'MAX' ELSE CONCAT('', C.MAX_LENGTH / 2, '') END, ')')
				WHEN T.name LIKE 'time%' AND T.name != 'timestamp' THEN CONCAT('TIME(', C.scale, ')')
				WHEN T.name LIKE 'varbinary%' THEN CONCAT('VARBINARY(', CASE WHEN C.MAX_LENGTH < 0 THEN 'MAX' ELSE CONCAT('', C.MAX_LENGTH / 2, '') END, ')')
				WHEN T.name LIKE 'varchar%' THEN CONCAT('VARCHAR(', CASE WHEN C.MAX_LENGTH < 0 THEN 'MAX' ELSE CONCAT('', C.MAX_LENGTH / 2, '') END, ')')
				ELSE UPPER(T.name)
			END AS DATA_TYPE,
		c.is_nullable,
		ISNULL(i.is_primary_key, 0) 'Primary Key'
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
WHERE c.object_id = OBJECT_ID('{0}'){1}", iTableName, Environment.NewLine);
        }

        #region Create Script Methods
        private static string GenerateCreateScript<T>(string iTableName)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] props = typeof(T).GetProperties();
            sb.Append(String.Format("INSERT INTO {0} (", iTableName));
            for (int i = 0; i < props.Length; i++)
            {
                if (!props[i].Name.ToLower().Contains("id"))
                {
                    sb.Append(props[i].Name + (i == props.Length - 1 ? "" : ","));
                }
            }
            sb.Append(String.Format("){0}", Environment.NewLine));
            sb.Append(String.Format("VALUES (", Environment.NewLine));
            for (int i = 0; i < props.Length; i++)
            {
                if (!props[i].Name.ToLower().Contains("id"))
                {
                    sb.Append("@" + props[i].Name + (i == props.Length - 1 ? "" : ","));
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        private static string GenerateCreateScript(string iTableName, List<Column> iColumnInfo)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("INSERT INTO {0} (", iTableName));
            for (int i = 0; i < iColumnInfo.Count; i++)
            {
                if (!iColumnInfo[i].IsPrimaryKey)
                {
                    sb.Append(iColumnInfo[i].ColumnName + (i == iColumnInfo.Count - 1 ? "" : ","));
                }
            }
            sb.Append(String.Format("){0}", Environment.NewLine));
            sb.Append(String.Format("VALUES (", Environment.NewLine));
            for (int i = 0; i < iColumnInfo.Count; i++)
            {
                if (!iColumnInfo[i].IsPrimaryKey)
                {
                    sb.Append("@" + iColumnInfo[i].ColumnName + (i == iColumnInfo.Count - 1 ? "" : ","));
                }
            }
            sb.Append(")");
            return sb.ToString();
        }
        #endregion

        #region Read Script Methods
        private static string GenerateReadScript<T>(string iTableName)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] props = typeof(T).GetProperties();
            sb.Append("SELECT ");
            for (int i = 0; i < props.Length; i++)
            {
                sb.Append(props[i].Name + (i == props.Length ? "" : ",") + Environment.NewLine);
            }
            sb.Append(String.Format("FROM {0}{1}", iTableName, Environment.NewLine));
            return sb.ToString();
        }

        private static string GenerateReadScript(string iTableName, List<Column> iColumnInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            for (int i = 0; i < iColumnInfo.Count; i++)
            {
                sb.Append(iColumnInfo[i].ColumnName + (i == iColumnInfo.Count - 1 ? "" : ",") + Environment.NewLine);
            }
            sb.Append(String.Format("FROM {0}", iTableName));
            return sb.ToString();
        }
        #endregion

        #region Update Script Methods
        private static string GenerateUpdateScript<T>(string iTableName)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] props = typeof(T).GetProperties();
            sb.Append(String.Format("UPDATE {0}{1}", iTableName, Environment.NewLine));
            sb.Append("SET ");
            bool idSet = false;
            string idColumnName = string.Empty;
            for (int i = 0; i < props.Length; i++)
            {
                if (!props[i].Name.ToLower().Contains("id"))
                {
                    sb.Append(String.Format("{0} = @{0}{1}", props[i].Name, Environment.NewLine));
                }
                else if (!idSet)
                {
                    idColumnName = props[i].Name;
                }
            }
            sb.Append(String.Format("WHERE {0} = @{0}", idColumnName, Environment.NewLine));
            return sb.ToString();
        }

        private static string GenerateUpdateScript(string iTableName, List<Column> iColumnInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("UPDATE {0}{1}", iTableName, Environment.NewLine));
            sb.Append("SET ");
            bool idSet = false;
            string idColumnName = string.Empty;
            for (int i = 0; i < iColumnInfo.Count; i++)
            {
                if (!iColumnInfo[i].IsPrimaryKey)
                {
                    sb.Append(String.Format("{0} = @{0}{1}{2}", iColumnInfo[i].ColumnName, i == iColumnInfo.Count - 1 ? "" : ",", Environment.NewLine));

                }
                else if (!idSet)
                {
                    idColumnName = iColumnInfo[i].ColumnName;
                }
            }
            sb.Append(String.Format("WHERE {0} = @{0}", idColumnName, Environment.NewLine));
            return sb.ToString();
        }
        #endregion

        #region Delete Script Methods
        private static string GenerateDeleteScript<T>(string iTableName)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] props = typeof(T).GetProperties();
            sb.Append(String.Format("DELETE{0}", Environment.NewLine));
            sb.Append(String.Format("FROM {0}{1}", iTableName, Environment.NewLine));
            string idColumnName = string.Empty;
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].Name.ToLower().Contains("id"))
                {
                    sb.Append(String.Format("WHERE {0} = @{0}", props[i].Name, Environment.NewLine));
                    break;
                }
            }
            return sb.ToString();
        }

        private static string GenerateDeleteScript(string iTableName, List<Column> iColumnInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("DELETE{0}", Environment.NewLine));
            sb.Append(String.Format("FROM {0}{1}", iTableName, Environment.NewLine));
            string idColumnName = string.Empty;
            for (int i = 0; i < iColumnInfo.Count; i++)
            {
                if (iColumnInfo[i].IsPrimaryKey)
                {
                    sb.Append(String.Format("WHERE {0} = @{0}", iColumnInfo[i].ColumnName, Environment.NewLine));
                    break;
                }
            }
            return sb.ToString();
        }
        #endregion

        #region View By Id Script Methods
        private static string GenerateViewByIdScript<T>(string iTableName)
        {
            string baseCommand = GenerateReadScript<T>(iTableName);
            StringBuilder sb = new StringBuilder(baseCommand);

            string iIdColumnName = "";
            PropertyInfo[] props = typeof(T).GetProperties();
            iTableName = iTableName.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) ? iTableName.Substring(0, iTableName.Length - 1) : iTableName;

            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].Name.ToLower().Contains(iTableName.ToLower()) && props[i].Name.ToLower().Contains("id"))
                {
                    iIdColumnName = props[i].Name;
                    break;
                }
            }

            sb.Append(String.Format("WHERE {0} = @{0}", iIdColumnName));
            return sb.ToString();
        }

        private static string GenerateViewByIdScript(string iTableName, List<Column> iColumnInfo)
        {
            string baseCommand = GenerateReadScript(iTableName, iColumnInfo);
            StringBuilder sb = new StringBuilder(baseCommand);

            string iIdColumnName = "";
            for (int i = 0; i < iColumnInfo.Count; i++)
            {
                if (iColumnInfo[i].IsPrimaryKey)
                {
                    iIdColumnName = iColumnInfo[i].ColumnName;
                    break;
                }
            }

            sb.Append(String.Format("{0}WHERE {1} = @{1}", Environment.NewLine, iIdColumnName));
            return sb.ToString();
        }
        #endregion
    }

    public enum ScriptType
    {
        Create,
        Read,
        Update,
        Delete,
        ViewById
    }
}
