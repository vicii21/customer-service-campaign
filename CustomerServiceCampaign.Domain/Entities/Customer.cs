using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("customer")]
    public class Customer : Entity
    {
        public int CustomerDiscountId { get; set; }
        public int PersonId { get; set; }

        public virtual CustomerDiscount CustomerDiscount { get; set; }
        public virtual Person Person { get; set; }
    }
}
