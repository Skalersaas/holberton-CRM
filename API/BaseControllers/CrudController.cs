using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using static Utilities.Services.ResponseGenerator;
namespace API.BaseControllers
{
    [ApiController]
    public abstract class CrudController<T>(IRepository<T> _context) : ControllerBase where T : class, IModel
    {
        protected string Name => typeof(T).Name;

        [HttpGet("{guid:guid}")]
        public virtual async Task<ObjectResult> GetById(Guid guid)
        {
            var entity = await _context.GetByIdAsync(guid);
            if (entity == null)
                return NotFound(GenerateNotFoundResponse(Name, "Guid"));

            return Ok(GenerateSuccessResponse(entity));
        }
        [HttpGet("all")]
        public virtual async Task<ObjectResult> GetAll()
        {
            return Ok(GenerateSuccessResponse(await _context.GetAllAsync()));
        }
        [HttpPost("new")]
        public virtual async Task<ObjectResult> New([FromBody] T entity)
        {
            return Ok(GenerateSuccessResponse(await _context.CreateAsync(entity)));
        }
        [HttpPut("update")]
        public virtual async Task<ObjectResult> Update([FromBody] T entity)
        {
            if (await _context.GetByIdAsync(entity.Guid) == null)
                return NotFound(GenerateNotFoundResponse(Name, "Guid"));

            await _context.UpdateAsync(entity);
            return Ok(GenerateSuccessResponse());
        }
        [HttpDelete("delete")]
        public virtual async Task<ObjectResult> Delete(Guid id)
        {
            if (!await _context.DeleteAsync(id))
                return NotFound(GenerateNotFoundResponse(Name, "Guid"));
            return Ok(GenerateSuccessResponse());
        }
    }
}
