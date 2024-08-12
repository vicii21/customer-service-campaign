using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("order")]
    public class Order : Entity
    {
        public int CustomerDiscountId { get; set; }
        public int ServiceId { get; set; }

        public virtual CustomerDiscount CustomerDiscount { get; set; }
        public virtual Service Service { get; set; }
    }
}
