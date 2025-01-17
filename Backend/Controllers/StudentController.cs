using holberton_CRM.Data;
using holberton_CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace holberton_CRM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController(IRepository<Student> _students) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ObjectResult> GetById(Guid Id)
        {
            var stud = await _students.GetByIdAsync(Id);
            if (stud == null)
                return NotFound(Id);

            return Ok(stud);
        }
        [HttpGet("all")]
        public async Task<ObjectResult> GetAll()
        {
            return Ok(await _students.GetAllAsync());
        }
        [HttpPost("new")]
        public async Task<ObjectResult> New([FromBody] Student stud)
        {
            return Ok(await _students.AddAsync(stud));
        }
        [HttpPut("update")]
        public async Task<ObjectResult> Update([FromBody] Student stud)
        {
            await _students.UpdateAsync(stud);
            return Ok("");
        }
    }   
}
