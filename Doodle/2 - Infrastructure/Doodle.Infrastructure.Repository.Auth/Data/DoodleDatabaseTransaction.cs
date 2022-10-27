using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doodle.Infrastructure.Repository.Auth.Data.Contexts;
using Doodle.Infrastructure.Repository.Auth.Data;

namespace Doodle.Infrastructure.Repository.Auth.Data
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
