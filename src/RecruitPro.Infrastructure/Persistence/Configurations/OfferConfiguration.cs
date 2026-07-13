using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("offers");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.OfferedSalary).HasColumnType("numeric(12,2)");
        builder.Property(o => o.CurrencyCode).HasMaxLength(3).IsRequired();
        builder.Property(o => o.OfferDate).HasColumnType("date");
        builder.Property(o => o.JoiningDate).HasColumnType("date");
        builder.Property(o => o.ExpiryDate).HasColumnType("date");
        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(o => o.Notes).HasColumnType("text");
        builder.Property(o => o.RowVersion).IsConcurrencyToken();

        // 1:1 with JobApplication.
        builder.HasIndex(o => o.ApplicationId).IsUnique().HasFilter("is_deleted = false");

        builder.HasOne(o => o.Application)
            .WithMany()
            .HasForeignKey(o => o.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
