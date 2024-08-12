using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Application.Actor
{
    public interface IApplicationActor
    {
        int Id { get; }
        string FullName { get; }
        string Email { get; }
        IEnumerable<int> AllowedUseCases { get; }
    }
}
