using Doodle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doodle.Infrastructure.Repository.Repositories.Abstractions
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
    }
}
