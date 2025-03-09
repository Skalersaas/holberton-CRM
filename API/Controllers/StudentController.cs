using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType<ApiResponse<Student>>(StatusCodes.Status200OK)]
    public class StudentController(IRepository<Student> _context) 
        : CrudController<Student, StudentDTO>(_context)
    {
        [ProducesResponseType<ApiResponse<IEnumerable<Student>>>(StatusCodes.Status200OK)]
        public override Task<ObjectResult> GetAll([FromQuery] SearchModel model)
        {
            return base.GetAll(model);
        }
    }
}
