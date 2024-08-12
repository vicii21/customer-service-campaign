using CustomerServiceCampaign.API.DTO;
using CustomerServiceCampaign.DataAccess;
using CustomerServiceCampaign.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServiceCampaign.API.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly CustomerServiceCampaignContext _context;

        public OrderController(CustomerServiceCampaignContext context)
        {
            _context = context;
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderRequestDto req) 
        {
            var customer = _context.Customer.FirstOrDefault(c => c.PersonId == req.PersonId);

            if (customer == null)
            {
                return NotFound($"Person with ID {req.PersonId} does not exist");
            }

            var service = _context.Service.FirstOrDefault(s => s.ID == req.ServiceId);

            if (service == null)
            {
                return NotFound($"Service with ID {req.ServiceId} does not exist");
            }

            var customerDiscount = _context.CustomerDiscount.FirstOrDefault(cd => cd.CustomerId == customer.ID && cd.IsActive == true);

            if (customerDiscount == null)
            {
                return NotFound($"There are no discounts found for customer with ID {customer.ID}");
            }

            var order = new Order
            {
                
                CustomerDiscountId = customerDiscount.ID,
                ServiceId = req.ServiceId,
            };

            _context.Order.Add(order);
            _context.SaveChanges();

            customerDiscount.IsActive = false;
            customerDiscount.IsUsed = true;
            _context.SaveChanges();

            Console.WriteLine($"Customer with ID {customer.ID} used their discount with ID {customerDiscount.ID} for service {service.ServiceName}");

            return Ok("Order successfully created");
        }
    }
}
