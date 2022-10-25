using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Doodle.Api.Auth.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<EmailController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EmailController(UserManager<IdentityUser> userManager, ILogger<EmailController> logger, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] AccountActivationInputModel input)
        {
            try
            {
                var normalizedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
                var result = await _userManager.ActivateAccount(input.UserId, normalizedCode);

                if (!result.Success)
                    return BadRequest(result.Message);

                return Ok(result.Message);
                //var user = await _userManager.FindByIdAsync(userId);
                //if (user == null)
                //    return NotFound($"Unable to load user with ID '{userId}'.");

                ////code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                //var result = await _userManager.ConfirmEmailAsync(user, code);
                //return result.Succeeded ? Ok("Thank you for confirming your email.") : BadRequest("Error confirming your email.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem();
            }
        }

        [HttpPost("change-email")]
        public async Task<IActionResult> ChangeEmail([FromQuery] ChangeEmailInputModel input)
        {
            try
            {
                if (input.UserId == null || input.Email == null || input.Code == null)
                    return BadRequest("Please fill all .");

                var user = await _userManager.FindByIdAsync(input.UserId);
                if (user == null)
                    return NotFound($"Unable to load user with ID '{input.UserId}'.");

                //code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ChangeEmailAsync(user, input.Email, input.Code);
                if (!result.Succeeded)
                    BadRequest("Error changing email.");

                // In our UI email and user name are one and the same, so when we update the email
                // we need to update the user name.
                var setUserNameResult = await _userManager.SetUserNameAsync(user, input.Email);
                if (!setUserNameResult.Succeeded)
                    BadRequest("Error changing user name.");

                await _signInManager.RefreshSignInAsync(user);
                return Ok("Thank you for confirming your email change.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem();
            }
        }
    }

    public class AccountActivationInputModel
    {
        public string UserId { get; set; }

        public string Code { get; set; }
    }

    public class ChangeEmailInputModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }
    }
}