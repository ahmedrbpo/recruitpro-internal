using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class ResumeConfiguration : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.ToTable("resumes");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ObjectKey).HasMaxLength(500).IsRequired();
        builder.Property(r => r.OriginalFileName).HasMaxLength(255).IsRequired();
        builder.Property(r => r.ContentType).HasMaxLength(150).IsRequired();
        builder.Property(r => r.RowVersion).IsConcurrencyToken();

        builder.HasIndex(r => r.ObjectKey).IsUnique();
        builder.HasIndex(r => r.CandidateId);
    }
}
