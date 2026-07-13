using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).HasMaxLength(250).IsRequired();
        builder.Property(c => c.Code).HasMaxLength(30).IsRequired();
        builder.Property(c => c.Type).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(c => c.Industry).HasMaxLength(100);
        builder.Property(c => c.HQCountry).HasMaxLength(100);
        builder.Property(c => c.Email).HasMaxLength(320);
        builder.Property(c => c.Phone).HasMaxLength(30);
        builder.Property(c => c.Website).HasMaxLength(250);
        builder.Property(c => c.GSTNumber).HasMaxLength(20);
        builder.Property(c => c.CurrencyCode).HasMaxLength(3);
        builder.Property(c => c.City).HasMaxLength(100);
        builder.Property(c => c.State).HasMaxLength(100);
        builder.Property(c => c.Country).HasMaxLength(100);
        builder.Property(c => c.PostalCode).HasMaxLength(20);
        builder.Property(c => c.RowVersion).IsConcurrencyToken();

        builder.HasIndex(c => c.ClientExtId).IsUnique();
        builder.HasIndex(c => c.Code).IsUnique().HasFilter("is_deleted = false");
        builder.HasIndex(c => c.IsActive).HasFilter("is_deleted = false");
    }
}
