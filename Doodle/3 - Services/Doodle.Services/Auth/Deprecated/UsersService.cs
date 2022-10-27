using Doodle.Domain.Entities;
using Doodle.Infrastructure.Security.MultiFactorAuthentication.Abstractions;
using Doodle.Services.Auth.Deprecated.Abstractions;
using Doodle.Services.Common;
using Doodle.Services.Security.Abstractions;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Auth.Deprecated
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

        public async Task<User> GetByCredentials(string username, string password)
        {
            var usersFromRepository = await _userRepository.GetAll();

            var userWithValidCredentials = _cryptoService.VerifyLogin(usersFromRepository, username, password);

            return userWithValidCredentials;
        }

        public async Task<Result<User>> SignIn(UserSignInInput input)
        {
            var user = await GetByCredentials(input.Username, input.Password);

            if (user == default)
                return new Result<User>(default, "Could not log in. Please verify your credentials.", false);

            if (user.MfaIdentity == default)
                return new Result<User>(default, "Your user is not authenticated via TOTP. Please authenticate in /api/verify", false);

            await UnverifyUser(input.Username, input.Password);

            var verificationResult = await _verification.CheckVerificationAsync(user, input.TotpCode);

            if (!verificationResult.IsValid)
                return new Result<User>(default, string.Join(", ", verificationResult.Errors), false);

            await VerifyUser(new UserVerifyInput() { Code = input.TotpCode, Username = input.Username, Password = input.Password, MfaExternalId = user.MfaIdentity });

            return new Result<User>(user, "Signed in Successfully.", true);
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
            await _userRepository.ClearChangeTrackers();

            var user = await GetByCredentials(input.Username, input.Password);

            user.Verified = true;

            await _userRepository.Update(user);

            return user;
        }

        public async Task<User> UnverifyUser(string username, string password)
        {
            await _userRepository.ClearChangeTrackers();

            var user = await GetByCredentials(username, password);

            user.Verified = false;

            await _userRepository.Update(user);

            return user;
        }

        public async Task<User> UpdateMfa(UserVerifyInput input)
        {
            await _userRepository.ClearChangeTrackers();

            var user = await GetByCredentials(input.Username, input.Password);

            user.MfaIdentity = input.MfaExternalId;

            await _userRepository.Update(user);

            return user;
        }
    }
}