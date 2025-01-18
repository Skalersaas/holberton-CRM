using holberton_CRM.Data;
using holberton_CRM.Data.Repositories;
using holberton_CRM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace holberton_CRM.Controllers
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
                return NotFound(entity.Guid);

            context.Detach(prev);

            await TrackAndSaveAdmissionChanges(prev, entity);

            await context.UpdateAsync(entity);

            return Ok("s");
        }
        public override async Task<ObjectResult> New([FromBody] Admission entity)
        {
            if (await management.Students.GetByIdAsync(entity.StudentGuid) == null)
                return BadRequest("no student with such guid");
            if (await management.Users.GetByIdAsync(entity.UserGuid) == null)
                return BadRequest("no user with such guid");

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
