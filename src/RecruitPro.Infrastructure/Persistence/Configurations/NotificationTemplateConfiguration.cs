using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Notifications.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.ToTable("notification_templates");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Code).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Name).HasMaxLength(150).IsRequired();
        builder.Property(t => t.Channel).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(t => t.Subject).HasMaxLength(300).IsRequired();
        builder.Property(t => t.Body).HasColumnType("text").IsRequired();
        builder.Property(t => t.RowVersion).IsConcurrencyToken();

        builder.HasIndex(t => t.Code).IsUnique();
    }
}
