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

    public class EmailVerificationCodeDataAccess : CommandExecutor 

    {
        public EmailVerificationCodeDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<EmailVerificationCode> ViewAllEmailVerificationCodes()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_EMAIL_VERIFICATION_CODE");
            return AutoMap<EmailVerificationCode>.FromDataTable(lQueryResult).ToList();
        }

        public EmailVerificationCode ViewEmailVerificationCodeById(Int64 EmailVerificationCodeId)
        {
            EmailVerificationCode response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_EMAIL_VERIFICATION_CODE_BY_ID",
                new SqlParameter("@EmailVerificationCodeId", EmailVerificationCodeId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<EmailVerificationCode>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewEmailVerificationCode(EmailVerificationCode emailverificationcode)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_EMAIL_VERIFICATION_CODE",
                new List<SqlParameter>()
                {
                    new SqlParameter("@UserId", emailverificationcode.UserId),
                    new SqlParameter("@DateCreated", emailverificationcode.DateCreated),
                    new SqlParameter("@VerificationCode", emailverificationcode.VerificationCode),
                    new SqlParameter("@ExpirationDate", emailverificationcode.ExpirationDate),
                },
                iCommandTimeout: 60);
        }

        public bool UpdateEmailVerificationCode(EmailVerificationCode emailverificationcode)
        {
            return ExecuteProcedureNonQuery("UPDATE_EMAIL_VERIFICATION_CODE",
                new List<SqlParameter>()
                {
                    new SqlParameter("@EmailVerificationCodeId", emailverificationcode.EmailVerificationCodeId),
                    new SqlParameter("@UserId", emailverificationcode.UserId),
                    new SqlParameter("@DateCreated", emailverificationcode.DateCreated),
                    new SqlParameter("@VerificationCode", emailverificationcode.VerificationCode),
                    new SqlParameter("@ExpirationDate", emailverificationcode.ExpirationDate),
                },
                iCommandTimeout: 60);
        }

        public bool DeleteEmailVerificationCode(Int64 EmailVerificationCodeId)
        {
            return ExecuteProcedureNonQuery("DELETE_EMAIL_VERIFICATION_CODE",
                new SqlParameter("@EmailVerificationCodeId", EmailVerificationCodeId), 30);
        }

    }
}
