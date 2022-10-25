using Doodle.Domain.Entities;
using Doodle.Services.Common;
using Doodle.Services.Security;
using Doodle.Services.Users.Abstractions;
using Doodle.Services.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Web;

namespace Doodle.Services.Users
{
    internal class IdentityUserService : IUsersService
    {
        private readonly ILogger<IdentityUserService> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSenderService _emailSender;
        private readonly IUsersService _usersService;

        public IdentityUserService(ILogger<IdentityUserService> logger,
                                   UserManager<IdentityUser> userManager,
                                   SignInManager<IdentityUser> signInManager,
                                   ITokenService tokenService,
                                   IUserStore<IdentityUser> userStore,
                                   IUserEmailStore<IdentityUser> emailStore,
                                   IEmailSenderService emailSender,
                                   IUsersService usersService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userStore = userStore;
            _emailStore = emailStore;
            _emailSender = emailSender;
            _usersService = usersService;
        }

        public Task<Result<User>> DeleteUser(UserFilterDTO input)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByCredentials(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<User>> Register(UserRegisterInput input)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
                return Result<User>.Fail("Erro ao criar usuário");

            _logger.LogInformation("User created a new account with password.");

            await SendEmailAsync(user);

            return Result<User>.Successful(new User(), "usuário criado.");
        }

        public async Task<Result<Token>> SignIn(UserSignInInput input, string username)
        {
            var externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var result = await _signInManager.PasswordSignInAsync(username, input.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                var user = _signInManager.UserManager.Users.FirstOrDefault(p => p.NormalizedUserName == username.ToUpper());
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

        public Task<User> UnverifyUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateMfa(UserVerifyInput input)
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword)
        {
            throw new NotImplementedException();
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

        public async Task<Result<bool>> ResetPassword(string email, string code, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result<bool>.Successful(true, "Your password has been reset. You can now log in.");

            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);

            if (result.Succeeded)
                return Result<bool>.Successful(true, "Your password has been reset. You can now log in.");

            var errors = result.Errors.Select(p => p.Description);

            return Result<bool>.Fail(string.Join("; ", errors));
        }

        public Task<User> VerifyUser(UserVerifyInput input)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> SendEmailAsync(IdentityUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = new Uri($"https://localhost:7706/auth/confirm-email?userId={userId}&code={code}&area=Identity");

            await _emailSender.SendEmailAsync(new List<string> { user.Email }, "Confirm your email",
                $"Please confirm your account by <a href='{HttpUtility.UrlEncode(callbackUrl.ToString())}'>clicking here</a>.");

            return true;
        }

        public async Task<Result<bool>> ActivateAccount(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result<bool>.Fail($"Unable to load user with ID '{userId}'.");

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return Result<bool>.Successful(true, "Thank you for confirming your email.");

            return Result<bool>.Fail("Error confirming your email.");
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}