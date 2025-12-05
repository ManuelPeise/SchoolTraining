namespace Shared.Models.Authentication
{
    public class RefreshTokenResponse
    {
        public string JwtToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
