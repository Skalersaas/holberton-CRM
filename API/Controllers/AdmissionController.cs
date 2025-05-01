using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType<ApiResponse<IEnumerable<Admission>>>(StatusCodes.Status200OK)]
        public override Task<ObjectResult> GetAll([FromBody] SearchModel model)
        {
            return base.GetAll(model);
        }
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public override async Task<ObjectResult> New([FromBody] AdmissionDTO entity)
        {
            var student = await management.Students.GetByIdAsync(entity.StudentId);
            var user = await management.Users.GetByIdAsync(entity.UserId);
            if (student is null)
                return ResponseGenerator.NotFound("Student with such Id was not found");

            if (user is null)
                return ResponseGenerator.NotFound("User with such Id was not found");

            var adm = Mapper.FromDTO<Admission, AdmissionDTO>(entity);

            adm.Student = student;
            adm.User = user;
            
            return ResponseGenerator.Ok(await management.Admissions.CreateAsync(adm));
        }

        public override async Task<ObjectResult> Update([FromBody] Admission entity)
        {
            var prev = await _context.GetByIdAsync(entity.Id);
            if (prev == null)
                return ResponseGenerator.NotFound("Admission with such GUID was not found");

            _context.Detach(prev);

            await TrackAndSaveAdmissionChanges(prev, entity);

            await _context.UpdateAsync(entity);

            return ResponseGenerator.Ok(entity);
        }
        [HttpGet("history")]
        public async Task<ObjectResult> History([FromQuery] Guid id)
        {
            var changes = await management.AdmissionChanges.GetAllAsync(new SearchModel()
            {
                Filters = new Dictionary<string, string>()
                {
                    { nameof(AdmissionChange.AdmissionId), id.ToString() }
                }
            });
            return ResponseGenerator.Ok(changes);
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
                AdmissionId = prev.Id,
                Data = JsonSerializer.SerializeToDocument(changes),
                CreatedTime = DateTime.UtcNow
            };

            await management.AdmissionChanges.CreateAsync(aChange);
        }
    }
}
