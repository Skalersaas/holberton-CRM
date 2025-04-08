using API.Middleware;
using Domain.Models.Interfaces;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using System.Reflection;
using Utilities.Services;

namespace API.BaseControllers
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
        public virtual async Task<ObjectResult> GetById(string slug)
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
        public virtual async Task<ObjectResult> GetAll([FromBody] SearchModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.SortedField) &&
                typeof(T).GetProperty(model.SortedField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) == null)
            {
                return ResponseGenerator.BadRequest();
            }

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
            if (await _context.GetByIdAsync(entity.Guid) == null)
                return ResponseGenerator.NotFound("Entity with such GUID was not found");
            await _context.UpdateAsync(entity);
            return ResponseGenerator.Ok(entity);
        }
        [HttpDelete("delete")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> Delete(Guid id)
        {
            if (!await _context.DeleteAsync(id))
                return ResponseGenerator.NotFound("Entity with such GUID was not found");
            return ResponseGenerator.Ok("No data");
        }
    }
}
