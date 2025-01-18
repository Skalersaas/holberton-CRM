using holberton_CRM.Data;
using holberton_CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace holberton_CRM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController(IRepository<Student> _context) : CrudController<Student>(_context)
    {
    }   
}
