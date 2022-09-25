using Doodle.Api.Services.Common;
using Doodle.Api.Services.Users.Abstractions;
using Doodle.Api.Services.Users.Models;
using Doodle.Domain.Entities;

namespace Doodle.Api.Services.Users
{
    public class UsersService : IUsersService
    {
        //private readonly IUserRepository _userRepository;

        /*public UsersService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            var user = new User()
            {
                Name = input.Name,
                Address = input.Address,
                Email = input.Email,
                Password = input.Password,
                PhoneNumber = input.PhoneNumber,
                Username = input.Username,
                CreatedAt = DateTime.Now
            };

            var userInserted = await _userRepository.Insert(user);

            return new Result<User>(userInserted, "User registered Successfully", true);
        }

        public async Task<Result<User>> SignIn(UserSignInInput input)
        {
            var userFromRepository = await _userRepository.GetByUsername(input.Username);
            return new Result<User>(new User(), "Signed in Successfully", true);
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
        }*/

        public Task<Result<User>> DeleteUser(UserFilterDTO input)
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> Register(UserRegisterInput input)
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> SignIn(UserSignInInput input)
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> SignOut(UserSignOutInput input)
        {
            throw new NotImplementedException();
        }

        public Task<Result<User>> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}