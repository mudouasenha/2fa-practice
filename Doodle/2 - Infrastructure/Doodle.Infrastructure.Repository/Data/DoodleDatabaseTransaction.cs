using Doodle.Infrastructure.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doodle.Infrastructure.Repository.Data
{
    public interface IDatabaseTransaction
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
    }

    public class DoodleDatabaseTransaction : IDatabaseTransaction
    {
        private readonly DoodleDbContext _context;

        public DoodleDatabaseTransaction(DoodleDbContext context) => _context = context;

        public Task<IDbContextTransaction> BeginTransactionAsync() => _context.Database.BeginTransactionAsync();
    }
}
