namespace MVCKhumaloCraftFinal4.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? TwoFactorCode { get; set; }
    }
}