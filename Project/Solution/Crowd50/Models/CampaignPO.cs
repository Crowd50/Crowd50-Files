using System;

namespace Crowd50.Models
{
    public class CampaignPO
    {
        public Int64 CampaignId { get; set; }
        public Int64 CampaignOrganizer { get; set; }
        public string CampaignName { get; set; }
        public string CampaignType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PercentageRate { get; set; }
    }

}