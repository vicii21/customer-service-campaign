using CustomerServiceCampaign.Application.Actor;
using CustomerServiceCampaign.Application.UseCases.Commands;
using CustomerServiceCampaign.Application.UseCases.DTO;
using CustomerServiceCampaign.DataAccess;
using CustomerServiceCampaign.Domain.Entities;
using CustomerServiceCampaign.Implementation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Implementation.UseCases.Commands
{
    public class EfCustomerDiscountCommand : EfUseCase, ICustomerDiscountCommand
    {
        private CustomerDiscountValidator _validator;
        private IApplicationActor _actor;

        public EfCustomerDiscountCommand(CustomerServiceCampaignContext context, IApplicationActor actor, CustomerDiscountValidator validator) : base(context)
        {
            _validator = validator;
            _actor = actor;
        }
        public int Id => 1;

        public string Name => "Customer Discount Command";

        public void Execute(CustomerDiscountDTO dto)
        {
            _validator.ValidateAndThrow(dto);
            CustomerDiscount customerDiscount = new CustomerDiscount
            {
                AgentId = dto.AgentId,
                CustomerId = dto.CustomerId,
                IsUsed = false,
                DiscountExpires = DateTime.Now.AddDays(7)
            };

            _context.CustomerDiscount.Add(customerDiscount);
            _context.SaveChanges();
        }
    }
}
