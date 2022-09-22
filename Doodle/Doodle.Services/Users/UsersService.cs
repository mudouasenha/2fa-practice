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

        public Task<bool> DeleteUser(UserFilterDTO input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertUser(UserInputDTO input)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}