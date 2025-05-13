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
        public override async Task<(bool, UserResponse?)> CreateAsync(UserCreate entity)
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
                ? (false, null)
                : (true, Mapper.FromDTO<UserResponse, User>(created));
        }
        public (int, string) Login(UserLogin userData)
        {
            var found = context.GetByField(nameof(User.Login), userData.Login);
            if (found == null) return (404, string.Empty);
            if (!PasswordHashGenerator.VerifyPassword(found.Password, userData.Password)) return (400, string.Empty);

            return (200, JwtTokenGenerator.GenerateJwtToken(userData.Login));
        }
        public (int, UserResponse?) GetInfoByIdentity(string? name)
        {
            if (string.IsNullOrEmpty(name)) return (401, null);

            var (searchResult, user) = GetByField(nameof(User.Login), name);

            return searchResult switch
            {
                false => (404, null),
                true => (200, user)
            };
        }
        public (int, UserResponse?) ChangePassword(UserChangePassword model)
        {
            var user = context.GetByField(nameof(User.Login), model.Login);
            if (user == null) return (404, null);

            var verificationResult = PasswordHashGenerator.VerifyPassword(user.Password, model.OldPassword);
            if (!verificationResult) return (400, null);

            user.Password = PasswordHashGenerator.GenerateHash(model.NewPassword);
            context.UpdateAsync(user);

            return (400, Mapper.FromDTO<UserResponse, User>(user));
        }
    }
}
