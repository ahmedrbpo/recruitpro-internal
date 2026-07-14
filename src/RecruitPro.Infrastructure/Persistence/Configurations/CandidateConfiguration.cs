using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable("candidates");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(320).IsRequired();
        builder.Property(c => c.Phone).HasMaxLength(30);
        builder.Property(c => c.Gender).HasConversion<string>().HasMaxLength(10);
        builder.Property(c => c.DateOfBirth).HasColumnType("date");
        builder.Property(c => c.Pan).HasMaxLength(10);
        builder.Property(c => c.TotalExperienceYears).HasColumnType("numeric(4,1)");
        builder.Property(c => c.RelevantExperienceYears).HasColumnType("numeric(4,1)");
        builder.Property(c => c.StreetAddress).HasMaxLength(500);
        builder.Property(c => c.City).HasMaxLength(100);
        builder.Property(c => c.State).HasMaxLength(100);
        builder.Property(c => c.PostalCode).HasMaxLength(20);
        builder.Property(c => c.WorkLocation).HasMaxLength(200);
        builder.Property(c => c.RowVersion).IsConcurrencyToken();

        builder.HasIndex(c => c.Email).HasFilter("is_deleted = false");

        builder.HasMany(c => c.Resumes)
            .WithOne(r => r.Candidate)
            .HasForeignKey(r => r.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(c => c.Resumes).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(c => c.Educations)
            .WithOne(e => e.Candidate)
            .HasForeignKey(e => e.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(c => c.Educations).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(c => c.EmploymentHistories)
            .WithOne(e => e.Candidate)
            .HasForeignKey(e => e.CandidateId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(c => c.EmploymentHistories).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
