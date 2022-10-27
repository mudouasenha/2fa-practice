using Doodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doodle.Auth.Infrastructure.Repository.Repositories.Abstractions
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
