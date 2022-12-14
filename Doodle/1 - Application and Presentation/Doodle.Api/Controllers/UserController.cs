using Doodle.Api.Controllers.Models;
using Doodle.Domain.Constants;
using Doodle.Domain.Entities;
using Doodle.Services.Common;
using Doodle.Services.Users.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doodle.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUsersService _usersService;

        public UserController(ILogger<UserController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpGet(), Authorize(Roles = RoleConstants.Admin)]
        public async Task<IEnumerable<User>> Get()
        {
            return new List<User> { new User() };
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

        [HttpPost("signout"), Authorize]
        public async Task<Result<User>> SignOut(UserSignOutInputModel inputModel)
        {
            var result = await _usersService.SignOut(UserSignOutInputModel.ToInput(inputModel));
            return result;
        }

        [HttpPut("change-password"), Authorize]
        public async Task<Result<User>> ChangePassword(UserSignOutInputModel inputModel)
        {
            throw new NotImplementedException("Not yet Implemented");
        }

        [HttpDelete("delete-account"), Authorize(Roles = RoleConstants.Admin)]
        public async Task<Result<User>> DeleteAccount()
        {
            throw new NotImplementedException("Not yet Implemented");
            //var result = await _usersService.DeleteUser(UserRegisterInputModel.ToInput(inputModel));
            //return result;
        }
    }
}