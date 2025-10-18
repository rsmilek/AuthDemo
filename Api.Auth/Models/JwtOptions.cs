namespace Api.Auth.Models
{
    /// <summary>
    /// JWT options model to hold configuration settings from appsettings.json and used for JWT token generation.
    /// </summary>
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string Secret { get; set; } = string.Empty;
    }
}
