using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Notifications.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
{
    public void Configure(EntityTypeBuilder<NotificationLog> builder)
    {
        builder.ToTable("notification_logs");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Channel).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(n => n.RecipientEmail).HasMaxLength(320).IsRequired();
        builder.Property(n => n.RecipientName).HasMaxLength(200);
        builder.Property(n => n.TemplateCode).HasMaxLength(100);
        builder.Property(n => n.Subject).HasMaxLength(300).IsRequired();
        builder.Property(n => n.Body).HasColumnType("text").IsRequired();
        builder.Property(n => n.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(n => n.LastError).HasColumnType("text");
        builder.Property(n => n.RelatedEntityType).HasMaxLength(100);
        builder.Property(n => n.RowVersion).IsConcurrencyToken();

        // Backs the background job's `WHERE Status = Pending` polling query.
        builder.HasIndex(n => n.Status).HasFilter("status = 'Pending'");
        builder.HasIndex(n => new { n.RelatedEntityType, n.RelatedEntityId });
    }
}
