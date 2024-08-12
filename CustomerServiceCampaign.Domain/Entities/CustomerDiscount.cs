using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("customer_discount")]
    public class CustomerDiscount : Entity
    {
        public int DiscountValue { get; set; }
        public bool IsUsed { get; set; }
        public DateTime DiscountExpires { get; set; }
        public int AgentId { get; set; }
        public int CustomerId { get; set; }

        public virtual Agent Agent { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
