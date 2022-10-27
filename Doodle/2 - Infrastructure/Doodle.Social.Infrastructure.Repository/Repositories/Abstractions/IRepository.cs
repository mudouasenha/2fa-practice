using Doodle.Domain.Entities;

namespace Doodle.Social.Infrastructure.Repository.Repositories.Abstractions
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task<bool> Exists(int id);

        Task<TEntity> Insert(TEntity entity);

        Task<TEntity> Delete(int id);

        Task<TEntity> Update(TEntity entity);

        IQueryable<TEntity> AsQueryable();

        Task<TEntity> SelectById(int id);
    }
}