using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using Persistance.Data.Repositories;
using System.Text.Json;
using static Utilities.Services.ResponseGenerator;
namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdmissionController(AdmissionManagement management) : CrudController<Admission>(management.Admissions)
    {
        private readonly IRepository<Admission> context = management.Admissions;

        public override async Task<ObjectResult> Update([FromBody] Admission entity)
        {
            var prev = await context.GetByIdAsync(entity.Guid);
            if (prev == null)
                return NotFound(GenerateNotFoundResponse(Name, "Guid"));

            context.Detach(prev);

            await TrackAndSaveAdmissionChanges(prev, entity);

            await context.UpdateAsync(entity);

            return Ok(GenerateSuccessResponse());
        }
        public override async Task<ObjectResult> New([FromBody] Admission entity)
        {
            if (await management.Students.GetByIdAsync(entity.StudentGuid) == null)
                return BadRequest(GenerateNotFoundResponse("Student", "Guid"));

            if (await management.Users.GetByIdAsync(entity.UserGuid) == null)
                return BadRequest(GenerateNotFoundResponse("User", "Guid"));

            return await base.New(entity);
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
