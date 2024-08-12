using CustomerServiceCampaign.Application.Actor;

namespace CustomerServiceCampaign.API.Jwt
{
    public class JwtActor : IApplicationActor
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public IEnumerable<int> AllowedUseCases { get; set; }
    }
}
