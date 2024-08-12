using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("person_color")]
    public class PersonColor : Entity
    {
        public int PersonId { get; set; }
        public int ColorId { get; set; }

        public Person Person { get; set; }
        public Color Color { get; set; }
    }
}
