using Doodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Doodle.Infrastructure.Repository.Auth.Data.Contexts;
using Doodle.Infrastructure.Repository.Auth.Repositories.Abstractions;
using Doodle.Infrastructure.Repository.Auth.Repositories;

namespace Doodle.Infrastructure.Repository.Auth.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DoodleAuthDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByEmailAndPassword(string email, string password) => await dbSet.AsQueryable()
            .AsNoTracking().FirstOrDefaultAsync(p => p.Email.Equals(email) && p.Password.Equals(password));

        public async Task<bool> ExistsByEmailAndPassword(string email, string password) => await dbSet.AsQueryable()
            .AsNoTracking().AnyAsync(p => p.Email.Equals(email) && p.Password.Equals(password));

        public async Task<User> GetByUsernameAndPassword(string username, string password) => await dbSet.AsQueryable()
            .AsNoTracking().FirstOrDefaultAsync(p => p.Username.Equals(username) && p.Password.Equals(password));

        public async Task<bool> ExistsByUsernameAndPassword(string username, string password) => await dbSet.AsQueryable()
            .AsNoTracking().AnyAsync(p => p.Username.Equals(username) && p.Password.Equals(password));

        public async Task<User> GetByEmail(string username) => await dbSet.AsQueryable().AsNoTracking()
            .FirstOrDefaultAsync(p => p.Username.Equals(username));

        public async Task<User> GetByUsername(string email) => await dbSet.AsQueryable().AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email.Equals(email));

        public async Task<bool> ExistsByUsername(string username) => await dbSet.AsQueryable().AsNoTracking()
            .AnyAsync(p => p.Username.Equals(username));

        public async Task<bool> ExistsByEmail(string email) => await dbSet.AsQueryable().AsNoTracking()
            .AnyAsync(p => p.Email.Equals(email));

        public async Task<List<User>> GetAll() => await dbSet.AsQueryable().AsNoTracking().ToListAsync();

        public async Task ClearChangeTrackers()
        {
            var changedEntriesCopy = dbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}
