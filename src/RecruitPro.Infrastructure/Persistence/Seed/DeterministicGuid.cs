using System.Security.Cryptography;
using System.Text;

namespace RecruitPro.Infrastructure.Persistence.Seed;

/// <summary>Derives a stable Guid from a string key. HasData seed rows must evaluate to
/// byte-identical values every time OnModelCreating runs (both at `dotnet ef migrations add`
/// time, when the values get frozen into the migration file, and at every app startup, when EF
/// re-validates the current model against that frozen snapshot) — Guid.NewGuid() would produce a
/// different value on each run and EF would report the model as perpetually out of sync with its
/// migrations. MD5 is used purely as a deterministic 16-byte digest here, not for any
/// cryptographic property.</summary>
internal static class DeterministicGuid
{
    public static Guid Create(string key) => new(MD5.HashData(Encoding.UTF8.GetBytes(key)));
}
