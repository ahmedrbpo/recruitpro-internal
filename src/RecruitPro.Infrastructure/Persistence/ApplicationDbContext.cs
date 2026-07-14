using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Domain.Common;
using RecruitPro.Domain.Identity.Entities;
using RecruitPro.Domain.Notifications.Entities;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Infrastructure.Persistence.Seed;

namespace RecruitPro.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

    public DbSet<JobApplication> Applications => Set<JobApplication>();
    public DbSet<ApplicationStageHistory> ApplicationStageHistories => Set<ApplicationStageHistory>();

    public DbSet<Interview> Interviews => Set<Interview>();
    public DbSet<InterviewFeedback> InterviewFeedbacks => Set<InterviewFeedback>();
    public DbSet<Offer> Offers => Set<Offer>();

    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            typeof(ApplicationDbContext)
                .GetMethod(nameof(ApplySoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entityType.ClrType)
                .Invoke(null, [builder]);

            // Every Id is generated client-side via Guid.NewGuid() in BaseEntity, never by the
            // database. Without this, EF's change tracker can misjudge a child entity's state as
            // Modified instead of Added when it's reachable only through navigation-collection
            // fixup (e.g. JobApplication.MoveToStage() appending to its private history list) —
            // a non-default key looks like "already exists" unless told otherwise.
            builder.Entity(entityType.ClrType).Property(nameof(BaseEntity.Id)).ValueGeneratedNever();
        }

        // The RecruitPro Roles & Responsibilities org-hierarchy document, seeded here so a fresh
        // database gets these roles/permissions automatically via `dotnet ef database update`
        // instead of requiring the ad-hoc scratch-script grants every earlier phase relied on.
        builder.Entity<Permission>().HasData(RbacSeedData.GetPermissions());
        builder.Entity<Role>().HasData(RbacSeedData.GetRoles());
        builder.Entity<RolePermission>().HasData(RbacSeedData.GetRolePermissions());

        base.OnModelCreating(builder);
    }

    private static void ApplySoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity =>
        builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
}
