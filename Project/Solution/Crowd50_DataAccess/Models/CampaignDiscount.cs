namespace Crowd50_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CampaignDiscount
    {
        public Int64 CampaignDiscountId { get; set; }
        public Int64 CampaignId { get; set; }
        public Int16 AmountOfTickets { get; set; }
        public decimal Price { get; set; }
    }
}
