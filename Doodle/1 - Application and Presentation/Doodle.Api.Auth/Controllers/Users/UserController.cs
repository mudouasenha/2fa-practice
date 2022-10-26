using Doodle.Api.Auth.Controllers.Models;
using Doodle.Api.Controllers.Models;
using Doodle.Domain.Entities;
using Doodle.Services.Common;
using Doodle.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doodle.Api.Auth.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IIdentityUserService _identityUserService;

        public AuthController(ILogger<AuthController> logger, IIdentityUserService identityUserService)
        {
            _logger = logger;
            _identityUserService = identityUserService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterInputModel inputModel)
        {
            var result = await _identityUserService.Register(UserRegisterInputModel.ToInput(inputModel));
            return Ok(result);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInInputModel inputModel)
        {
            var result = await _identityUserService.SignIn(UserSignInInputModel.ToInput(inputModel));
            return Ok(result);
        }

        [HttpPost("signout"), Authorize]
        public async Task<IActionResult> SignOut([FromBody] UserSignOutInputModel inputModel)
        {
            try
            {
                var result = await _identityUserService.SignOut(UserSignOutInputModel.ToInput(inputModel));

                if (!result.Success)
                    return Problem(result.Message);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem();
            }
        }

        [HttpPut("change-password"), Authorize]
        public async Task<Result<User>> ChangePassword([FromBody] UserSignOutInputModel inputModel)
        {
            throw new NotImplementedException("Not yet Implemented");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] PasswordResetInputModel input)
        {
            try
            {
                var result = await _identityUserService.ForgotPassword(input.Email);

                if (!result.Success)
                    return Problem(result.Message);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetInputModel inputModel)
        {
            try
            {
                var result = await _identityUserService.ResetPassword(PasswordResetInputModel.ToInput(inputModel));

                if (!result.Success)
                    return Problem(result.Message);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem();
            }
        }
    }
}