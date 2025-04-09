using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;
using Persistance.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType<ApiResponse<Student>>(StatusCodes.Status200OK)]
    public class StudentController : CrudController<Student, StudentDTO>
    {
        private readonly IRepository<Admission> _admissionRepository;

        public StudentController(IRepository<Student> context, IRepository<Admission> admissionRepository)
            : base(context) 
        {
            _admissionRepository = admissionRepository;
        }

        [ProducesResponseType<ApiResponse<IEnumerable<Student>>>(StatusCodes.Status200OK)]
        public override Task<ObjectResult> GetAll([FromQuery] SearchModel model)
        {
            return base.GetAll(model);
        }

        [HttpGet("no-admission")]
        [ProducesResponseType<ApiResponse<IEnumerable<Student>>>(StatusCodes.Status200OK)]
        public async Task<ObjectResult> NoAdmissionStudents()
        {
            var allStudents = await _context.GetAllAsync(new SearchModel());

            var admittedStudents = await _admissionRepository.GetAllAsync(new SearchModel());

            var noAdmissionStudents = allStudents.Where(s => !admittedStudents.Any(a => a.StudentGuid == s.Guid));

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
