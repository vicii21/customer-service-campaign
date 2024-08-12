using CustomerServiceCampaign.Application.Exceptions;
using CustomerServiceCampaign.Application.Logging;
using FluentValidation;

namespace CustomerServiceCampaign.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IErrorLogger _errorLogger;
        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate, IErrorLogger errorLogger)
        {
            _requestDelegate = requestDelegate;
            _errorLogger = errorLogger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (ValidationException e)
            {
                context.Response.StatusCode = 400;

                var err = e.Errors.Select(x => new
                {
                    x.ErrorMessage,
                    x.PropertyName
                });

                await context.Response.WriteAsJsonAsync(err);
            }
            catch (UnauthorizedUseCaseExecutionException e)
            {
                context.Response.StatusCode = 401;
            }
            catch (EntityNotFoundException e)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = e.Message
                });
            }
            catch (Exception e)
            {
                Guid errorId = Guid.NewGuid();
                AppError error = new AppError
                {
                    Exception = e,
                    ErrorId = errorId,
                    Username = "test"
                };
                _errorLogger.Log(error);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var responseBody = new
                {
                    message = $"A problem has been encountered with code: {errorId}."
                };

                await context.Response.WriteAsJsonAsync(responseBody);
            }
        }
    }
}
