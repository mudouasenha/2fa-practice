using Doodle.Domain.Entities;
using Doodle.Infrastructure.Repository.Repositories.Abstractions;
using Doodle.Services.Users.Abstractions;
using Doodle.Services.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doodle.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;

        public UsersService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> DeleteUser(UserFilterDTO input)
        {
            var user = await _userRepository.GetByUsernameAndPassword(input.UserName, input.Password);

            if (user == null)
                throw new Exception("User Not Found");

            var userDeleted = await _userRepository.Delete(user.Id);

            return userDeleted;
        }

        public async Task<User> InsertUser(UserInputDTO input)
        {
            var userFromRepository = await _userRepository.ExistsByUsernameAndPassword(input.Username, input.Password);

            if (userFromRepository)
                throw new Exception("User Already Exists");

            var user = new User()
            {
                Name = input.Name,
                Address = input.Address,
                Email = input.Email,
                Password = input.Password,
                PhoneNumber = input.PhoneNumber,
                Username = input.Username
            };

            var userInserted = await _userRepository.Insert(user);

            return userInserted;
        }

        public async Task<User> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword)
        {
            var userFromRepository = await _userRepository.GetByUsernameAndPassword(input.UserName, input.Password);

            if (userFromRepository == null)
                throw new Exception("User Not Found");

            var userToUpdate = userFromRepository;
            userToUpdate.Password = newPassword;

            var userUpdated = await _userRepository.Update(userFromRepository);

            return userUpdated;
        }
    }
}