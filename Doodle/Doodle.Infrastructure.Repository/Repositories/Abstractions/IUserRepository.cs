using Doodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doodle.Infrastructure.Repository.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmail(string email);

        Task<User> GetByUsername(string username);

        Task<bool> ExistsByUsername(string username);

        Task<bool> ExistsByEmail(string email);

        Task<User> GetByEmailAndPassword(string email, string password);

        Task<bool> ExistsByEmailAndPassword(string email, string password);

        Task<User> GetByUsernameAndPassword(string username, string password);

        Task<bool> ExistsByUsernameAndPassword(string username, string password);
    }
}