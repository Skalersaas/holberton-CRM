using holberton_CRM.Data;
using holberton_CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace holberton_CRM.Controllers
{
    [ApiController]
    public abstract class CrudController<T>(IRepository<T> _context) : ControllerBase where T : class, IModel
    {
        [HttpGet("{guid:guid}")]
        public virtual async Task<ObjectResult> GetById(Guid guid)
        {
            var entity = await _context.GetByIdAsync(guid);
            if (entity == null)
                return NotFound(guid);

            return Ok(entity);
        }
        [HttpGet("all")]
        public virtual async Task<ObjectResult> GetAll()
        {
            return Ok(await _context.GetAllAsync());
        }
        [HttpPost("new")]
        public virtual async Task<ObjectResult> New([FromBody] T entity)
        {
            return Ok(await _context.CreateAsync(entity));
        }
        [HttpPut("update")]
        public virtual async Task<ObjectResult> Update([FromBody] T entity)
        {
            if (await _context.GetByIdAsync(entity.Guid) == null)
                return NotFound(entity.Guid);

            await _context.UpdateAsync(entity);
            return Ok("");
        }
        [HttpDelete("delete")]
        public virtual async Task<ObjectResult> Delete(Guid id)
        {
            if (!await _context.DeleteAsync(id))
                return NotFound(id);
            return Ok("ok");
        }
    }
}
