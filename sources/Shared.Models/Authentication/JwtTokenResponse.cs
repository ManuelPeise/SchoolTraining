namespace Shared.Models.Authentication
{
    public class JwtTokenResponse
    {
        public int UserId { get; set; }
        public string Jwt { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string ExpiresAt { get; set; } = string.Empty;
    }
}
