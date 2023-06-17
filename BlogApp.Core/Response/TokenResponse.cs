namespace BlogApp.Core.Response
{
    public class TokenResponse
    {
        public TokenResponse(string token, DateTime expireDate)
        {
            Token = token;
            ExpireDate = expireDate;
        }

        public string Token { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}
