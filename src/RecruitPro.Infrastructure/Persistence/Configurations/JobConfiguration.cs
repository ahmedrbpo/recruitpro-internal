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

        builder.Property(j => j.JobCode).HasMaxLength(30).IsRequired();
        builder.Property(j => j.Title).HasMaxLength(300).IsRequired();
        builder.Property(j => j.Description).IsRequired();
        builder.Property(j => j.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(j => j.EmploymentType).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(j => j.WorkMode).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(j => j.ExperienceMin).HasColumnType("numeric(4,1)");
        builder.Property(j => j.ExperienceMax).HasColumnType("numeric(4,1)");
        builder.Property(j => j.SalaryMin).HasColumnType("numeric(12,2)");
        builder.Property(j => j.SalaryMax).HasColumnType("numeric(12,2)");
        builder.Property(j => j.CurrencyCode).HasMaxLength(3).IsRequired();
        builder.Property(j => j.PublishedDate).HasColumnType("date");
        builder.Property(j => j.Onboarding).HasConversion<string>().HasMaxLength(30);
        builder.Property(j => j.RowVersion).IsConcurrencyToken();

        builder.HasIndex(j => j.JobCode).IsUnique().HasFilter("is_deleted = false");
        builder.HasIndex(j => j.Status).HasFilter("is_deleted = false");
        builder.HasIndex(j => j.DepartmentId);
        builder.HasIndex(j => j.ClientId);
        builder.HasIndex(j => j.JobCategoryId);
        builder.HasIndex(j => j.RecruiterId);

        builder.HasOne<Department>()
            .WithMany()
            .HasForeignKey(j => j.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(j => j.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<JobCategory>()
            .WithMany()
            .HasForeignKey(j => j.JobCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Points at Recruiter, not ApplicationUser directly, so a vendor recruiter with no
        // login can still be assigned to a job.
        builder.HasOne<Recruiter>()
            .WithMany()
            .HasForeignKey(j => j.RecruiterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(j => j.Skills)
            .WithOne(s => s.Job)
            .HasForeignKey(s => s.JobId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(j => j.Skills).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
