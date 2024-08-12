using CustomerServiceCampaign.API.DTO;
using CustomerServiceCampaign.API.ErrorLogging;
using CustomerServiceCampaign.API.Jwt;
using CustomerServiceCampaign.Application.Logging;
using CustomerServiceCampaign.Application.UseCases.Commands;
using CustomerServiceCampaign.Domain.Entities;
using CustomerServiceCampaign.Implementation.UseCases.Commands;
using CustomerServiceCampaign.Implementation.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceCampaign.Application.Extensions
{
    public static class ContainerExtensions
    {
        public static void AddAllUseCases(this IServiceCollection services)
        {
            services.AddTransient<ICustomerDiscountCommand, EfCustomerDiscountCommand>();
            services.AddTransient<CustomerDiscountValidator>();
            //services.AddTransient<UseCaseHandler>();
            //services.AddTransient<IUseCaseLogger, SPUseCaseLogger>();
        }

        public static string ExtractTokenClaim(this HttpRequest request, string claimType)
        {
            if (request == null || !request.Headers.ContainsKey("Authorization"))
            {
                return null;
            }

            string authHeader = request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                string token = authHeader.Split("Bearer ")[1];
                var handler = new JwtSecurityTokenHandler();
                try
                {
                    var tokenObj = handler.ReadJwtToken(token);
                    var claim = tokenObj.Claims.FirstOrDefault(c => c.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase))?.Value;
                    return claim;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error reading token: {e.Message}");
                }
            }
            return null;
        }

        public static void AddLogger(this IServiceCollection services)
        {
            services.AddTransient<IErrorLogger>(static e =>
            {
                var contextAccessor = e.GetService<IHttpContextAccessor>();
                var logger = contextAccessor.HttpContext.Request.Headers["Logger"].FirstOrDefault();

                dynamic res;

                if (contextAccessor == null || contextAccessor.HttpContext == null || logger == "Console")
                {
                    res = new ConsoleErrorLogger();
                }
                else
                {
                    res = new BugSnagErrorLogger(e.GetService<Bugsnag.IClient>());
                }
                return res;
            });
        }
    }
}
