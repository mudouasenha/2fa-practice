using Doodle.Api.Controllers.Models;
using Doodle.Infrastructure.Security.MultiFactorAuthentication;
using Doodle.Services.Users.Abstractions;
using Doodle.Services.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doodle.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Verify")]
    public class VerifyController : Controller
    {
        private readonly ILogger<VerifyController> _logger;
        private readonly IVerification _verification;
        private readonly IUsersService _usersService;

        public VerifyController(ILogger<VerifyController> logger, IVerification verification, IUsersService usersService)
        {
            _logger = logger;
            _verification = verification;
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<VerificationResult> Post(UserVerifyInputModel inputModel)
        {
            var signInInput = new UserSignInInput() { Username = inputModel.Username, Password = inputModel.Password };

            var userResult = await _usersService.SignIn(signInInput);
            var user = userResult.Data;

            if (user == default)
                return new VerificationResult(new List<string> { "User not Found." });

            var result = await _verification.CheckVerificationAsync(user, inputModel.Code);
            if (result.IsValid)
            {
                await _usersService.VerifyUser(UserVerifyInputModel.ToInput(inputModel));

                _logger.Log(LogLevel.Information, $"User verified: {user.Username}");
            }

            return result;
        }

        [HttpPost("first")]
        public async Task<VerificationResult> FirstAuth(UserVerifyInputModel inputModel)
        {
            var signInInput = new UserSignInInput() { Username = inputModel.Username, Password = inputModel.Password };

            var result = await _usersService.SignIn(signInInput);

            if (result.Data == default)
                return new VerificationResult(new List<string> { "User not Found." });

            if (!result.Data.Verified)
            {
                return await _verification.StartVerificationAsync(result.Data, "totp");
            }

            await _usersService.VerifyUser(UserVerifyInputModel.ToInput(inputModel));

            return new VerificationResult(new List<string> { "Your totp is already verified." });
        }
    }
}