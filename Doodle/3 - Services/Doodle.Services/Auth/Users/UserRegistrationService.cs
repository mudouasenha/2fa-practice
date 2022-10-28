using Doodle.Domain.Constants;
using Doodle.Domain.Entities;
using Doodle.Services.Auth.Users.Abstractions;
using Doodle.Services.Common;
using Doodle.Services.EmailSender.Abstractions;
using Doodle.Services.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Web;

namespace Doodle.Services.Auth.Users
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly ILogger<UserRegistrationService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderService _emailSender;

        public UserRegistrationService(ILogger<UserRegistrationService> logger,
                                   UserManager<ApplicationUser> userManager,
                                   IEmailSenderService emailSender)
        {
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<Result<ApplicationUser>> Register(UserRegisterInput input)
        {
            var user = CreateUser();

            await _userManager.SetUserNameAsync(user, input.Username);
            await _userManager.SetEmailAsync(user, input.Email);

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
                return Result<ApplicationUser>.Fail("Erro ao criar usuário");

            await _userManager.AddToRoleAsync(user, RoleConstants.Reader);

            _logger.LogInformation("User created a new account with password.");

            await SendEmailAccountConfirmation(user);

            return Result<ApplicationUser>.Successful(new ApplicationUser(), "usuário criado.");
        }

        public async Task<Result<bool>> ActivateAccount(AccountActivationInput input)
        {
            var user = await _userManager.FindByIdAsync(input.UserId);
            if (user == null)
                return Result<bool>.Fail($"Unable to load user with ID '{input.UserId}'.");

            var result = await _userManager.ConfirmEmailAsync(user, input.Code);

            if (result.Succeeded)
                return Result<bool>.Successful(true, "Thank you for confirming your email.");

            return Result<bool>.Fail("Error confirming your email.");
        }

        private async Task<bool> SendEmailAccountConfirmation(ApplicationUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = new Uri($"https://localhost:7706/auth/confirm-email?userId={userId}&code={code}&area=Identity");

            await _emailSender.SendEmailAsync(new List<string> { user.Email }, "Confirm your email",
                $"Please confirm your account by <a href='{HttpUtility.UrlEncode(callbackUrl.ToString())}'>clicking here</a>.");

            return true;
        }

        private static ApplicationUser CreateUser() => Activator.CreateInstance<ApplicationUser>();
    }
}