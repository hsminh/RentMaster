namespace RentMaster.Core.Auth.Models
{
    public class LoginRequest
    {
        public string Gmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}