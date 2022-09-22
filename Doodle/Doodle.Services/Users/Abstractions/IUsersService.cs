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
        Task<bool> InsertUser(UserInputDTO input);

        Task<bool> DeleteUser(UserFilterDTO input);

        Task<bool> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword);
    }
}
