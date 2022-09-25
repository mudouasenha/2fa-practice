using Doodle.Api.Controllers.Models;
using Doodle.Api.Services.Common;
using Doodle.Api.Services.Users.Abstractions;
using Doodle.Domain.Entities;
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

        [HttpGet()]
        public async Task<IEnumerable<User>> Get()
        {
            return new List<User> { new User() };
        }

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

        [HttpPost("signout")]
        public async Task<Result<User>> SignOut(UserSignOutInputModel inputModel)
        {
            var result = await _usersService.SignOut(UserSignOutInputModel.ToInput(inputModel));
            return result;
        }

        [HttpPut("change-password")]
        public async Task<Result<User>> ChangePassword(UserSignOutInputModel inputModel)
        {
            throw new NotImplementedException("Not yet Implemented");
        }

        [HttpDelete("delete-account")]
        public async Task<Result<User>> DeleteAccount()
        {
            throw new NotImplementedException("Not yet Implemented");
            //var result = await _usersService.DeleteUser(UserRegisterInputModel.ToInput(inputModel));
            //return result;
        }
    }
}