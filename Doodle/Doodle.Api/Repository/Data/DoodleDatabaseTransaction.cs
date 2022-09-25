﻿using Doodle.Api.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

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