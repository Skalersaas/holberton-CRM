using Microsoft.AspNetCore.Mvc;
using Utilities.Responses;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataTypeController : ControllerBase
    {

        [HttpGet("model/{name}")]
        public IActionResult GetModel(string name)
        {
            var modelType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.Namespace != null &&
                    t.Namespace.StartsWith("Domain.Models.Entities") &&
                    t.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            if (modelType == null)
                return ResponseGenerator.NotFound("Model not found");

            var modelInstance = Activator.CreateInstance(modelType);
            return ResponseGenerator.Ok(modelInstance);
        }

        [HttpGet("enum/{name}")]
        public IActionResult GetEnum(string name)
        {
            var enumType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.Namespace != null &&
                    t.Namespace.StartsWith("Domain.Enums") &&
                    t.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            if (enumType == null || !enumType.IsEnum)
                return ResponseGenerator.NotFound("Enum not found");

            var values = Enum.GetValues(enumType)
                .Cast<Enum>()
                .Select(e => new { Name = e.ToString(), Value = Convert.ToInt32(e) })
                .ToList();

            return ResponseGenerator.Ok(values);
        }

    }
}
