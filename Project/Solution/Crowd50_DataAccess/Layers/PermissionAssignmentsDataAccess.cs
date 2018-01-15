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

    public class PermissionAssignmentDataAccess : CommandExecutor 

    {
        public PermissionAssignmentDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<PermissionAssignment> ViewAllPermissionAssignments()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_PERMISSION_ASSIGNMENT");
            return AutoMap<PermissionAssignment>.FromDataTable(lQueryResult).ToList();
        }

        public PermissionAssignment ViewPermissionAssignmentById(Int64 PermissionAssignmentId)
        {
            PermissionAssignment response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_PERMISSION_ASSIGNMENT_BY_ID",
                new SqlParameter("@PermissionAssignmentId", PermissionAssignmentId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<PermissionAssignment>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewPermissionAssignment(PermissionAssignment permissionassignment)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_PERMISSION_ASSIGNMENT",
                new List<SqlParameter>()
                {
                    new SqlParameter("@PermissionId", permissionassignment.PermissionId),
                    new SqlParameter("@UserId", permissionassignment.UserId),
                },
                iCommandTimeout: 60);
        }

        public bool UpdatePermissionAssignment(PermissionAssignment permissionassignment)
        {
            return ExecuteProcedureNonQuery("UPDATE_PERMISSION_ASSIGNMENT",
                new List<SqlParameter>()
                {
                    new SqlParameter("@PermissionAssignmentId", permissionassignment.PermissionAssignmentId),
                    new SqlParameter("@PermissionId", permissionassignment.PermissionId),
                    new SqlParameter("@UserId", permissionassignment.UserId),
                },
                iCommandTimeout: 60);
        }

        public bool DeletePermissionAssignment(Int64 PermissionAssignmentId)
        {
            return ExecuteProcedureNonQuery("DELETE_PERMISSION_ASSIGNMENT",
                new SqlParameter("@PermissionAssignmentId", PermissionAssignmentId), 30);
        }

    }
}
