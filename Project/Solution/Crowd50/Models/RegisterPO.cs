using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Crowd50.Models
{
    public class RegisterPO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string RegistrationPassword { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public DateTime LastLogin { get; set; }
    }
}