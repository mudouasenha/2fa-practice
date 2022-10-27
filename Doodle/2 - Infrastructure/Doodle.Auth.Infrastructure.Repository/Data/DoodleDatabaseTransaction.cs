using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doodle.Auth.Infrastructure.Repository.Data.Contexts;
using Doodle.Auth.Infrastructure.Repository.Data;

namespace Doodle.Auth.Infrastructure.Repository.Data
{
    public interface IDatabaseTransaction
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
    }

    public class DoodleDatabaseTransaction : IDatabaseTransaction
    {
        private readonly DoodleAuthDbContext _context;

        public DoodleDatabaseTransaction(DoodleAuthDbContext context) => _context = context;

        public Task<IDbContextTransaction> BeginTransactionAsync() => _context.Database.BeginTransactionAsync();
    }
}
