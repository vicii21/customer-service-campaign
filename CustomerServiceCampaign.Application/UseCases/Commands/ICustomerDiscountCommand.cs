using CustomerServiceCampaign.Application.UseCases.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Application.UseCases.Commands
{
    public interface ICustomerDiscountCommand : ICommand<CustomerDiscountDTO>
    {
    }
}
