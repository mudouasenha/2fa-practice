using Doodle.Domain.Entities;
using Doodle.Services.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doodle.Services.Users.Abstractions
{
    public interface IUsersService
    {
        Task<User> InsertUser(UserInputDTO input);

        Task<User> DeleteUser(UserFilterDTO input);

        Task<User> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword);
    }
}