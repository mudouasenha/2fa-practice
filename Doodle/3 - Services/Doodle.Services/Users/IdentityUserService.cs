using Doodle.Domain.Entities;
using Doodle.Services.Common;
using Doodle.Services.Security;
using Doodle.Services.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Web;

namespace Doodle.Services.Users
{
    internal class IdentityUserService : IIdentityUserService
    {
        private readonly ILogger<IdentityUserService> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSenderService _emailSender;

        public IdentityUserService(ILogger<IdentityUserService> logger,
                                   UserManager<IdentityUser> userManager,
                                   SignInManager<IdentityUser> signInManager,
                                   ITokenService tokenService,
                                   IUserStore<IdentityUser> userStore,
                                   IUserEmailStore<IdentityUser> emailStore,
                                   IEmailSenderService emailSender)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userStore = userStore;
            _emailStore = emailStore;
            _emailSender = emailSender;
        }

        public async Task<Result<User>> Register(UserRegisterInput input)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, input.Username, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
                return Result<User>.Fail("Erro ao criar usuário");

            _logger.LogInformation("User created a new account with password.");

            await SendEmailAccountConfirmation(user);

            return Result<User>.Successful(new User(), "usuário criado.");
        }

        public async Task<Result<Token>> SignIn(UserSignInInput input)
        {
            var result = await _signInManager.PasswordSignInAsync(input.Username, input.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                var user = await _signInManager.UserManager.FindByNameAsync(input.Username);
                var token = _tokenService.GenerateToken(user);

                return Result<Token>.Successful(token, "User logged in.");
            }

            if (result.RequiresTwoFactor)
                return Result<Token>.Fail("Please login with 2fa");

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return Result<Token>.Fail("This account has been locked out, please try again later.");
            }

            return Result<Token>.Fail("Invalid login attempt.");
        }

        public async Task<Result<bool>> SignOut(UserSignOutInput input)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return Result<bool>.Successful(true, "User logged out.");
        }

        public async Task<Result<bool>> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return Result<bool>.Successful(true, "Please check your email to reset your password.");

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _emailSender.SendEmailAsync(
                new List<string> { email },
                "Reset Password",
                $"Please reset your password by <a href='{HttpUtility.UrlEncode($"https://localhost:7077/account/reset-password?area=Identity&code={code}")}'>clicking here</a>.");

            return Result<bool>.Successful(true, "Please check your email to reset your password.");
        }

        public async Task<Result<bool>> ResetPassword(PasswordResetInput input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);

            if (user == null)
                return Result<bool>.Successful(true, "Your password has been reset. You can now log in.");

            var result = await _userManager.ResetPasswordAsync(user, input.Token, input.Password);

            if (result.Succeeded)
                return Result<bool>.Successful(true, "Your password has been reset. You can now log in.");

            var errors = result.Errors.Select(p => p.Description);

            return Result<bool>.Fail(string.Join("; ", errors));
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

        public async Task<Result<bool>> ChangeEmail(ChangeEmailInput input)
        {
            if (input.UserId == null || input.Email == null || input.Code == null)
                return Result<bool>.Fail("Please fill all .");

            var user = await _userManager.FindByIdAsync(input.UserId);
            if (user == null)
                return Result<bool>.Fail($"Unable to load user with ID '{input.UserId}'.");

            var result = await _userManager.ChangeEmailAsync(user, input.Email, input.Code);
            if (!result.Succeeded)
                Result<bool>.Fail("Error changing email.");

            var setEmailResult = await _userManager.SetEmailAsync(user, input.Email);
            if (!setEmailResult.Succeeded)
                Result<bool>.Fail("Error changing user name.");

            await _signInManager.RefreshSignInAsync(user);
            return Result<bool>.Successful(true, "Thank you for confirming your email change.");
        }

        private async Task<bool> SendEmailAccountConfirmation(IdentityUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = new Uri($"https://localhost:7706/auth/confirm-email?userId={userId}&code={code}&area=Identity");

            await _emailSender.SendEmailAsync(new List<string> { user.Email }, "Confirm your email",
                $"Please confirm your account by <a href='{HttpUtility.UrlEncode(callbackUrl.ToString())}'>clicking here</a>.");

            return true;
        }

        private static IdentityUser CreateUser() => Activator.CreateInstance<IdentityUser>();
    }
}