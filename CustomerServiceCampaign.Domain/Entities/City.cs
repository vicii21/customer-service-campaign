using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("city")]
    public class City : Entity
    {
        public string CityName { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
