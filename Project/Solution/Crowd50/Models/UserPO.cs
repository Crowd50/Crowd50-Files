using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crowd50.Models
{
    public class UserPO
    {
        public Int64 UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrependSalt { get; set; }
        public byte[] Password { get; set; }
        public string AppendSalt { get; set; }
        public string EmailAddress { get; set; }
        public DateTime LastLogin { get; set; }
    }
}