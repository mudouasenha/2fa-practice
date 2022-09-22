using Doodle.Domain.Entities;
using Doodle.Infrastructure.Repository.Data.Contexts;
using Doodle.Infrastructure.Repository.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doodle.Infrastructure.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DoodleDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByEmailAndPassword(string email, string password) => await dbSet.AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email.Equals(email)
                                      && p.Password.Equals(password));
    }
}