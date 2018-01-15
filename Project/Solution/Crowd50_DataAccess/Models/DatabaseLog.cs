namespace Crowd50_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DatabaseLog
    {
        public Int64 DatabaseLogId { get; set; }
        public DateTime DateAndTime { get; set; }
        public Int64 User { get; set; }
        public string Table { get; set; }
        public string ActionType { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
    }
}
