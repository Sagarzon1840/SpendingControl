using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SpendingControl.Api.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Simple passthrough for now
            if (context.Exception is ArgumentException)
            {
                context.Result = new BadRequestObjectResult(new { error = context.Exception.Message });
            }
        }
    }
}
