using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Application.Actor
{
    public interface IActorFetch
    {
        IApplicationActor Fetch();
    }
}
