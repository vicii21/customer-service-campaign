using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("state")]
    public class State : Entity
    {
        public string StateName { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
