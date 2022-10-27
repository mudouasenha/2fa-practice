using Doodle.Api.Controllers.Models;
using Doodle.Infrastructure.Security.MultiFactorAuthentication.Abstractions;
using Doodle.Infrastructure.Security.MultiFactorAuthentication.Models;
using Doodle.Services.Users.Abstractions;
using Doodle.Services.Users.Models;
using Microsoft.AspNetCore.Mvc;

namespace Doodle.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class VerifyController : ControllerBase
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
            var user = await _usersService.GetByCredentials(inputModel.Username, inputModel.Password);

            if (user == default)
                return new VerificationResult(new List<string> { "User not Found." });

            //var userUnverified = await _usersService.UnVerifyUser(inputModel.Username, inputModel.Password);

            if (user.Verified)
                return new VerificationResult(new List<string> { "Your totp is already verified." });

            var toInput = UserVerifyInputModel.ToInput(inputModel);
            var verificationResult = await _verification.VerifyResource(user, inputModel.Code);

            await _usersService.VerifyUser(toInput);

            return verificationResult;
        }

        [HttpGet("first")]
        public async Task<VerificationResult> GetFirstAuth(UserVerifyInputModel inputModel)
        {
            var user = await _usersService.GetByCredentials(inputModel.Username, inputModel.Password);

            if (user == default)
                return new VerificationResult(new List<string> { "User not Found." });

            if (user.Verified)
                return new VerificationResult(new List<string> { "Your totp is already verified." });

            var toInput = UserVerifyInputModel.ToInput(inputModel);
            var (verificationResult, userId) = await _verification.StartVerificationAsync(user);
            toInput.MfaExternalId = userId;

            await _usersService.UpdateMfa(toInput);

            return verificationResult;
        }
    }
}