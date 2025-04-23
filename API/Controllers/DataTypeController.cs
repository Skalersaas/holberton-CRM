using API.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Services;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [ModelValidation]
    public class DataTypeController : ControllerBase
    {
        [HttpGet("model/{name}")]
        public ObjectResult GetModel(string name)
        {
            var modelType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Namespace != null
                                && t.Namespace.StartsWith("Domain.Models.Entities")
                                && t.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            if (modelType == null)
                return ResponseGenerator.NotFound("Model not found");

            var modelInstance = Activator.CreateInstance(modelType);
            return ResponseGenerator.Ok(modelInstance);
        }
        [HttpGet("enum/{name}")]
        public ObjectResult GetEnum(string name)
        {
            var modelType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Namespace != null
                                && t.Namespace.StartsWith("Domain.Enums")
                                && t.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            if (modelType == null || !modelType.IsEnum)
                return ResponseGenerator.NotFound("Enum not found");


            return ResponseGenerator.Ok(GetEnumValues(modelType));
        }
        private static List<object> GetEnumValues(Type enumType) 
        {
            return Enum.GetValues(enumType)
                .Cast<Enum>()
                .Select(stat => new { Name = stat.ToString(), Value = Convert.ToInt32(stat) })
                .ToList<object>();
        }
    }
}
