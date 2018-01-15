namespace DatabaseRestoration
{
    #region using statements
    using DatabaseRestoration.Display;
    using DatabaseRestoration.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    #endregion

    class Program
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;

        private static NameValueCollection Paths = new NameValueCollection();

        private static void Init()
        {
            Paths = new NameValueCollection();
            Paths["dataAccessProject"] = ConfigurationManager.AppSettings["dataAccessProject"];
            Paths["businessLogicProject"] = ConfigurationManager.AppSettings["businessLogicProject"];
            Paths["mvcProject"] = ConfigurationManager.AppSettings["mvcProject"];
        }
        static void Main(string[] args)
        {
            Init();
            bool menuRunning = true;
            //Menu
            while (menuRunning)
            {
                int userChoice = Prompts.Menu();
                switch (userChoice)
                {
                    case 1:
                        CreateBasicCrud();
                        break;

                    case 2:
                        GenerateLayers();
                        break;

                    case 3:
                        GenerateModels();
                        break;

                    case 4:
                        menuRunning = false;
                        break;
                    default:
                        Console.WriteLine("I'm sorry, but that has not been implemented yet.");
                        break;
                }
            }
        }

        private static void GenerateLayers()
        {
            List<string> tables = ObtainTableNames();
            foreach (string table in tables)
            {
                List<Column> properties = ObtainTableInfo(table);
                FileCreator.CreateLayer("Crowd50_DataAccess", "DataAccess", table, properties);
            }
        }

        private static void GenerateModels()
        {
            List<string> tables = ObtainTableNames();
            foreach (string table in tables)
            {
                List<Column> info = ObtainTableInfo(table);
                FileCreator.CreateModel("Crowd50_DataAccess.Models", table, info);
            }
        }

        private static void CreateBasicCrud()
        {
            List<string> tables = ObtainTableNames();
            foreach (string table in tables)
            {
                //Create procedure name.
                List<Column> columnInfo = ObtainTableInfo(table);

                string createStoredProcedure = SqlScriptGenerator.GenerateStoredProcedureScript(ScriptType.Create, "CREATE_" + SqlizeTable(table), table, columnInfo);
                ExecuteCommand(createStoredProcedure, 30);
                string readStoredProcedure = SqlScriptGenerator.GenerateStoredProcedureScript(ScriptType.Read, "VIEW_ALL_" + SqlizeTable(table), table, columnInfo);
                ExecuteCommand(readStoredProcedure, 30);
                string updateStoredProcedure = SqlScriptGenerator.GenerateStoredProcedureScript(ScriptType.Update, "UPDATE_" + SqlizeTable(table), table, columnInfo);
                ExecuteCommand(updateStoredProcedure, 30);
                string deleteStoredProcedure = SqlScriptGenerator.GenerateStoredProcedureScript(ScriptType.Delete, "DELETE_" + SqlizeTable(table), table, columnInfo);
                ExecuteCommand(deleteStoredProcedure, 30);
                string vbidStoredProcedure = SqlScriptGenerator.GenerateStoredProcedureScript(ScriptType.ViewById, "VIEW_" + SqlizeTable(table) + "_BY_ID", table, columnInfo);
                ExecuteCommand(vbidStoredProcedure, 30);
            }
        }

        private static string SqlizeTable(string table)
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

        private static List<string> ObtainTableNames()
        {
            List<string> oResponse = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(SqlScriptGenerator.GenerateTableQueryScript(), connection);
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        oResponse.Add(reader.GetString(0));
                    }
                    reader.Close();
                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return oResponse;
        }

        private static List<Column> ObtainTableInfo(string iTableName)
        {
            List<Column> oResponse = new List<Column>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(SqlScriptGenerator.GenerateDataTypeQueryScript(iTableName), connection);
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 60;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        oResponse.Add(new Column
                        {
                            ColumnName = reader.GetString(0),
                            DataType = reader.GetString(1),
                            IsNullable = reader.GetBoolean(2),
                            IsPrimaryKey = reader.GetBoolean(3)
                        });
                    }
                    reader.Close();
                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            return oResponse;
        }

        private static void ExecuteCommand(string iCommand, int iTimeout)
        {
            Console.Clear();
            Console.WriteLine(iCommand);
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(iCommand, connection);
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = iTimeout;

                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
            Thread.Sleep(200);
        }
    }

    public static class extensions
    {
        public static string CreateTabs(int tabCount)
        {
            StringBuilder tabsBuilder = new StringBuilder();
            for (int i = 0; i < tabCount; i++)
            {
                tabsBuilder.Append("    ");
            }
            return tabsBuilder.ToString();
        }

        public static void WriteWithTab(this StreamWriter writer, int tabs, string value, params object[] args)
        {
            string tab = CreateTabs(tabs) + value;

            if (args.Count() > 0)
            {
                writer.WriteLine(String.Format(tab, args));
            }
            else
            {
                writer.WriteLine(tab);
            }
        }
    }
}

//1.) Generate Database Creation Script.
//Do you want to include the stored procedures with the script?
//1a.) Yes
//2b.) No

//2.) Create the basic CRUD stored procedures for the tables that exist.

//3.) Backup the database.

//4.) Restore the database to a previous backup.
//4a.) Default Location.
//4b.) Custom Location.

//5.) Exit