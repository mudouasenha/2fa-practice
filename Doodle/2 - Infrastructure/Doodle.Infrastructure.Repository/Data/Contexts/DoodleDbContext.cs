﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Doodle.Infrastructure.Repository.Auth.Data.Contexts
{
    public class DoodleDbContext : DbContext
    {
        public DoodleDbContext(DbContextOptions<DoodleDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}