namespace Shared.Models.Authentication
{
    public class LoginModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public bool Remember { get; set; }
    }
}
