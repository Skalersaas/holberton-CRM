using API.BaseControllers;
using Application.Models;
using Application.Services;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Utilities.DataManipulation;
using Utilities.Responses;

namespace API.Controllers
{
    public class UserController(IServiceProvider serviceProvider) : CrudController<User, UserCreate, UserUpdate, UserResponse>
        (ActivatorUtilities.CreateInstance<UserService>(serviceProvider))
    {
        [HttpPost("register")]
        [ProducesResponseType<ApiResponse<UserResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Create([FromBody] UserCreate entity) => await base.Create(entity);
        [ProducesResponseType<ApiResponse<UserResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> GetById(Guid guid) => await base.GetById(guid);
        [ProducesResponseType<ApiResponse<UserResponse[]>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> GetAll([FromBody] SearchModel model) => await base.GetAll(model);
        [ProducesResponseType<ApiResponse<UserResponse>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Update([FromBody] UserUpdate entity) => await base.Update(entity);
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status200OK)]
        public override async Task<ObjectResult> Delete(Guid id) => await base.Delete(id);

        [HttpPost("login")]
        public ObjectResult Login([FromBody] UserLogin entity)
        {
            var (loginResult, token) = (service as UserService)!.Login(entity);
            return loginResult switch
            {
                200 => ResponseGenerator.Ok(token),
                400 => ResponseGenerator.BadRequest("Invalid login/password"),
                404 => ResponseGenerator.NotFound("User not found"),
                _ => ResponseGenerator.InternalServerError("An unexpected error occurred"),
            };
        }
    }
}
