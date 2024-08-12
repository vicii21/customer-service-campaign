using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Application.UseCases.DTO
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SSN { get; set; }
        public int DOB{ get; set; }
        public int Age { get; set; }
    }
}
