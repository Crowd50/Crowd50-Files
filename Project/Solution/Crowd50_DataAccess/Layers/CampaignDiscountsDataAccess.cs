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

    public class CampaignDiscountDataAccess : CommandExecutor 

    {
        public CampaignDiscountDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<CampaignDiscount> ViewAllCampaignDiscounts()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_CAMPAIGN_DISCOUNT");
            return AutoMap<CampaignDiscount>.FromDataTable(lQueryResult).ToList();
        }

        public CampaignDiscount ViewCampaignDiscountById(Int64 CampaignDiscountId)
        {
            CampaignDiscount response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_CAMPAIGN_DISCOUNT_BY_ID",
                new SqlParameter("@CampaignDiscountId", CampaignDiscountId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<CampaignDiscount>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewCampaignDiscount(CampaignDiscount campaigndiscount)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_CAMPAIGN_DISCOUNT",
                new List<SqlParameter>()
                {
                    new SqlParameter("@CampaignId", campaigndiscount.CampaignId),
                    new SqlParameter("@AmountOfTickets", campaigndiscount.AmountOfTickets),
                    new SqlParameter("@Price", campaigndiscount.Price),
                },
                iCommandTimeout: 60);
        }

        public bool UpdateCampaignDiscount(CampaignDiscount campaigndiscount)
        {
            return ExecuteProcedureNonQuery("UPDATE_CAMPAIGN_DISCOUNT",
                new List<SqlParameter>()
                {
                    new SqlParameter("@CampaignDiscountId", campaigndiscount.CampaignDiscountId),
                    new SqlParameter("@CampaignId", campaigndiscount.CampaignId),
                    new SqlParameter("@AmountOfTickets", campaigndiscount.AmountOfTickets),
                    new SqlParameter("@Price", campaigndiscount.Price),
                },
                iCommandTimeout: 60);
        }

        public bool DeleteCampaignDiscount(Int64 CampaignDiscountId)
        {
            return ExecuteProcedureNonQuery("DELETE_CAMPAIGN_DISCOUNT",
                new SqlParameter("@CampaignDiscountId", CampaignDiscountId), 30);
        }

    }
}
