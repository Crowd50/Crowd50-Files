namespace Crowd50_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Ticket
    {
        public Int64 TicketId { get; set; }
        public Int64 CampaignId { get; set; }
        public Int64 UserId { get; set; }
        public DateTime GenerationDate { get; set; }
        public Guid ValidationCode { get; set; }
    }
}
