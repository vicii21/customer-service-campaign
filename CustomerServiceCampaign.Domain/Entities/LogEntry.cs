using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("log_entry")]
    public class LogEntry : Entity
    {
        public int ActorId { get; set; }
        public string Actor { get; set; }
        public string UseCaseName { get; set; }
        public string UseCaseData { get; set; }
    }
}
