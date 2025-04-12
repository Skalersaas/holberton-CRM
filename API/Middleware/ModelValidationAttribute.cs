using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Utilities.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Middleware
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .Select(e => new
                    {
                        field = e.Key,
                        error = e.Value?.Errors.First().ErrorMessage
                    });

                context.Result = ResponseGenerator.BadRequest("Invalid Data",errors);
            }
        }
    }
}
