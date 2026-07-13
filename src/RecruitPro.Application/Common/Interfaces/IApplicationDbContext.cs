using Microsoft.EntityFrameworkCore;
using RecruitPro.Domain.Identity.Entities;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<AuditLog> AuditLogs { get; }

    DbSet<Job> Jobs { get; }
    DbSet<JobSkill> JobSkills { get; }
    DbSet<Skill> Skills { get; }
    DbSet<Department> Departments { get; }
    DbSet<JobCategory> JobCategories { get; }
    DbSet<Client> Clients { get; }
    DbSet<Recruiter> Recruiters { get; }
    DbSet<Tag> Tags { get; }

    DbSet<Candidate> Candidates { get; }
    DbSet<Resume> Resumes { get; }

    DbSet<JobApplication> Applications { get; }
    DbSet<ApplicationStageHistory> ApplicationStageHistories { get; }

    DbSet<Interview> Interviews { get; }
    DbSet<InterviewFeedback> InterviewFeedbacks { get; }
    DbSet<Offer> Offers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
