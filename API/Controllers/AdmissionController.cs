using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using Persistance.Data.Repositories;
using System.Text.Json;
using Utilities.Services;
using static Utilities.Services.ResponseGenerator;
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
                AdmissionGuid = prev.Guid,
                Data = JsonSerializer.SerializeToDocument(changes)
            };

            await management.AdmissionChanges.CreateAsync(aChange);
        }

    }
}
