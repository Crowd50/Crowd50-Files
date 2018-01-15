using System.ComponentModel.DataAnnotations;

namespace Crowd50.Models
{
    public class LoginPO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}