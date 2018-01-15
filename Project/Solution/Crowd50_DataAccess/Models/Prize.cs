namespace Crowd50_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Prize
    {
        public Int64 PrizeId { get; set; }
        public Int64 CampaignId { get; set; }
        public Int64 CampaignTierId { get; set; }
        public decimal Amount { get; set; }
        public string Placement { get; set; }
    }
}
