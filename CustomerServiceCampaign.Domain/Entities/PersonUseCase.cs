using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Domain.Entities
{
    [Table("person_use_case")]
    public class PersonUseCase
    {
        public int PersonId { get; set; }
        public int UseCaseId { get; set; }

        public virtual Person Person { get; set; }
    }
}
