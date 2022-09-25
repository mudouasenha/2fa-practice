using Doodle.Api.Repository.Data.Contexts;
using Doodle.Domain.Entities;
using Doodle.Infrastructure.Repository.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Doodle.Api.Repository.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly DoodleDbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        public Repository(DoodleDbContext dbContext)
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
            dbSet.Update(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual IQueryable<TEntity> AsQueryable() => dbSet;

        public async Task<TEntity> SelectById(int id) => await dbSet.AsQueryable().AsNoTracking().FirstOrDefaultAsync(p => p.Id.Equals(id));
    }
}