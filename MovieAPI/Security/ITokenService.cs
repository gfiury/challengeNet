using MovieModels.Models;

namespace MovieAPI.Security
{
    public interface ITokenService
    {
        Task<UserResponse?> AuthenticateUser(UserRequest model);
    }
}
