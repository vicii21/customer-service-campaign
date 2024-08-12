using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("color")]
    public class Color : Entity
    {
        public string ColorName { get; set; }
        public virtual ICollection<PersonColor> PersonColor { get; set; }
    }
}
