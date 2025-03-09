using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.Services;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    public class EnumController : ControllerBase
    {
        [HttpGet("admissionStatuses")]
        public ObjectResult AdmissionStatuses()
        {
            return ResponseGenerator.Ok(GetEnumValues<AdmissionStatus>());
        }
        [HttpGet("userRoles")]
        public ObjectResult UserRoles()
        {
            return ResponseGenerator.Ok(GetEnumValues<UserRole>());
        }
        public static List<object> GetEnumValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(stat => new { Name = stat.ToString(), Value = Convert.ToInt32(stat) })
                .ToList<object>();
        }
    }
}
