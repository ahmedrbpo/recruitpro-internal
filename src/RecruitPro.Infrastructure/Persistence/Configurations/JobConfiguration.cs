using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("jobs");
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Title).HasMaxLength(300).IsRequired();
        builder.Property(j => j.Status).HasMaxLength(30).IsRequired();
        builder.Property(j => j.SalaryMin).HasColumnType("numeric(12,2)");
        builder.Property(j => j.SalaryMax).HasColumnType("numeric(12,2)");
        builder.Property(j => j.RowVersion).IsConcurrencyToken();

        builder.HasIndex(j => j.Status).HasFilter("is_deleted = false");
        builder.HasIndex(j => j.DepartmentId);

        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(j => j.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(j => j.Skills)
            .WithOne(s => s.Job)
            .HasForeignKey(s => s.JobId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(j => j.Skills).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
