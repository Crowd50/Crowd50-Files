namespace Crowd50_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Permission
    {
        public Int64 PermissionId { get; set; }
        public string Name { get; set; }
        public Int16 Rank { get; set; }
    }
}
