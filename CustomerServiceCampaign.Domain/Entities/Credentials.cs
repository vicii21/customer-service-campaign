using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("credentials")]
    public class Credentials : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; }
    }
}
