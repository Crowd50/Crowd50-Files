namespace Crowd50_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PermissionAssignment
    {
        public Int64 PermissionAssignmentId { get; set; }
        public Int64 PermissionId { get; set; }
        public Int64 UserId { get; set; }
    }
}
