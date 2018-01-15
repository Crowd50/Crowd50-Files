namespace Crowd50_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationLog
    {
        public Int64 ApplicationLogId { get; set; }
        public DateTime DateAndTime { get; set; }
        public Int64 User { get; set; }
        public Int16 Priority { get; set; }
        public string Message { get; set; }
    }
}
