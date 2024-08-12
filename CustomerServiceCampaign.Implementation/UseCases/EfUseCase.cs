using CustomerServiceCampaign.Application.Logging;
using CustomerServiceCampaign.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Implementation.UseCases
{
    public abstract class EfUseCase
    {
        protected CustomerServiceCampaignContext _context { get; set; }

        protected EfUseCase(CustomerServiceCampaignContext context)
        {
            _context = context;
        }
    }
}
