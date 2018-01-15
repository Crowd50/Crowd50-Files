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

    public class PrizeDataAccess : CommandExecutor 

    {
        public PrizeDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<Prize> ViewAllPrizes()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_PRIZE");
            return AutoMap<Prize>.FromDataTable(lQueryResult).ToList();
        }

        public Prize ViewPrizeById(Int64 PrizeId)
        {
            Prize response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_PRIZE_BY_ID",
                new SqlParameter("@PrizeId", PrizeId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<Prize>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewPrize(Prize prize)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_PRIZE",
                new List<SqlParameter>()
                {
                    new SqlParameter("@CampaignId", prize.CampaignId),
                    new SqlParameter("@CampaignTierId", prize.CampaignTierId),
                    new SqlParameter("@Amount", prize.Amount),
                    new SqlParameter("@Placement", prize.Placement),
                },
                iCommandTimeout: 60);
        }

        public bool UpdatePrize(Prize prize)
        {
            return ExecuteProcedureNonQuery("UPDATE_PRIZE",
                new List<SqlParameter>()
                {
                    new SqlParameter("@PrizeId", prize.PrizeId),
                    new SqlParameter("@CampaignId", prize.CampaignId),
                    new SqlParameter("@CampaignTierId", prize.CampaignTierId),
                    new SqlParameter("@Amount", prize.Amount),
                    new SqlParameter("@Placement", prize.Placement),
                },
                iCommandTimeout: 60);
        }

        public bool DeletePrize(Int64 PrizeId)
        {
            return ExecuteProcedureNonQuery("DELETE_PRIZE",
                new SqlParameter("@PrizeId", PrizeId), 30);
        }

    }
}
