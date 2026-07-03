# Migration Guide: Adopting `master` Changes into a Modernized Branch

This guide documents the process and hard-won lessons for selectively porting
changes from the fork's `master` branch into a heavily modernized working branch
(e.g. `fix-kamera-issues`). It exists because this branch has **diverged
architecturally** from `master` and a plain `git merge` is unsafe.

> **TL;DR** — Don't merge. Enumerate the commits, capture the exact diffs, port
> each change *by intent* into the new layout, watch for related settings that
> must move together, then **verify at runtime** before documenting.

---

## Why not just `git merge master`?

`master` is the original single-project layout. This branch is a `.NET 10`,
SDK-style, split solution:

*   `InventoryKamera.Services` — core scanning / OCR / database logic
*   `InventoryKamera.WinForms` — legacy UI fallback

Files were moved, namespaces changed, static `NLog` became DI + `ILogger`, and
`AssemblyInfo.cs` was replaced by csproj properties. A wholesale merge would
produce massive conflicts and risk silently reintroducing patterns the
modernization deliberately removed. Instead, port **the meaning** of each commit,
adapting it to the new structure.

---

## The porting workflow

### 1. Fetch and enumerate
```pwsh
git fetch origin
git log --oneline origin/master        # list candidate commits
```
Identify the meaningful commits since the branch point. Ignore purely cosmetic or
already-present changes.

### 2. Capture the exact diff for each commit
```pwsh
git show <sha> --stat --no-color       # what files/areas it touches
git show <sha> --no-color              # the exact change
```
Read the real diff — never port from memory or a commit title alone.

### 3. Classify each commit
| Classification | Action |
| --- | --- |
| Meaningful backend logic | Port by intent into the new file layout |
| Empty merge commit (`Merge: a b`, no diff) | **Skip** — the underlying fix is a separate commit |
| Already present / superseded | Skip |
| UI-only against the old frontend | Re-implement against the current UI, or skip if N/A |

### 4. Port by intent, not by patch
`git apply` / `git cherry-pick` will usually fail because paths and code shape
differ. Instead, locate the equivalent code in the new layout and apply the
*intent* of the change. Adapt idioms to the branch's conventions (DI/`ILogger`
instead of static `NLog`, csproj version properties instead of `AssemblyInfo.cs`,
etc.).

### 5. Watch for "related settings that must move together"
This is the highest-value lesson from this migration. A single logical change is
sometimes spread across **multiple constants/files**, and porting only the obvious
one leaves a latent bug.

> **Real example:** commit `30a156c` repointed the data source `repoBaseURL` to the
> new `AnimeGameData2` GitLab repo, but the *version-detection* URL
> (`commitsAPIURL`, a different project ID) was left on the old, now-frozen repo.
> Result: data updated to 6.7 while the app reported version 6.4. The two URLs
> describe the *same* data source and must be changed together.

When you change a constant, **grep for siblings** that reference the same external
system, ID, or path:
```pwsh
# after changing a URL/host/project id, look for anything still pointing at the old one
Select-String -Path .\**\*.cs -Pattern '53216109|AnimeGameData/-/raw' 
```

### 6. Beware behavior that only differs at runtime
Some ported logic compiles fine but is wrong at runtime. In this migration, a
version refresh only ran on `force == true`, so the non-forced "Update Database"
menu path rewrote data files without ever advancing the displayed version. **A
green build is not proof the port works.**

### 7. Verify with a contained, disposable harness
For backend logic that talks to live data, build a tiny throwaway console project
that references `InventoryKamera.Services`, exercises the ported path against the
real source, and prints observable results (status + entry counts + version).
```pwsh
# scratch project outside the solution; deleted afterwards
dotnet run -c Debug
# assert: STATUS=Success, expected counts, expected version
```
Reproduce the *exact* reported scenario (e.g. seed a stale `version.txt`, then call
the **non-forced** path) so the test actually covers the bug. **Delete the harness
when done** and confirm `git status` shows only the intended files:
```pwsh
Remove-Item -Recurse -Force .\__scratch
git status --short
```

### 8. Build the full solution
```pwsh
# or use the IDE build
dotnet build .\InventoryKamera.slnx
```

### 9. Document the port
Record every ported and skipped commit — with SHA and rationale — in
[`CHANGELOG.md`](./CHANGELOG.md). Note *how* each change was adapted, not just that
it happened. This is the audit trail for the next person who compares against
`master`.

---

## Lessons learned (checklist)

- [ ] **Never blind-merge** a divergent branch — port by intent.
- [ ] **Read the real diff** (`git show <sha>`), not the commit title.
- [ ] **Skip empty merge commits**; port the underlying implementation commit.
- [ ] **Adapt to current idioms** (DI/`ILogger`, csproj version props, split projects).
- [ ] **Move related settings together** — grep for sibling constants (URLs, IDs, paths).
- [ ] **Preserve data until validated** — never delete-then-download; download,
	  validate, *then* replace, so a failed update can't destroy good data.
- [ ] **Version bumps live in the `.csproj`** (`Version`/`AssemblyVersion`/`FileVersion`)
	  on SDK-style projects — there is no `AssemblyInfo.cs`.
- [ ] **A clean build ≠ correct behavior** — verify the runtime path.
- [ ] **Reproduce the exact scenario** in a disposable harness against live data.
- [ ] **Clean up** scratch projects; confirm `git status` shows only intended files.
- [ ] **Document ported vs skipped** commits with SHAs and rationale in the changelog.

---

## Quick reference: commands used

```pwsh
# discover
git fetch origin
git log --oneline origin/master

# inspect
git show <sha> --stat --no-color
git show <sha> --no-color

# find siblings after a constant/URL/ID change
Select-String -Path .\**\*.cs -Pattern '<old-id-or-url>'

# verify (disposable harness), then clean up
dotnet run -c Debug
Remove-Item -Recurse -Force .\__scratch
git status --short
```
