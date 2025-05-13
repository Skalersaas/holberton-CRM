using Domain.Models.Entities;
using Application.Models;
using Persistance.Data.Interfaces;
using Utilities.DataManipulation;

namespace Application.Services
{
    public class StudentService(IRepository<Student> context) :
        ModelService<Student, StudentCreate, StudentUpdate, StudentResponse>(context)
    {
        public override async Task<(bool, StudentResponse?)> CreateAsync(StudentCreate entity)
        {
            var model = Mapper.FromDTO<Student, StudentCreate>(entity);
            var builtSlug = model.BuildSlug();
            model.Slug = builtSlug + "-" + context.GetCount(new SearchModel()
            {
                Filters = new Dictionary<string, string>()
                {
                    { nameof(Student.Slug), builtSlug }
                }
            });

            var created = await context.CreateAsync(model);

            return created == null
                ? (false, null)
                : (true, Mapper.FromDTO<StudentResponse, Student>(created));
        }
    }
}
