using CustomerServiceCampaign.Application.Actor;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace CustomerServiceCampaign.API.Jwt
{
    public class ActorFetch : IActorFetch
    {
        private string _auth;

        public ActorFetch(string auth)
        {
            _auth = auth;
        }

        public IApplicationActor Fetch()
        {
            if (string.IsNullOrEmpty(_auth) || !_auth.StartsWith("Bearer "))
            {
                return new UnauthorizedActor();
            }

            string token = _auth.Substring("Bearer ".Length);
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken tokenObj = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claims = tokenObj.Claims;

            var claim = claims.First(x => x.Type == "jti").Value;
            if ( claim == null )
            {
                return new UnauthorizedActor();
            }

            var jwtActor = new JwtActor
            {
                Id = int.TryParse(claims.FirstOrDefault(ja => ja.Type == "Id")?.Value, out int id) ? id : 0,
                FullName = claims.FirstOrDefault(ja => ja.Type == "FullName")?.Value ?? "Unknown",
                AllowedUseCases = JsonConvert.DeserializeObject<List<int>>(
                    claims.FirstOrDefault(ja => ja.Type == "UseCaseIds")?.Value ?? "[]")
            };

            return jwtActor;
        }
    }
}
