using Microsoft.AspNetCore.Mvc.Filters;
using Utilities.Services;

namespace API.Middleware
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = ResponseGenerator.BadRequest("Invalid data");
        }   
    }
}
