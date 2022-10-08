using Doodle.Api.Controllers.Models;
using Doodle.Domain.Entities;
using Doodle.Services.Common;
using Doodle.Services.Users.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doodle.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUsersService _usersService;

        public AuthController(ILogger<AuthController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        /// <summary>
        /// Registers an user.
        /// </summary>
        /// <param name="registerInput"></param>
        /// <returns>A newly created User</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /user/register
        ///     {
        ///        "Name": "matheus",
        ///        "Username": "username",
        ///        "Address": "5th Street",
        ///        "Email": "test@test.com",
        ///        "Password": "password",
        ///        "PhoneNumber": "4444444444"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the newly created User</response>
        /// <response code="400">If the registration fails</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<Result<User>> Register(UserRegisterInputModel inputModel)
        {
            var result = await _usersService.Register(UserRegisterInputModel.ToInput(inputModel));
            return result;
        }

        [HttpPost("signin")]
        public async Task<Result<User>> SignIn(UserSignInInputModel inputModel)
        {
            var result = await _usersService.SignIn(UserSignInInputModel.ToInput(inputModel));
            return result;
        }
    }
}