using Domain.Models.Interfaces;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using Utilities.Services;
using static Utilities.Services.ResponseGenerator;

namespace API.BaseControllers
{
    [ApiController]
    public abstract class CrudController<T, D>(IRepository<T> _context) : 
        ControllerBase where T : class, D, IModel
                       where D : class
    {
        protected string Name => typeof(T).Name;

        [HttpGet("{slug}")]
        [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> GetById(string slug)
        {
            var entity = Guid.TryParse(slug, out var guid)
                ? await _context.GetByIdAsync(guid)
                : await _context.GetBySlugAsync(slug);


            return entity is not null
                ? ResponseOK(entity)
                : GenerateNotFound("Entity with such GUID was not found");
        }
        [HttpGet("all")]
        public virtual async Task<ObjectResult> GetAll([FromQuery] SearchModel model)
        {
            return ResponseOK(await _context.GetAllAsync(model));
        }
        [HttpPost("new")]
        [ProducesResponseType<ErrorResponse>(StatusCodes.Status409Conflict)]
        public virtual async Task<ObjectResult> New([FromBody] D entity)
        {
            var created = await _context.CreateAsync(Mapper.FromDTO<T, D>(entity));
            if (created == default)
                return GenerateConflict("User with such slug exists");

            return ResponseOK(created);
        }
        [HttpPut("update")]
        [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> Update([FromBody] T entity)
        {
            if (await _context.GetByIdAsync(entity.Guid) == null)
                return GenerateNotFound("Entity with such GUID was not found");
            await _context.UpdateAsync(entity);
            return ResponseOK(entity);
        }
        [HttpDelete("delete")]
        [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> Delete(Guid id)
        {
            if (!await _context.DeleteAsync(id))
                return GenerateNotFound("Entity with such GUID was not found");
            return ResponseOK("No data");
        }
    }
}
