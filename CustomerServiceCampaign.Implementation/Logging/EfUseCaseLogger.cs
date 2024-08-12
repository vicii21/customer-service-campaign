using Newtonsoft.Json;
using CustomerServiceCampaign.Application.Logging;
using CustomerServiceCampaign.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Implementation.Logging
{
    public class EfUseCaseLogger : IUseCaseLogger
    {
        private readonly CustomerServiceCampaignContext _context;

        public EfUseCaseLogger(CustomerServiceCampaignContext context)
        {
            _context = context;
        }

        public void Add(UseCaseLogEntry entry)
        {
            _context.LogEntry.Add(new Domain.Entities.LogEntry
            {
                Actor = entry.Actor,
                ActorId = entry.ActorId,
                UseCaseData = JsonConvert.SerializeObject(entry.Data),
                UseCaseName = entry.UseCaseName,
                CreatedAt = DateTime.UtcNow
            });

            _context.SaveChanges();
        }
    }
}
