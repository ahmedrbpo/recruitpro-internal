using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class CandidateEducationConfiguration : IEntityTypeConfiguration<CandidateEducation>
{
    public void Configure(EntityTypeBuilder<CandidateEducation> builder)
    {
        builder.ToTable("candidate_education");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.College).HasMaxLength(300).IsRequired();
        builder.Property(e => e.Degree).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Stream).HasMaxLength(200);
        builder.Property(e => e.Type).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(e => e.StartDate).HasColumnType("date").IsRequired();
        builder.Property(e => e.EndDate).HasColumnType("date");
        builder.Property(e => e.Location).HasMaxLength(200);
        builder.Property(e => e.RowVersion).IsConcurrencyToken();

        builder.HasIndex(e => e.CandidateId);
    }
}
