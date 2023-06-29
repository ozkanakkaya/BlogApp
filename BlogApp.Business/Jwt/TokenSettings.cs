namespace BlogApp.Business.Jwt
{
    public class TokenSettings
    {
        //public const string Issuer = "http://localhost";
        //public const string Audience = "http://localhost";
        //public const string Key = "ozkanozkanozkan1.";
        //public const int Expire = 240;
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int Expire { get; set; }
    }
}
