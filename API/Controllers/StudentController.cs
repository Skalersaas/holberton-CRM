using API.BaseControllers;
using Application.Models;
using Application.Services;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Utilities.DataManipulation;
using Utilities.Responses;

namespace API.Controllers
{
    public class StudentController(IServiceProvider serviceProvider):CrudController<Student, StudentCreate, StudentUpdate, StudentResponse>
        (ActivatorUtilities.CreateInstance<StudentService>(serviceProvider))
    {
        [ProducesResponseType<ApiResponse<StudentResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Create([FromBody] StudentCreate entity) => await base.Create(entity);
        
        [ProducesResponseType<ApiResponse<StudentResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> GetById(Guid guid) => await base.GetById(guid);
        
        [ProducesResponseType<ApiResponse<StudentResponse[]>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> GetAll([FromBody] SearchModel model) => await base.GetAll(model);
        
        [ProducesResponseType<ApiResponse<StudentResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Update([FromBody] StudentUpdate entity) => await base.Update(entity);
        
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Delete(Guid id) => await base.Delete(id);
    }
}