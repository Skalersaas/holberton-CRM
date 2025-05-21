using API.Middleware;
using Application.Services;
using Domain.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities.DataManipulation;
using Utilities.Responses;

namespace API.BaseControllers
{
    [ApiController]
    [ModelValidation]
    [Route("[controller]")]
#if !DEBUG
    [Authorize]
#endif
    public abstract class CrudController<TModel, TCreate, TUpdate, TResponse>(ModelService<TModel, TCreate, TUpdate, TResponse> service)
        : ControllerBase
        where TModel : class, IModel, new()
        where TCreate : class
        where TUpdate : class, IModel
        where TResponse : class, new()
    {
        protected ModelService<TModel, TCreate, TUpdate, TResponse> service = service;
        [HttpPost]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status409Conflict)]
        public virtual async Task<ObjectResult> Create([FromBody] TCreate entity)
        {
            var (suceed, result) = await service.CreateAsync(entity);
                
            return suceed
                ? ResponseGenerator.Ok(result)
                : ResponseGenerator.BadRequest("Entity with such GUID exists");
        }

        [HttpGet("{guid}")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> GetById(Guid guid)
        {
            var (succeed, result) = await service.GetByIdAsync(guid);

            return succeed
                ? ResponseGenerator.Ok(result)
                : ResponseGenerator.NotFound("Entity with such GUID was not found");
        }

        [HttpGet("slug/{slug}")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> GetBySlug(string slug)
        {
            (bool succeed, TResponse? result) = Guid.TryParse(slug, out var guid)
                ? await service.GetByIdAsync(guid)
                : service.GetByField("Slug", slug);

            return succeed
                ? ResponseGenerator.Ok(result)
                : ResponseGenerator.NotFound("Entity with such GUID or slug was not found");
        }

        [HttpPost("all")]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
        public virtual async Task<ObjectResult> GetAll([FromBody] SearchModel model)
        {
            var (data, fullCount) = await service.GetAllAsync(model);
            return ResponseGenerator.Ok(data, fullCount);
        }
        [HttpPut]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> Update([FromBody] TUpdate entity)
        {
            var (succeed, result) = await service.UpdateAsync(entity);
                
            return succeed
                ? ResponseGenerator.Ok(result)
                : ResponseGenerator.BadRequest("Entity with such GUID exists");
        }
        [HttpDelete]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public virtual async Task<ObjectResult> Delete(Guid id)
        {
            if (!await service.DeleteAsync(id))
                return ResponseGenerator.NotFound("Entity with such GUID was not found");
            return ResponseGenerator.Ok("No data");
        }
    }
}
