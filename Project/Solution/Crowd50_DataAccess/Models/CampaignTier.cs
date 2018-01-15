namespace Crowd50_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CampaignTier
    {
        public Int64 CampaignTierId { get; set; }
        public Int64 CampaignId { get; set; }
        public decimal TierAmount { get; set; }
        public decimal FirstPlacePrize { get; set; }
        public decimal SecondPlacePrize { get; set; }
        public decimal ThirdPlacePrize { get; set; }
        public decimal FourthPlacePrize { get; set; }
        public decimal FifthPlacePrize { get; set; }
        public decimal SixthPlacePrize { get; set; }
    }
}
