using CustomerServiceCampaign.Application.Actor;

namespace CustomerServiceCampaign.API.Jwt
{
    public class UnauthorizedActor : IApplicationActor
    {
        public int Id => 0;

        public string FullName => "Guest";

        public string Email => "";

        public IEnumerable<int> AllowedUseCases => new List<int> { }; // TODO - add
    }
}
