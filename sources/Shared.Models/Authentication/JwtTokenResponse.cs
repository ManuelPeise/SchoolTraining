namespace Shared.Models.Authentication
{
    public class JwtTokenResponse
    {
        public string Jwt { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpireSeconds { get; set; }
    }
}
