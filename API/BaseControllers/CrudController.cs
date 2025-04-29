using API.Middleware;
using Domain.Models.Interfaces;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data.Interfaces;
using Utilities.Services;

namespace API.BaseControllers
{
    [ApiController]
    [ModelValidation]
    public abstract class CrudController<T, D>(IRepository<T> context) : 
        ControllerBase where T : class, D, IModel
                       where D : class
    {
        protected readonly IRepository<T> _context = context;

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
        [HttpPost("all")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
        public virtual async Task<ObjectResult> GetAll([FromBody] SearchModel model)
        {
            var (data, fullCount) = await _context.GetAllAsync(model);
            return ResponseGenerator.Ok(data, fullCount);
        }
        [HttpPost]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status409Conflict)]
        public virtual async Task<ObjectResult> New([FromBody] D entity)
        {
            var created = await _context.CreateAsync(Mapper.FromDTO<T, D>(entity));
            if (created == default)
                return ResponseGenerator.Conflict("Entity with such slug exists");

            return ResponseGenerator.Ok(created);
        }
        [HttpPut]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> Update([FromBody] T entity)
        {
            if (await _context.GetByIdAsync(entity.Id) == null)
                return ResponseGenerator.NotFound("Entity with such GUID was not found");
            return ResponseGenerator.Ok(await _context.UpdateAsync(entity));
        }
        [HttpDelete]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> Delete(Guid id)
        {
            if (!await _context.DeleteAsync(id))
                return ResponseGenerator.NotFound("Entity with such GUID was not found");
            return ResponseGenerator.Ok("No data");
        }
    }
}
