using Doodle.Api.Auth.Controllers.Auth.Models;
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
        private readonly IIdentityUserService _identityUserService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IIdentityUserService identityUserService, ILogger<EmailController> logger)
        {
            _identityUserService = identityUserService;
            _logger = logger;
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] AccountActivationInputModel inputModel)
        {
            try
            {
                inputModel.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(inputModel.Code));
                var result = await _identityUserService.ActivateAccount(AccountActivationInputModel.ToInput(inputModel));

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
                var result = await _identityUserService.ChangeEmail(ChangeEmailInputModel.ToInput(inputModel));

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