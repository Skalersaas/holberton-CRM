using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using Utilities.Services;

namespace API.Middleware
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine(context.ModelState.IsValid);
            if (!context.ModelState.IsValid)
                context.Result = ResponseGenerator.BadRequest("Invalid data");
        }   
    }
}
