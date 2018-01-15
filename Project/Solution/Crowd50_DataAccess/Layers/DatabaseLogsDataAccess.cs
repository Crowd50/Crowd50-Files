namespace Crowd50_DataAccess
{
    using System;
    using System.Linq;
    using System.Data;
    using AutoMapping;
    using CommandExecution;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using Crowd50_DataAccess.Models;

    public class DatabaseLogDataAccess : CommandExecutor 

    {
        public DatabaseLogDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<DatabaseLog> ViewAllDatabaseLogs()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_DATABASE_LOG");
            return AutoMap<DatabaseLog>.FromDataTable(lQueryResult).ToList();
        }

        public DatabaseLog ViewDatabaseLogById(Int64 DatabaseLogId)
        {
            DatabaseLog response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_DATABASE_LOG_BY_ID",
                new SqlParameter("@DatabaseLogId", DatabaseLogId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<DatabaseLog>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewDatabaseLog(DatabaseLog databaselog)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_DATABASE_LOG",
                new List<SqlParameter>()
                {
                    new SqlParameter("@DateAndTime", databaselog.DateAndTime),
                    new SqlParameter("@User", databaselog.User),
                    new SqlParameter("@Table", databaselog.Table),
                    new SqlParameter("@ActionType", databaselog.ActionType),
                    new SqlParameter("@Before", databaselog.Before),
                    new SqlParameter("@After", databaselog.After),
                },
                iCommandTimeout: 60);
        }

        public bool UpdateDatabaseLog(DatabaseLog databaselog)
        {
            return ExecuteProcedureNonQuery("UPDATE_DATABASE_LOG",
                new List<SqlParameter>()
                {
                    new SqlParameter("@DatabaseLogId", databaselog.DatabaseLogId),
                    new SqlParameter("@DateAndTime", databaselog.DateAndTime),
                    new SqlParameter("@User", databaselog.User),
                    new SqlParameter("@Table", databaselog.Table),
                    new SqlParameter("@ActionType", databaselog.ActionType),
                    new SqlParameter("@Before", databaselog.Before),
                    new SqlParameter("@After", databaselog.After),
                },
                iCommandTimeout: 60);
        }

        public bool DeleteDatabaseLog(Int64 DatabaseLogId)
        {
            return ExecuteProcedureNonQuery("DELETE_DATABASE_LOG",
                new SqlParameter("@DatabaseLogId", DatabaseLogId), 30);
        }

    }
}
