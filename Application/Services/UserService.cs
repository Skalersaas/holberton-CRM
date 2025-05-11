using Domain.Models.Entities;
using Application.Models;
using Persistance.Data.Interfaces;
using Utilities.Security;
using Utilities.DataManipulation;

namespace Application.Services
{
    public class UserService(IRepository<User> context):
        ModelService<User, UserCreate, UserUpdate, UserResponse>(context)
    {
        public override async Task<(bool, UserResponse)> CreateAsync(UserCreate entity)
        {
            var model = Mapper.FromDTO<User, UserCreate>(entity);
            model.Password = PasswordHashGenerator.GenerateHash(model.Password);
            var builtSlug = model.BuildSlug();
            model.Slug = builtSlug + "-" + context.GetCount(new SearchModel()
            {
                Filters = new Dictionary<string, string>()
                {
                    { nameof(User.Slug), builtSlug }
                }
            });

            var created = await context.CreateAsync(model);
            
            return created == null
                ? (false, new UserResponse())
                : (true, Mapper.FromDTO<UserResponse, User>(created));
        }
        public (int, string) Login(UserLogin userData)
        {
            var found = context.GetByField(nameof(User.Login), userData.Login);
            if (found == null) return (404, "");
            if (!PasswordHashGenerator.VerifyPassword(found.Password, userData.Password)) return (400, "");

            return (200, JwtTokenGenerator.GenerateJwtToken(userData.Login));
        }
    }
}
