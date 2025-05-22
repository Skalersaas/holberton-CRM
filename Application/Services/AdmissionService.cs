using Application.Models;
using Domain.Models;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Persistance.Data.Interfaces;
using System.Text.Json;
using Utilities.DataManipulation;

namespace Application.Services
{
    public class AdmissionService(IRepository<Admission> admissions) :
        ModelService<Admission, AdmissionCreate, AdmissionUpdate, AdmissionResponse>
        (admissions)
    {
        public override async Task<(bool, AdmissionResponse?)> CreateAsync(AdmissionCreate entity)
        {
            var model = Mapper.FromDTO<Admission, AdmissionCreate>(entity);

            var builtSlug = model.BuildSlug();
            model.Slug = builtSlug + "-" + context.GetCount(new SearchModel()
            {
                Filters = new Dictionary<string, string>()
                {
                    { nameof(Admission.Slug), builtSlug }
                }
            });
            var created = await context.CreateAsync(model);

            return created == null
                ? (false, null)
                : (true, Mapper.FromDTO<AdmissionResponse, Admission>(created));
        }
        public override async Task<(bool, AdmissionResponse?)> GetByIdAsync(Guid guid)
        {
            var admission = await context.GetByIdAsync(guid, x => x.Notes, x => x.Changes);

            return admission == null
                ? (false, null)
                : (true, Mapper.FromDTO<AdmissionResponse, Admission>(admission));
        }
        public override async Task<(bool, AdmissionResponse?)> UpdateAsync(AdmissionUpdate entity)
        {
            var found = await context.GetByIdAsync(entity.Id, x => x.Changes, x => x.Student!);
            if (found == null)
                return (false, null);

            if (entity.Status == Domain.Enums.AdmissionStatus.Submitted)
            {
                found.Student!.IsEnrolled = true;
                found.Student!.EnrolledAt = DateTime.UtcNow;
            }

            context.Detach(found);
            var adm = Mapper.FromDTO<Admission, AdmissionUpdate>(entity);
            TrackAndSaveAdmissionChanges(found, adm);

            var admission = await context.UpdateAsync(adm);

            return admission == null
                ? (false, null)
                : (true, Mapper.FromDTO<AdmissionResponse, Admission>(admission));
        }
        public async Task<(bool Found, AdmissionChange[] Changes)> GetHistoryAsync(Guid id)
        {
            var found = await context.GetByIdAsync(id, x => x.Changes);

            return found is null
                ? (false, [])
                : (true, [.. found.Changes]);
        }
        public async Task<int> AddNote(Guid id, string note)
        {
            var found = await context.GetByIdAsync(id, x => x.Notes);
            if (found == null) return 404;

            found.Notes.Add(new(note, id));
            return 200;
        }

        private static void TrackAndSaveAdmissionChanges(Admission prev, Admission next)
        {
            var changes = new List<object>();

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
                        FieldName = property.Name,
                        PreValue = prevValue?.ToString(),
                        PostValue = nextValue?.ToString()
                    });
                }
            }
            var aChange = new AdmissionChange()
            {
                AdmissionId = prev.Id,
                Data = JsonSerializer.SerializeToDocument(changes),
                CreatedTime = DateTime.UtcNow
            };

            next.Changes.Add(aChange);
        }
    }
}
