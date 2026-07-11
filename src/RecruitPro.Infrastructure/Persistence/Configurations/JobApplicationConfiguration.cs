using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        // Table name matches the blueprint's schema even though the C# type is JobApplication.
        builder.ToTable("applications");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Stage).HasMaxLength(30).IsRequired();
        builder.Property(a => a.RowVersion).IsConcurrencyToken();

        builder.HasIndex(a => a.JobId);
        builder.HasIndex(a => a.CandidateId);
        builder.HasIndex(a => a.Stage).HasFilter("stage NOT IN ('hired','rejected')");

        builder.HasOne(a => a.Job)
            .WithMany()
            .HasForeignKey(a => a.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Candidate)
            .WithMany()
            .HasForeignKey(a => a.CandidateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.StageHistory)
            .WithOne(h => h.Application)
            .HasForeignKey(h => h.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(a => a.StageHistory).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
