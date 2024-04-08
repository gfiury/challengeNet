using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieEntityFramework.Interfaces;
using MovieModels.Interfaces;
using MovieModels.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieAPI.Security
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;

        public TokenService(IOptions<AppSettings> appSettings, IUserService userService)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
        }

        public async Task<UserResponse?> AuthenticateUser(UserRequest model)
        {
            var user = await _userService.GetByEmailAndPassword(model.Email, model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = await GenerateJwtToken(user);

            return new UserResponse(user, token);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            //Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {

                var key = Encoding.ASCII.GetBytes(_appSettings.SecretToken);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
