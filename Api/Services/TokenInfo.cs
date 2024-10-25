namespace EccomercePage.Api.Services
{
    public class TokenInfo
    {
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
        public string RefreshToken { get; set; }
    }
}
