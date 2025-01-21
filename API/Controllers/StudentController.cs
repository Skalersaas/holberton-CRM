using API.BaseControllers;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController(IRepository<Student> _context) : CrudController<Student>(_context)
    {
    }   
}
