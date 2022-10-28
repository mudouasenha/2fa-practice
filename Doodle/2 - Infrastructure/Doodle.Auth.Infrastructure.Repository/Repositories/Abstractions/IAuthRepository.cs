using Microsoft.AspNetCore.Identity;

namespace Doodle.Auth.Infrastructure.Repository.Repositories.Abstractions
{
    public interface IAuthRepository<TEntity> where TEntity : IdentityUser<Guid>
    {
        Task<bool> Exists(int id);

        Task<TEntity> Insert(TEntity entity);

        Task<TEntity> Delete(int id);

        Task<TEntity> Update(TEntity entity);

        IQueryable<TEntity> AsQueryable();

        Task<TEntity> SelectById(int id);
    }
}