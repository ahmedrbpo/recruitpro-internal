using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class ApplicationStageHistoryConfiguration : IEntityTypeConfiguration<ApplicationStageHistory>
{
    public void Configure(EntityTypeBuilder<ApplicationStageHistory> builder)
    {
        builder.ToTable("application_stage_history");
        builder.HasKey(h => h.Id);

        builder.Property(h => h.FromStage).HasMaxLength(30).IsRequired();
        builder.Property(h => h.ToStage).HasMaxLength(30).IsRequired();
        builder.Property(h => h.RowVersion).IsConcurrencyToken();

        builder.HasIndex(h => h.ApplicationId);
    }
}
