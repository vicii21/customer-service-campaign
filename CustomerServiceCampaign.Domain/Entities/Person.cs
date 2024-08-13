using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("person")]
    public class Person : Entity
    {
        public string Name { get; set; }
        public string SSN { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public int? SpouseId { get; set; }
        public int HomeAddressId { get; set; }
        public int? OfficeAddressId { get; set; }

        public virtual Person Spouse { get; set; }
        public virtual Address HomeAddress { get; set; }
        public virtual Address OfficeAddress { get; set; }
        public virtual Credentials Credentials { get; set; }

        public ICollection<Person> SpousePersons { get; set; } = new HashSet<Person>();
        public virtual Agent Agent { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<PersonColor> ColorPerson { get; set; }
        public virtual ICollection<PersonUseCase> PersonUseCases { get; set; }
    }
}
