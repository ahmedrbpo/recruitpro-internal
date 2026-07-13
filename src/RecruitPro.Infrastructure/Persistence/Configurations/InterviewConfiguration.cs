using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Identity.Entities;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class InterviewConfiguration : IEntityTypeConfiguration<Interview>
{
    public void Configure(EntityTypeBuilder<Interview> builder)
    {
        builder.ToTable("interviews");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Mode).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(i => i.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(i => i.Notes).HasColumnType("text");
        builder.Property(i => i.RowVersion).IsConcurrencyToken();

        builder.HasIndex(i => i.ApplicationId);
        builder.HasIndex(i => i.InterviewerId);
        builder.HasIndex(i => i.ScheduledAt);

        builder.HasOne(i => i.Application)
            .WithMany()
            .HasForeignKey(i => i.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Interviewer)
            .WithMany()
            .HasForeignKey(i => i.InterviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.Feedback)
            .WithOne(f => f.Interview)
            .HasForeignKey(f => f.InterviewId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(i => i.Feedback).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
