namespace Crowd50_DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EmailVerificationCode
    {
        public Int64 EmailVerificationCodeId { get; set; }
        public Int64 UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid VerificationCode { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
