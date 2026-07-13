using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecruitPro.Domain.Identity.Entities;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Infrastructure.Persistence.Configurations;

public sealed class RecruiterConfiguration : IEntityTypeConfiguration<Recruiter>
{
    public void Configure(EntityTypeBuilder<Recruiter> builder)
    {
        builder.ToTable("recruiters");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Type).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(r => r.VendorCompany).HasMaxLength(250);
        builder.Property(r => r.PAN).HasMaxLength(20);
        builder.Property(r => r.Mobile).HasMaxLength(20);
        builder.Property(r => r.RowVersion).IsConcurrencyToken();

        builder.HasIndex(r => r.UserId).IsUnique().HasFilter("is_deleted = false");

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
