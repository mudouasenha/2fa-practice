using Doodle.Domain.Entities;
using Doodle.Infrastructure.Repository.Repositories.Abstractions;
using Doodle.Infrastructure.Security.MultiFactorAuthentication;
using Doodle.Services.Common;
using Doodle.Services.Security.Abstractions;
using Doodle.Services.Users.Abstractions;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerification _verification;
        private readonly ICryptoService _cryptoService;

        public UsersService(IUserRepository userRepository, ICryptoService cryptoService, IVerification verification)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _verification = verification;
        }

        public async Task<Result<User>> DeleteUser(UserFilterDTO input)
        {
            var user = await _userRepository.GetByUsername(input.UserName);

            if (user == null)
                throw new Exception("User Not Found");

            var userDeleted = await _userRepository.Delete(user.Id);

            return new Result<User>(userDeleted, "User deleted Successfully", true);
        }

        public async Task<Result<User>> Register(UserRegisterInput input)
        {
            var userFromRepository = await _userRepository.ExistsByUsername(input.Username);

            if (userFromRepository)
                throw new Exception("User Already Exists");

            var encryptionData = _cryptoService.GenerateUserEncryptionData(input.Username, input.Password);

            var user = new User()
            {
                Name = input.Name,
                Address = input.Address,
                Email = input.Email,
                Password = encryptionData.EncryptedPassword,
                PhoneNumber = input.PhoneNumber,
                Salt = encryptionData.Salt,
                Username = encryptionData.EncryptedUsername,
                CreatedAt = DateTime.Now
            };

            var userInserted = await _userRepository.Insert(user);

            return new Result<User>(userInserted, "User registered Successfully", true);
        }

        public async Task<Result<User>> SignIn(UserSignInInput input)
        {
            var usersFromRepository = await _userRepository.GetAll();

            var userLoggedIn = _cryptoService.VerifyLogin(usersFromRepository, input.Username, input.Password);

            if (userLoggedIn == default)
                return new Result<User>(default, "Could not log in. Please verify your credentials.", false);

            if (!userLoggedIn.Verified)
                return new Result<User>(default, "Your user is not authenticated via TOTP. Please authenticate in /api/verify", false);

            var verificationResult = await _verification.CheckVerificationAsync(userLoggedIn, input.TotpCode);

            if (!verificationResult.IsValid)
                return new Result<User>(default, string.Join(", ", verificationResult.Errors), false);

            return new Result<User>(userLoggedIn, "Signed in Successfully.", true);
        }

        public async Task<Result<User>> SignOut(UserSignOutInput input)
        {
            var userFromRepository = await _userRepository.GetByUsername(input.Username);
            return new Result<User>(new User(), "Signed out Successfully", true);
        }

        public async Task<Result<User>> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword)
        {
            var userFromRepository = await _userRepository.GetByUsername(input.UserName);

            if (userFromRepository == null)
                throw new Exception("User Not Found");

            var userToUpdate = userFromRepository;
            userToUpdate.Password = newPassword;

            var userUpdated = await _userRepository.Update(userFromRepository);

            return new Result<User>(userUpdated, "Password Updated Successfully", true);
        }

        public async Task<User> VerifyUser(UserVerifyInput input)
        {
            var userResult = await SignIn(new UserSignInInput() { Username = input.Username, Password = input.Password });

            var user = userResult.Data;
            user.Verified = true;

            await _userRepository.Update(user);

            return user;
        }
    }
}