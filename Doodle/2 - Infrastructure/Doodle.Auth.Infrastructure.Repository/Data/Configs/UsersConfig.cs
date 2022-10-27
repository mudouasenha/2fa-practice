using Doodle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Doodle.Auth.Infrastructure.Repository.Data.Configs
{
    public class UsersConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).UseIdentityColumn();
            builder.Property(p => p.Address).HasColumnType("varchar").HasMaxLength(200);
            builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(70).IsRequired();
            builder.Property(p => p.Email).HasColumnType("varchar").HasMaxLength(62).IsRequired();
            builder.Property(p => p.PhoneNumber).HasColumnType("varchar").HasMaxLength(15).IsRequired();
            builder.Property(p => p.Username).HasColumnType("varchar").HasMaxLength(64).IsRequired();
            builder.Property(p => p.Password).HasColumnType("varchar").HasMaxLength(255).IsRequired();
            builder.Property(p => p.Salt).HasColumnType("varchar").HasMaxLength(64).IsRequired();
            builder.Property(p => p.MfaEnabled).HasColumnType("bit").IsRequired().HasDefaultValue(false);
            builder.Property(p => p.Verified).HasColumnType("bit").IsRequired().HasDefaultValue(false);
            builder.Property(p => p.MfaIdentity).HasColumnType("varchar").HasMaxLength(36);
            builder.Property(p => p.CreatedAt).HasColumnType("datetime").IsRequired();
            builder.Property(p => p.UpdatedAt).HasColumnType("datetime");

            builder.HasIndex(p => p.Username, "nci_Users_Username").IsUnique().IsClustered(false);
            builder.HasIndex(p => p.Email, "nci_Users_Email").IsUnique().IsClustered(false);
        }
    }
}
