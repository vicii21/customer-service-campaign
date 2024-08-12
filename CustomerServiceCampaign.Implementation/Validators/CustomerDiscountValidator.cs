using CustomerServiceCampaign.Application.UseCases.DTO;
using CustomerServiceCampaign.DataAccess;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Implementation.Validators
{
    public class CustomerDiscountValidator : AbstractValidator<CustomerDiscountDTO>
    {
        public CustomerDiscountValidator(CustomerServiceCampaignContext _context)
        {
            RuleFor(e => e.AgentId)
                .NotEmpty()
                .WithMessage("Agent ID is required")
                .Must((dto, agentId) => !HasExceededDailyLimit(_context, agentId));

            RuleFor(e => e.CustomerId)
                .NotEmpty()
                .WithMessage("Customer ID is required")
                .Must((dto, customerId) => !HasActiveDiscount(_context, customerId))
                .WithMessage("Customer has already received a discount in the last 7 days.");
        }

        private bool HasExceededDailyLimit(CustomerServiceCampaignContext context, int agentId)
        {
            var today = DateTime.Today;
            var numberOfDiscounts = context.CustomerDiscount.Count(cd => cd.AgentId == agentId && cd.CreatedAt.Date == today);

            return numberOfDiscounts < 5;
        }

        private bool HasActiveDiscount(CustomerServiceCampaignContext context, int customerId)
        {
            var weekAgo = DateTime.Today.AddDays(-7);
            var recentDiscounts = context.CustomerDiscount
                .Where(cd => cd.CustomerId == customerId && cd.CreatedAt.Date >= weekAgo)
                .ToList();

            return !recentDiscounts.Any();
        }
    }
}
