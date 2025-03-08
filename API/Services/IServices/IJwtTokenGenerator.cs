using Domain.Models.Entities;

namespace API.Services.IServices
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User applicationUser, IEnumerable<string> roles);
    }
}
