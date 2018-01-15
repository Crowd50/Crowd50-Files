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

    public class CampaignDataAccess : CommandExecutor 

    {
        public CampaignDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<Campaign> ViewAllCampaigns()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_CAMPAIGN");
            return AutoMap<Campaign>.FromDataTable(lQueryResult).ToList();
        }

        public Campaign ViewCampaignById(Int64 CampaignId)
        {
            Campaign response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_CAMPAIGN_BY_ID",
                new SqlParameter("@CampaignId", CampaignId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<Campaign>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewCampaign(Campaign campaign)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_CAMPAIGN",
                new List<SqlParameter>()
                {
                    new SqlParameter("@CampaignOrganizer", campaign.CampaignOrganizer),
                    new SqlParameter("@CampaignName", campaign.CampaignName),
                    new SqlParameter("@CampaignType", campaign.CampaignType),
                    new SqlParameter("@StartDate", campaign.StartDate),
                    new SqlParameter("@EndDate", campaign.EndDate),
                    new SqlParameter("@PercentageRate", campaign.PercentageRate),
                },
                iCommandTimeout: 60);
        }

        public bool UpdateCampaign(Campaign campaign)
        {
            return ExecuteProcedureNonQuery("UPDATE_CAMPAIGN",
                new List<SqlParameter>()
                {
                    new SqlParameter("@CampaignId", campaign.CampaignId),
                    new SqlParameter("@CampaignOrganizer", campaign.CampaignOrganizer),
                    new SqlParameter("@CampaignName", campaign.CampaignName),
                    new SqlParameter("@CampaignType", campaign.CampaignType),
                    new SqlParameter("@StartDate", campaign.StartDate),
                    new SqlParameter("@EndDate", campaign.EndDate),
                    new SqlParameter("@PercentageRate", campaign.PercentageRate),
                },
                iCommandTimeout: 60);
        }

        public bool DeleteCampaign(Int64 CampaignId)
        {
            return ExecuteProcedureNonQuery("DELETE_CAMPAIGN",
                new SqlParameter("@CampaignId", CampaignId), 30);
        }

    }
}
