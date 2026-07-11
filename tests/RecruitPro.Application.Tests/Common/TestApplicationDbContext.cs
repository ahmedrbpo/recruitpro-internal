using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Domain.Identity.Entities;

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

    public static TestApplicationDbContext CreateInMemory() =>
        new(new DbContextOptionsBuilder<TestApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);
}
