using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("agent")]
    public class Agent : Entity
    {
        public string Title { get; set; }
        public long? Salary { get; set; }
        public string? Notes { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; }
        public virtual ICollection<CustomerDiscount> CustomerDiscounts { get; set; }
    }
}
