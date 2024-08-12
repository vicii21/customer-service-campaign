using CustomerServiceCampaign.DataAccess;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerServiceCampaign.API.Jwt
{
    public class JwtManager
    {
        private readonly CustomerServiceCampaignContext _context;
        private readonly JwtSettings _settings;
        private readonly ITokenStorage _storage;

        public JwtManager(
            CustomerServiceCampaignContext context, JwtSettings settings, ITokenStorage storage)
        {
            _context = context;
            _settings = settings;
            _storage = storage;
        }

        public string MakeToken(string email, string password)
        {
            var credentials = _context.Credentials
                               .FirstOrDefault(x => x.Email == email && x.IsActive);

            var verified = BCrypt.Net.BCrypt.Verify(password, credentials.Password);

            if (credentials == null || !credentials.Person.IsActive || !verified)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            int id = credentials.ID;
            List<int> useCases = credentials.Person.PersonUseCases.Select(x => x.UseCaseId).ToList();

            //Header.Payload.Signature

            var tokenId = Guid.NewGuid().ToString();

            _storage.AddToken(tokenId);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, tokenId, ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Iss, _settings.Issuer, ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim("Id", id.ToString()),
                //new Claim("FullName", credentials.Person.Name),
                new Claim("Email", credentials.Email),
                new Claim("UseCases", JsonConvert.SerializeObject(useCases))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: "Any",
                claims: claims,
                notBefore: now,
                expires: now.AddSeconds(_settings.DurationInSeconds),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
