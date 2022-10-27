using Doodle.Api.Auth.Controllers.Auth.Models;
using Doodle.Services.Auth.Users.Abstractions;
using Doodle.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Doodle.Api.Auth.Controllers.Auth
{
    [ApiController]
    [Route("auth/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IUserSessionService _userSessionService;
        private readonly IUserRegistrationService _userRegistrationService;

        public EmailController(ILogger<EmailController> logger, IUserRegistrationService userRegistrationService, IUserSessionService identityUserService)
        {
            _logger = logger;
            _userRegistrationService = userRegistrationService;
            _userSessionService = identityUserService;
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] AccountActivationInputModel inputModel)
        {
            try
            {
                inputModel.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(inputModel.Code));
                var result = await _userRegistrationService.ActivateAccount(AccountActivationInputModel.ToInput(inputModel));

                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem();
            }
        }

        [HttpPost("change-email")]
        public async Task<IActionResult> ChangeEmail([FromQuery] ChangeEmailInputModel inputModel)
        {
            try
            {
                var normalizedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(inputModel.Code));
                var result = await _userSessionService.ChangeEmail(ChangeEmailInputModel.ToInput(inputModel));

                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem();
            }
        }
    }
}