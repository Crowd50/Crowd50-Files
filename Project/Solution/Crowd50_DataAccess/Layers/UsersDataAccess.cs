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

    public class UserDataAccess : CommandExecutor 

    {
        public UserDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<User> ViewAllUsers()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_USER");
            return AutoMap<User>.FromDataTable(lQueryResult).ToList();
        }

        public User ViewUserById(Int64 UserId)
        {
            User response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_USER_BY_ID",
                new SqlParameter("@UserId", UserId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<User>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewUser(User user)
        {
            return ExecuteProcedureNonQuery("CREATE_USER",
                new List<SqlParameter>()
                {
                    new SqlParameter("@FirstName", user.FirstName),
                    new SqlParameter("@LastName", user.LastName),
                    new SqlParameter("@PrependSalt", user.PrependSalt),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@AppendSalt", user.AppendSalt),
                    new SqlParameter("@EmailAddress", user.EmailAddress),
                    new SqlParameter("@LastLogin", user.LastLogin),
                },
                iCommandTimeout: 60);
        }

        public bool UpdateUser(User user)
        {
            return ExecuteProcedureNonQuery("UPDATE_USER",
                new List<SqlParameter>()
                {
                    new SqlParameter("@UserId", user.UserId),
                    new SqlParameter("@FirstName", user.FirstName),
                    new SqlParameter("@LastName", user.LastName),
                    new SqlParameter("@PrependSalt", user.PrependSalt),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@AppendSalt", user.AppendSalt),
                    new SqlParameter("@EmailAddress", user.EmailAddress),
                    new SqlParameter("@LastLogin", user.LastLogin),
                },
                iCommandTimeout: 60);
        }

        public bool DeleteUser(Int64 UserId)
        {
            return ExecuteProcedureNonQuery("DELETE_USER",
                new SqlParameter("@UserId", UserId), 30);
        }

        public User ViewUserByUsername(string emailAddress)
        {
            User response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_USER_BY_USERNAME",
                new SqlParameter("@EmailAddress", emailAddress),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<User>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }
    }
}
