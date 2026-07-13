using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Identity.Entities;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name).HasMaxLength(200).IsRequired();
        builder.Property(d => d.DepartmentCode).HasMaxLength(30);
        builder.Property(d => d.RowVersion).IsConcurrencyToken();

        builder.HasIndex(d => d.DepartmentExtId).IsUnique();
        builder.HasIndex(d => d.Name).IsUnique().HasFilter("is_deleted = false");
        builder.HasIndex(d => d.ParentDepartmentId);
        builder.HasIndex(d => d.ManagerId);

        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(d => d.ParentDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
