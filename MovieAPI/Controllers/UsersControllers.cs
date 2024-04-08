using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Security;
using MovieModels.Interfaces;
using MovieModels.Models;
using MovieModels.Models.Arguments;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public UsersController(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [SwaggerOperation(Summary = "Login with an existing user",
                          Description = "Try with the default user with email: \\\"challenge@gmail.com\\\", password: \\\"challenge\\\""
        )]
        [SwaggerResponse(200, "The User was successfully authenticated")]
        [SwaggerResponse(400, "The Email or password provided is incorrect")]
        public async Task<IActionResult> Authenticate([FromBody] UserRequest model)
        {
            try
            {
                var response = await _tokenService.AuthenticateUser(model);

                if (response == null)
                    return BadRequest(new { message = "Email or password is incorrect" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Needs loggin the error
                return Problem($"message=Unknown error while trying to authenticate an User, error={ex.Message}");
            }
        }

        [HttpPost("createUser")]
        [SwaggerOperation(Summary = "Create a New User",
                          Description = "The Passwrod needs to be at least 4 characters"
        )]
        [SwaggerResponse(200, "The User was created successfully")]
        [SwaggerResponse(400, "Some of the data is incorrect")]
        public async Task<IActionResult> CreateUser([FromBody] UserArguments newUser)
        {
            try
            {
                var id = await _userService.CreateUser(newUser);
                if (id == null) return BadRequest(new { message = "The user data is incorrect, please check all fields and that the password has more than 4 characters" });
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                // Needs loggin the error
                return Problem($"message=Unknown error while trying to create a new user, error={ex.Message}");
            }
        }
    }
}
