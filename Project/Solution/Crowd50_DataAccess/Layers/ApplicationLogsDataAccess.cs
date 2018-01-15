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

    public class ApplicationLogDataAccess : CommandExecutor 

    {
        public ApplicationLogDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<ApplicationLog> ViewAllApplicationLogs()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_APPLICATION_LOG");
            return AutoMap<ApplicationLog>.FromDataTable(lQueryResult).ToList();
        }

        public ApplicationLog ViewApplicationLogById(Int64 ApplicationLogId)
        {
            ApplicationLog response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_APPLICATION_LOG_BY_ID",
                new SqlParameter("@ApplicationLogId", ApplicationLogId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<ApplicationLog>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewApplicationLog(ApplicationLog applicationlog)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_APPLICATION_LOG",
                new List<SqlParameter>()
                {
                    new SqlParameter("@DateAndTime", applicationlog.DateAndTime),
                    new SqlParameter("@User", applicationlog.User),
                    new SqlParameter("@Priority", applicationlog.Priority),
                    new SqlParameter("@Message", applicationlog.Message),
                },
                iCommandTimeout: 60);
        }

        public bool UpdateApplicationLog(ApplicationLog applicationlog)
        {
            return ExecuteProcedureNonQuery("UPDATE_APPLICATION_LOG",
                new List<SqlParameter>()
                {
                    new SqlParameter("@ApplicationLogId", applicationlog.ApplicationLogId),
                    new SqlParameter("@DateAndTime", applicationlog.DateAndTime),
                    new SqlParameter("@User", applicationlog.User),
                    new SqlParameter("@Priority", applicationlog.Priority),
                    new SqlParameter("@Message", applicationlog.Message),
                },
                iCommandTimeout: 60);
        }

        public bool DeleteApplicationLog(Int64 ApplicationLogId)
        {
            return ExecuteProcedureNonQuery("DELETE_APPLICATION_LOG",
                new SqlParameter("@ApplicationLogId", ApplicationLogId), 30);
        }

    }
}
