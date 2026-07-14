using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Domain.Common;
using RecruitPro.Domain.Identity.Entities;
using RecruitPro.Domain.Notifications.Entities;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Tests.Common;

/// <summary>
/// Minimal EF Core InMemory-backed IApplicationDbContext for Application-layer tests. Kept
/// in the test project (not the real Infrastructure.ApplicationDbContext) so these tests stay
/// isolated from Infrastructure per the Clean Architecture dependency rule.
/// </summary>
public sealed class TestApplicationDbContext(DbContextOptions<TestApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobSkill> JobSkills => Set<JobSkill>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<JobCategory> JobCategories => Set<JobCategory>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Recruiter> Recruiters => Set<Recruiter>();
    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<CandidateEducation> CandidateEducations => Set<CandidateEducation>();
    public DbSet<CandidateEmploymentHistory> CandidateEmploymentHistories => Set<CandidateEmploymentHistory>();

    public DbSet<JobApplication> Applications => Set<JobApplication>();
    public DbSet<ApplicationStageHistory> ApplicationStageHistories => Set<ApplicationStageHistory>();

    public DbSet<Interview> Interviews => Set<Interview>();
    public DbSet<InterviewFeedback> InterviewFeedbacks => Set<InterviewFeedback>();
    public DbSet<Offer> Offers => Set<Offer>();

    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Every Id is generated client-side via Guid.NewGuid() in BaseEntity, never by the
        // database. Without this, EF's change tracker can misjudge a child entity's state as
        // Modified instead of Added when it's reachable only through navigation-collection
        // fixup (e.g. JobApplication.MoveToStage() appending to its private history list) — a
        // non-default key looks like "already exists" unless told otherwise. Mirrors the real
        // Infrastructure.ApplicationDbContext's equivalent fix.
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                builder.Entity(entityType.ClrType).Property(nameof(BaseEntity.Id)).ValueGeneratedNever();
        }

        base.OnModelCreating(builder);
    }

    public static TestApplicationDbContext CreateInMemory() =>
        new(new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
}
