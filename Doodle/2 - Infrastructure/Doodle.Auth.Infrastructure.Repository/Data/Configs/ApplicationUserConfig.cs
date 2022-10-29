﻿using Doodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doodle.Auth.Infrastructure.Repository.Data.Configs
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Address).HasColumnType("varchar").HasMaxLength(200);
            builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(70).IsRequired();
            builder.Property(p => p.Email).HasColumnType("varchar").HasMaxLength(62).IsRequired();
            builder.Property(p => p.PhoneNumber).HasColumnType("varchar").HasMaxLength(15).IsRequired();
            builder.Property(p => p.UserName).HasColumnType("varchar").HasMaxLength(64).IsRequired();
            builder.Property(p => p.PasswordHash).HasColumnType("varchar").HasMaxLength(255).IsRequired();
            builder.Property(p => p.Salt).HasColumnType("varchar").HasMaxLength(64);
            builder.Property(p => p.MfaEnabled).HasColumnType("bit").IsRequired().HasDefaultValue(false);
            builder.Property(p => p.Verified).HasColumnType("bit").IsRequired().HasDefaultValue(false);
            builder.Property(p => p.MfaIdentity).HasColumnType("varchar").HasMaxLength(36);
            builder.Property(p => p.CreatedAt).HasColumnType("datetime").IsRequired().HasDefaultValue(DateTime.MinValue);
            builder.Property(p => p.UpdatedAt).HasColumnType("datetime");

            builder.HasIndex(p => p.UserName, "nci_Users_Username").IsUnique().IsClustered(false);
            builder.HasIndex(p => p.Email, "nci_Users_Email").IsUnique().IsClustered(false);
        }
    }
}