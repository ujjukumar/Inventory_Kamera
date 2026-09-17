# Changelog

## Port of post–March 2026 `master` changes into `fix-kamera-issues`

The `fix-kamera-issues` branch is a .NET 10 / split-solution modernization
(`InventoryKamera.Services` + `InventoryKamera.WinForms`) that has diverged
architecturally from the fork's `master`. A straight merge was therefore unsafe,
so the meaningful backend changes pushed to `origin/master` after March 2026 were
**selectively ported** into the new file layout. Each change below was adapted to
the modernized structure and verified with a live forced game-data update
(`UpdateGameData(force: true)`), which returned `Success` with
117 characters, 247 weapons, 61 artifacts, and 715 materials regenerated with no
data loss.

### Ported

| SHA | Summary | How it was ported |
| --- | --- | --- |
| `77a589a` | Add `TextMap_MediumEN` mapping source | Added `TextMapMediumEnURL` constant and merged the medium TextMap into the primary dictionary in `LoadMappings()` (`DatabaseManager.cs`). |
| `43c1612` | Skip non-playable characters | Added `if (characterID > 10000900) return; // Not playable characters` guard in `UpdateCharacters()`. |
| `45e8968` | Prevent data loss on failed updates | Replaced the "delete file, then download" pattern with a download-validate-then-replace flow across `UpdateCharacters`, `UpdateArtifacts`, `UpdateWeapons`, and `UpdateMaterials`, backed by new `ValidateCharacterData`/`ValidateArtifactData`/`ValidateWeaponData`/`ValidateMaterialData` helpers. Existing data is preserved unless the freshly downloaded dataset validates. |
| `30a156c` | Switch to `AnimeGameData2` source + fix weapon keys | Updated `repoBaseURL` to `https://gitlab.com/Dimbreath/AnimeGameData2/-/raw/main/` and corrected the critical weapon validation keys to `silversword` and `beginnersprotector`. |
| `e10075c`, `00254f7` | Bump version 1.4.2 → 1.4.4 | Applied via SDK-style `.csproj` properties (`Version`/`AssemblyVersion`/`FileVersion`) in `InventoryKamera.WinForms.csproj` and `InventoryKamera.Services.csproj`, since this branch has no `AssemblyInfo.cs`. |

### Skipped

| SHA | Summary | Reason |
| --- | --- | --- |
| `492645a` | Merge commit for `45e8968` | Empty merge commit (`Merge: e10075c 45e8968`) with no additional changes; the underlying fix was ported directly. |

### Follow-up fixes (post-port)

Two version-detection bugs surfaced after the port when a real update reported the
game version as `6.4` even though the downloaded data was `6.7`:

- **Stale version source.** `30a156c` repointed `repoBaseURL` to `AnimeGameData2`
  but left `commitsAPIURL` on the old, now-frozen `AnimeGameData` project
  (`53216109`). Version detection therefore lagged the data. `commitsAPIURL` now
  targets `AnimeGameData2` (project `83871005`) so the detected version matches the
  data being downloaded.
- **Version not refreshed on non-forced updates.** `UpdateGameData()` only called
  `CheckRemoteVersion()` when `force == true`, so the manual "Update Database" menu
  action rewrote the data files but never advanced `LocalVersion`/`version.txt`. The
  remote version is now resolved on every update (forced or not), keeping the
  previous version if the lookup fails.

## Second upstream sync — `taiwenlee/master` up to v1.4.5

Compared the branch against the parent repo `taiwenlee/Inventory_Kamera` (added as
the `upstream` remote). The histories share no common ancestor because this branch
is a rewritten/modernized layout, but everything up to `00254f7` (v1.4.4) was
already ported in the previous sync. Only two upstream commits after that point
carried new content, both ported below.

### Ported

| SHA | Summary | How it was ported |
| --- | --- | --- |
| `480222b` | Fix: Update Lookup Bug | Made character constellation logic null-safe on newer game data by switching `entry["skillIcon"/"openConfig"/"icon"].ToString()` to `entry.Value<string>(...)?.Contains(...) == true` in `UpdateCharacters()`, preventing `NullReferenceException` when those fields are missing. The graceful skip for characters without skill data was already present from the earlier `43c1612` port. |
| `427b868` | Bump assembly version to 1.4.5 | Applied via SDK-style `.csproj` properties (`Version`/`AssemblyVersion`/`FileVersion` = 1.4.5) in `InventoryKamera.WinForms.csproj` and `InventoryKamera.Services.csproj`. |

## Bug fix — artifact scan crash on partial last page

`ArtifactScraper` crashed with `ArgumentException: Parameter is not valid` at
`Image.get_Width()` (`InventoryScraper.ProcessScreenshot`) whenever the final
inventory page was not full (e.g. 31 artifacts detected as 4×7 instead of 32).
`ProcessScreenshot` disposed the `Bitmap` it was handed
(`screenshot.Dispose()` after `ApplyKirschFilter()`), but `GetPageOfItems` calls it
**repeatedly with the same bitmap** inside a retry loop. On the retry the bitmap was
already disposed, so reading its dimensions threw. Fixed by having
`ProcessScreenshot` run its Kirsch/threshold pre-processing on a private disposable
copy (`using (Bitmap working = screenshot.ApplyKirschFilter())`) and leaving the
caller-owned `screenshot` intact. This also removes a latent double-dispose, since
`GetPageOfItems` already disposes `processedScreenshot` after the loop.



