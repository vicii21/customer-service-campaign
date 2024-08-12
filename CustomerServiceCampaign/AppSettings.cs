namespace CustomerServiceCampaign
{
    public class AppSettings
    {
        public JwtSettings Jwt { get; set; }
        public string ConnectionString { get; set; }
    }
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public int DurationInSeconds { get; set; }
        public string Issuer { get; set; }
    }
}
