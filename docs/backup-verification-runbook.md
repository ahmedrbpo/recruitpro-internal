# Backup & Recovery Verification Runbook

Per the architecture blueprint's Data Management / Backup & Recovery section. RecruitPro's
database is Supabase-hosted PostgreSQL (the blueprint's RDS references are stale AWS-era
artifacts — see `CLAUDE.md`), so this runbook is written against Supabase's actual backup
tooling rather than RDS snapshots.

## What's already in place

| Control | Setting |
|---|---|
| Automated backups | Supabase's daily automated backups (Dashboard → Database → Backups). Retention depends on plan tier — confirm current retention window before relying on a specific number of days back. |
| Point-in-time recovery (PITR) | If enabled on the project's plan, allows restoring to any point within the retention window rather than only to a daily snapshot boundary. Confirm PITR is actually enabled (it's a paid add-on on some Supabase tiers, not automatic) — if not enabled, recovery granularity is limited to the daily snapshot cadence. |
| Manual snapshot before migrations | Take an explicit manual backup (Dashboard → Database → Backups → "Create backup") before applying any migration to the production database, in addition to the automated daily one — matches the blueprint's "before every major migration or schema change" rule. |

## Quarterly recovery test procedure

Run this once per quarter, and additionally before any major release, per the blueprint's
testing cadence. Do this in Supabase, **not** against the production project directly.

1. **Note the current state.** From the Supabase dashboard, record the most recent automated
   backup's timestamp (Dashboard → Database → Backups) and the current schema migration version
   (`SELECT * FROM "__EFMigrationsHistory" ORDER BY migration_id DESC LIMIT 1;` against
   production, read-only).
2. **Restore into a scratch project**, not in place. Supabase's restore flow can target a new
   project rather than overwriting the existing one — use that path. Never restore directly onto
   the production project as a "test."
3. **Point a local API instance at the restored scratch database** via
   `ConnectionStrings__Default` (same mechanism used for every deploy this project has done —
   see the MonsterASP.NET `web.config` `environmentVariables` pattern), and confirm:
   - The app boots without an EF "pending model changes" warning (confirms the restored schema
     matches what the current migration history expects).
   - `dotnet ef migrations list --project src/RecruitPro.Infrastructure --startup-project
     src/RecruitPro.Api` (run inside Docker per this project's standing WDAC workaround) shows
     the expected migration as already applied, not pending.
   - A real login against a seeded account (e.g. `administrator@coventine.com`) succeeds and
     returns the expected permission set — confirms both data and RBAC seed rows survived the
     restore intact.
   - Spot-check row counts on a couple of core tables (`jobs`, `candidates`, `applications`)
     against what you'd expect from the backup's timestamp.
4. **Record the result.** Log the date, the backup timestamp restored, pass/fail, and time taken
   to complete the restore (this becomes your basis for the Recovery Time Objective — the
   blueprint's NFR target is RTO ≤ 4 hours via manual restore-and-redeploy until Multi-AZ-style
   failover is adopted).
5. **Tear down the scratch project** once verified — don't leave a second live copy of
   production data sitting around indefinitely.

## What this does NOT cover

This runbook verifies that a backup is restorable and the application functions against the
restored data — it does not itself constitute a disaster-recovery drill for the API/frontend
tiers (MonsterASP.NET, Vercel-or-equivalent), which would need a corresponding runbook once
those hosting choices are load-bearing enough to justify one (see the blueprint's Part 2 trigger
tables — don't build that ahead of an actual observed need).
