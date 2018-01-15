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

    public class PermissionDataAccess : CommandExecutor 

    {
        public PermissionDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<Permission> ViewAllPermissions()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_PERMISSION");
            return AutoMap<Permission>.FromDataTable(lQueryResult).ToList();
        }

        public Permission ViewPermissionById(Int64 PermissionId)
        {
            Permission response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_PERMISSION_BY_ID",
                new SqlParameter("@PermissionId", PermissionId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<Permission>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewPermission(Permission permission)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_PERMISSION",
                new List<SqlParameter>()
                {
                    new SqlParameter("@Name", permission.Name),
                    new SqlParameter("@Rank", permission.Rank),
                },
                iCommandTimeout: 60);
        }

        public bool UpdatePermission(Permission permission)
        {
            return ExecuteProcedureNonQuery("UPDATE_PERMISSION",
                new List<SqlParameter>()
                {
                    new SqlParameter("@PermissionId", permission.PermissionId),
                    new SqlParameter("@Name", permission.Name),
                    new SqlParameter("@Rank", permission.Rank),
                },
                iCommandTimeout: 60);
        }

        public bool DeletePermission(Int64 PermissionId)
        {
            return ExecuteProcedureNonQuery("DELETE_PERMISSION",
                new SqlParameter("@PermissionId", PermissionId), 30);
        }

    }
}
