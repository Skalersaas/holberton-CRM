using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using Persistance.Data.Repositories;
using System.Text.Json;
using Utilities.Services;
namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType<ApiResponse<Admission>>(StatusCodes.Status200OK)]
    public class AdmissionController(AdmissionManagement management) : CrudController<Admission, AdmissionDTO>(management.Admissions)
    {
        private readonly IRepository<Admission> context = management.Admissions;

        [ProducesResponseType<ApiResponse<IEnumerable<Admission>>>(StatusCodes.Status200OK)]
        public override Task<ObjectResult> GetAll([FromQuery] SearchModel model)
        {
            return base.GetAll(model);
        }
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public override async Task<ObjectResult> New([FromBody] AdmissionDTO entity)
        {
            if (await management.Students.GetByIdAsync(entity.StudentGuid) == null)
                return ResponseGenerator.NotFound("Student with such GUID was not found");

            if (await management.Users.GetByIdAsync(entity.UserGuid) == null)
                return ResponseGenerator.NotFound("User with such GUID was not found");

            return await base.New(entity);
        }
        public override async Task<ObjectResult> Update([FromBody] Admission entity)
        {
            var prev = await context.GetByIdAsync(entity.Guid);
            if (prev == null)
                return ResponseGenerator.NotFound("Admission with such GUID was not found");

            context.Detach(prev);

            await TrackAndSaveAdmissionChanges(prev, entity);

            await context.UpdateAsync(entity);

            return ResponseGenerator.Ok(entity);
        }
        private async Task TrackAndSaveAdmissionChanges(Admission prev, Admission next)
        {
            var changes = new List<ChangeTemplate>();

            var properties = typeof(Admission).GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == nameof(Admission.User) ||
                    property.Name == nameof(Admission.Student) ||
                    property.Name == nameof(Admission.Notes))
                {
                    continue;
                }

                var prevValue = property.GetValue(prev);
                var nextValue = property.GetValue(next);

                if (!Equals(prevValue, nextValue))
                {
                    changes.Add(new ChangeTemplate
                    {
                        Field = property.Name,
                        Prev = prevValue?.ToString(),
                        Next = nextValue?.ToString()
                    });
                }
            }
            var aChange = new AdmissionChange()
            {
                Guid = Guid.NewGuid(),
                AdmissionGuid = prev.Guid,
                ChangedAt = DateTime.UtcNow,
                Data = JsonSerializer.SerializeToDocument(changes),
                Admission = next
            };

            await management.AdmissionChanges.CreateAsync(aChange);
        }

        [Consumes("multipart/form-data")]
        [HttpPost("upload-file")]
        [ProducesResponseType<ApiResponse<IEnumerable<Admission>>>(StatusCodes.Status200OK)]
        public async Task<ObjectResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            if (!file.FileName.EndsWith(".xlsx"))
                return BadRequest("Only .xlsx format is allowed.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var admissions = ExcelParser.ParseFromExcel<Admission>(stream);

            return Ok(ResponseGenerator.Ok(admissions));
        }
        
        [HttpGet("admission-history/{slug}")]
        [ProducesResponseType(typeof(ApiResponse<List<AdmissionChangeDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAdmissionHistoryBySlug(string slug)
        {
            var admission = await management.Admissions.GetBySlugAsync(slug);
            if (admission == null)
                return NotFound(ResponseGenerator.Error("Admission with the specified slug was not found."));

            var changes = await management.AdmissionChanges
                .GetAllAsync(new SearchModel
                {
                    
                });

            var result = changes.OrderByDescending(c => c.ChangedAt)
                .Select(change => new AdmissionChangeDto
                {
                    ChangedAt = change.ChangedAt,
                    Changes = JsonSerializer.Deserialize<List<ChangeTemplateDto>>(change.Data)
                })
                .ToList();

            return Ok(ResponseGenerator.Ok(result));
        }

    }
}