using System.ComponentModel.DataAnnotations;

namespace MVCKhumaloCraftFinal4.Models
{
    public class User
    {
        [Key]
        public int userID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool Enable2FA { get; set; } = false; 

        public string? TwoFactorCode { get; set; } 

        public DateTime? TwoFactorCodeExpiration { get; set; } 
    }
}
