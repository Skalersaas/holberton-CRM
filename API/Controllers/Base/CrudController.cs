using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using API.Attributes;
using Domain.Interfaces;
using Persistence.Repositories;
using Application.Services;
using Application.JsonTemplates;

namespace API.Controllers.Base
{
    [Authorize]
    [ApiController]
    [ModelValidation]
    public abstract class CrudController<T, D>(IRepository<T> _context) :
            ControllerBase where T : class, D, IModel
                           where D : class
    {
        [HttpGet("{slug}")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> GetOne(string slug)
        {
            var entity = Guid.TryParse(slug, out var guid)
                ? await _context.GetByIdAsync(guid)
                : await _context.GetBySlugAsync(slug);


            return entity is not null
                ? ResponseGenerator.Ok(entity)
                : ResponseGenerator.NotFound("Entity with such GUID was not found");
        }
        [HttpGet("all")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
        public virtual async Task<ObjectResult> GetAll([FromQuery] SearchModel model)
        {
            if (typeof(T).GetProperty(model.SortedField) == null)
                return ResponseGenerator.BadRequest();

            return ResponseGenerator.Ok(await _context.GetAllAsync(model));
        }
        [HttpPost("new")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status409Conflict)]
        public virtual async Task<ObjectResult> New([FromBody] D entity)
        {
            var created = await _context.CreateAsync(Mapper.FromDTO<T, D>(entity));
            if (created == default)
                return ResponseGenerator.Conflict("Entity with such slug exists");

            return ResponseGenerator.Ok(created);
        }
        [HttpPut("update")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> Update([FromBody] T entity)
        {
            if (await _context.GetByIdAsync(entity.Id) == null)
                return ResponseGenerator.NotFound("Entity with such Id was not found");
            await _context.UpdateAsync(entity);
            return ResponseGenerator.Ok(entity);
        }
        [HttpDelete("delete")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> Delete(Guid id)
        {
            if (!await _context.DeleteAsync(id))
                return ResponseGenerator.NotFound("Entity with such Id was not found");
            return ResponseGenerator.Ok("Deleted");
        }
    }
}
