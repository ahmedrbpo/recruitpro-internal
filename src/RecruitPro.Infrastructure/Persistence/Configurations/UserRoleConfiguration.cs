using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");
        builder.HasKey(ur => ur.Id);
        builder.Property(ur => ur.RowVersion).IsConcurrencyToken();

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique().HasFilter("is_deleted = false");
    }
}
