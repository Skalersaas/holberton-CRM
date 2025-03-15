using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [Authorize]
        [HttpGet("ping")]
        public ObjectResult Ping()
        {
            return Ok("Server is working!");
        }
    }
}
