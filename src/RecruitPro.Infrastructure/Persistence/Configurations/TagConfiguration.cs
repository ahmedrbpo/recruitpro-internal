using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Color).HasMaxLength(20);
        builder.Property(t => t.Category).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(t => t.RowVersion).IsConcurrencyToken();

        builder.HasIndex(t => new { t.Name, t.Category }).IsUnique().HasFilter("is_deleted = false");
    }
}
