using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doodle.Infrastructure.Repository.Extensions
{
    public class DbContextInitializer<TContext> : IAsyncInitializer where TContext : DbContext
    {
        private readonly TContext dbContext;

        public DbContextInitializer(TContext dbContext) => this.dbContext = dbContext;

        public async Task InitializeAsync() => await dbContext.Database.MigrateAsync();
    }
}