using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Resource).HasMaxLength(100);
        builder.Property(p => p.Action).HasConversion<string>().HasMaxLength(30);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.RowVersion).IsConcurrencyToken();

        builder.HasIndex(p => p.PermissionExtId).IsUnique();
        builder.HasIndex(p => p.Name).IsUnique().HasFilter("is_deleted = false");

        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(p => p.RolePermissions).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
