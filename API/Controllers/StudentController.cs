using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data.Interfaces;
using Utilities.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType<ApiResponse<Student>>(StatusCodes.Status200OK)]
    public class StudentController(IRepository<Student> context, IRepository<Admission> admissionRepository) : CrudController<Student, StudentDTO>(context)
    {
        private readonly IRepository<Admission> _admissions = admissionRepository;

        [ProducesResponseType<ApiResponse<IEnumerable<Student>>>(StatusCodes.Status200OK)]
        public override Task<ObjectResult> GetAll([FromBody] SearchModel model)
        {
            return base.GetAll(model);
        }

        [HttpGet("no-admission")]
        [ProducesResponseType<ApiResponse<IEnumerable<Student>>>(StatusCodes.Status200OK)]
        public async Task<ObjectResult> NoAdmissionStudents()
        {
            var (allStudents, _) = await _context.GetAllAsync(new SearchModel());

            var (admittedStudents, _) = await _admissions.GetAllAsync(new SearchModel());

            var noAdmissionStudents = allStudents.Where(s => !admittedStudents.Any(a => a.StudentGuid == s.Id));

            return ResponseGenerator.Ok(noAdmissionStudents);
        }

        [Consumes("multipart/form-data")]
        [HttpPost("upload-file")]
        [ProducesResponseType<ApiResponse<IEnumerable<Student>>>(StatusCodes.Status200OK)]
        public async Task<ObjectResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            if (!file.FileName.EndsWith(".xlsx"))
                return BadRequest("Only .xlsx format is allowed.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var students = ExcelParser.ParseFromExcel<Student>(stream);

            return Ok(ResponseGenerator.Ok(students));
        }
    }
}
