namespace EccomercePage.Api.Services
{
    public class TokenInfo
    {
        public string TokenType { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public int ExpireIn { get; set; } = 0;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
