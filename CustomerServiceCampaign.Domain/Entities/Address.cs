using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("address")]
    public class Address : Entity
    {
        public string Street { get; set; }
        public string Zip { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Person> HomePersons { get; set; }
        public virtual ICollection<Person> OfficePersons { get; set; }
    }
}
