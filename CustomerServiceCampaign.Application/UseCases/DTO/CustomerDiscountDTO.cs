using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Application.UseCases.DTO
{
    public class CustomerDiscountDTO
    {
        public int AgentId { get; set; }

        public int CustomerId { get; set; }
    }
}
