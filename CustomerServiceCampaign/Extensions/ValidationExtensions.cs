using CustomerServiceCampaign.API.DTO;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServiceCampaign.API.Extensions
{
    public static class ValidationExtensions
    {
        public static UnprocessableEntityObjectResult UnprocessableEntity(this ValidationResult result)
        {
            var errors = result.Errors.Select(x => new ClientErrorDto
            {
                Error = x.ErrorMessage,
                Property = x.PropertyName
            });

            return new UnprocessableEntityObjectResult(errors);
        }

    }
}
