using Doodle.Auth.Infrastructure.Repository.Data.Contexts;
using Doodle.Auth.Infrastructure.Repository.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Doodle.Auth.Infrastructure.Repository.Repositories
{
    public class AuthRepository<TEntity> : IAuthRepository<TEntity> where TEntity : IdentityUser<int>
    {
        protected readonly DoodleAuthDbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        public AuthRepository(DoodleAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<TEntity>();
        }

        public async Task<bool> Exists(int id) => await dbSet.AsQueryable().AsNoTracking().AnyAsync(p => p.Id.Equals(id));

        public async Task<TEntity> Insert(TEntity entity)
        {
            dbSet.Attach(entity);
            await dbContext.SaveChangesAsync(true);
            return entity;
        }

        public async Task<TEntity> Delete(int id)
        {
            var entity = await SelectById(id);

            dbSet.Remove(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            dbContext.ChangeTracker.Clear();

            dbSet.Update(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual IQueryable<TEntity> AsQueryable() => dbSet;

        public async Task<TEntity> SelectById(int id) => await dbSet.AsQueryable().AsNoTracking().FirstOrDefaultAsync(p => p.Id.Equals(id));
    }
}