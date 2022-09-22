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
        Task<User> GetByEmailAndPassword(string email, string password);
    }
}