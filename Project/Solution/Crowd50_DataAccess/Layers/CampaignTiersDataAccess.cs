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

    public class CampaignTierDataAccess : CommandExecutor 

    {
        public CampaignTierDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<CampaignTier> ViewAllCampaignTiers()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_CAMPAIGN_TIER");
            return AutoMap<CampaignTier>.FromDataTable(lQueryResult).ToList();
        }

        public CampaignTier ViewCampaignTierById(Int64 CampaignTierId)
        {
            CampaignTier response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_CAMPAIGN_TIER_BY_ID",
                new SqlParameter("@CampaignTierId", CampaignTierId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<CampaignTier>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewCampaignTier(CampaignTier campaigntier)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_CAMPAIGN_TIER",
                new List<SqlParameter>()
                {
                    new SqlParameter("@CampaignId", campaigntier.CampaignId),
                    new SqlParameter("@TierAmount", campaigntier.TierAmount),
                    new SqlParameter("@FirstPlacePrize", campaigntier.FirstPlacePrize),
                    new SqlParameter("@SecondPlacePrize", campaigntier.SecondPlacePrize),
                    new SqlParameter("@ThirdPlacePrize", campaigntier.ThirdPlacePrize),
                    new SqlParameter("@FourthPlacePrize", campaigntier.FourthPlacePrize),
                    new SqlParameter("@FifthPlacePrize", campaigntier.FifthPlacePrize),
                    new SqlParameter("@SixthPlacePrize", campaigntier.SixthPlacePrize),
                },
                iCommandTimeout: 60);
        }

        public bool UpdateCampaignTier(CampaignTier campaigntier)
        {
            return ExecuteProcedureNonQuery("UPDATE_CAMPAIGN_TIER",
                new List<SqlParameter>()
                {
                    new SqlParameter("@CampaignTierId", campaigntier.CampaignTierId),
                    new SqlParameter("@CampaignId", campaigntier.CampaignId),
                    new SqlParameter("@TierAmount", campaigntier.TierAmount),
                    new SqlParameter("@FirstPlacePrize", campaigntier.FirstPlacePrize),
                    new SqlParameter("@SecondPlacePrize", campaigntier.SecondPlacePrize),
                    new SqlParameter("@ThirdPlacePrize", campaigntier.ThirdPlacePrize),
                    new SqlParameter("@FourthPlacePrize", campaigntier.FourthPlacePrize),
                    new SqlParameter("@FifthPlacePrize", campaigntier.FifthPlacePrize),
                    new SqlParameter("@SixthPlacePrize", campaigntier.SixthPlacePrize),
                },
                iCommandTimeout: 60);
        }

        public bool DeleteCampaignTier(Int64 CampaignTierId)
        {
            return ExecuteProcedureNonQuery("DELETE_CAMPAIGN_TIER",
                new SqlParameter("@CampaignTierId", CampaignTierId), 30);
        }

    }
}
