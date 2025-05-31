using API.BaseControllers;
using Application.Models;
using Application.Services;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Utilities.DataManipulation;
using Utilities.Responses;

namespace API.Controllers
{
    public class AdmissionController(IServiceProvider serviceProvider): CrudController<Admission, AdmissionCreate, AdmissionUpdate, AdmissionResponse>
        (ActivatorUtilities.CreateInstance<AdmissionService>(serviceProvider))
    {
        [ProducesResponseType<ApiResponse<AdmissionResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Create([FromBody] AdmissionCreate entity) => await base.Create(entity);

        [ProducesResponseType<ApiResponse<AdmissionResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> GetById(Guid guid) => await base.GetById(guid);

        [ProducesResponseType<ApiResponse<AdmissionResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> GetBySlug(string slug) => await base.GetBySlug(slug);

        [ProducesResponseType<ApiResponse<AdmissionResponse[]>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> GetAll([FromBody] SearchModel model) => await base.GetAll(model);

        [ProducesResponseType<ApiResponse<AdmissionResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Update([FromBody] AdmissionUpdate entity) => await base.Update(entity);

        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Delete(Guid id) => await base.Delete(id);

        [HttpGet("history")]
        [ProducesResponseType<ApiResponse<AdmissionChange[]>>(StatusCodes.Status200OK)]
        public async Task<ObjectResult> GetHistory(Guid id)
        {
            var (found, changes) = await ((AdmissionService)service).GetHistoryAsync(id);
            if (!found)
                return ResponseGenerator.NotFound("Entity with such GUID was not found");
            return ResponseGenerator.Ok(changes);
        }
        [HttpPost("addnote")]
        [ProducesResponseType<ApiResponse<AdmissionChange[]>>(StatusCodes.Status200OK)]
        public async Task<ObjectResult> AddNote(Guid id, string note)
        {
            var result = await ((AdmissionService)service).AddNote(id, note);

            return result switch
            {
                200 => ResponseGenerator.Ok("Note added"),
                404 => ResponseGenerator.NotFound("Entity with such GUID was not found"),
                _ => ResponseGenerator.BadRequest("Unknown error")
            };
        }

        [HttpPut("editnote")]
        [ProducesResponseType<ApiResponse<AdmissionResponse>>(StatusCodes.Status200OK)]
        public async Task<ObjectResult> EditNote(Guid id, string content)
        {
            var result = await ((AdmissionService)service).EditNote(id, content);

            return result switch
            {
                200 => ResponseGenerator.Ok("Note updated"),
                404 => ResponseGenerator.NotFound("Note with such ID was not found"),
                _ => ResponseGenerator.BadRequest("Unknown error")
            };
        }
    }
}
