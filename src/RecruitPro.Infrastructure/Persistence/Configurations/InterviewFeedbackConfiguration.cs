using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class InterviewFeedbackConfiguration : IEntityTypeConfiguration<InterviewFeedback>
{
    public void Configure(EntityTypeBuilder<InterviewFeedback> builder)
    {
        builder.ToTable("interview_feedback");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Recommendation).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(f => f.Comments).HasColumnType("text");
        builder.Property(f => f.RowVersion).IsConcurrencyToken();

        builder.HasIndex(f => f.InterviewId);
        builder.HasIndex(f => new { f.InterviewId, f.InterviewerId }).IsUnique().HasFilter("is_deleted = false");

        builder.HasOne(f => f.Interviewer)
            .WithMany()
            .HasForeignKey(f => f.InterviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
