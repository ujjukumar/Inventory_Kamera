using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace InventoryKamera.Properties
{
    /// <summary>
    /// Service for loading and saving application settings.
    /// Replaces the legacy JsonUserSettingsProvider with a simpler, modern approach.
    /// </summary>
    public sealed class SettingsService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static SettingsService _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets the singleton instance of the SettingsService.
        /// </summary>
        public static SettingsService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SettingsService();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the current application settings.
        /// </summary>
        public AppSettings Settings { get; private set; }

        /// <summary>
        /// Gets the directory where settings are stored.
        /// </summary>
        public string SettingsDirectory { get; }

        /// <summary>
        /// Gets the full path to the settings file.
        /// </summary>
        public string SettingsFilePath { get; }

        private const string SettingsFileName = "settings.json";

        private SettingsService()
        {
            SettingsDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Path.GetFileNameWithoutExtension(Application.ExecutablePath));

            SettingsFilePath = Path.Combine(SettingsDirectory, SettingsFileName);

            Settings = new AppSettings();
            Load();
        }

        /// <summary>
        /// Loads settings from the JSON file.
        /// If the file doesn't exist or is invalid, default settings are used.
        /// </summary>
        public void Load()
        {
            try
            {
                if (!File.Exists(SettingsFilePath))
                {
                    Logger.Info("Settings file not found, using defaults: {0}", SettingsFilePath);
                    return;
                }

                var json = File.ReadAllText(SettingsFilePath);
                
                // Check if the file contains XML instead of JSON (corrupted/legacy format)
                if (json.TrimStart().StartsWith("<?xml"))
                {
                    Logger.Warn("Settings file contains XML instead of JSON. Deleting corrupted file and using defaults.");
                    try
                    {
                        File.Delete(SettingsFilePath);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "Failed to delete corrupted settings file");
                    }
                    return;
                }

                var loadedSettings = JsonConvert.DeserializeObject<AppSettings>(json);

                if (loadedSettings != null)
                {
                    Settings = loadedSettings;
                    Logger.Info("Settings loaded from: {0}", SettingsFilePath);
                }
                else
                {
                    Logger.Warn("Deserialized settings were null, using defaults");
                }
            }
            catch (JsonSerializationException ex)
            {
                Logger.Warn(ex, "Failed to parse settings file, using defaults. The corrupted file will be backed up.");
                BackupAndDeleteCorruptedSettings();
            }
            catch (JsonReaderException ex)
            {
                Logger.Warn(ex, "Failed to read settings file as JSON, using defaults. The corrupted file will be backed up.");
                BackupAndDeleteCorruptedSettings();
            }
            catch (IOException ex)
            {
                Logger.Warn(ex, "Failed to read settings file, using defaults");
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "Unexpected error loading settings, using defaults");
            }
        }

        /// <summary>
        /// Backs up and deletes a corrupted settings file to allow fresh start
        /// </summary>
        private void BackupAndDeleteCorruptedSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var backupPath = SettingsFilePath + ".corrupted." + DateTime.Now.ToString("yyyyMMddHHmmss");
                    File.Copy(SettingsFilePath, backupPath, true);
                    Logger.Info("Backed up corrupted settings to: {0}", backupPath);
                    
                    File.Delete(SettingsFilePath);
                    Logger.Info("Deleted corrupted settings file");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to backup/delete corrupted settings file");
            }
        }

        /// <summary>
        /// Saves the current settings to the JSON file.
        /// </summary>
        public void Save()
        {
            try
            {
                Directory.CreateDirectory(SettingsDirectory);

                var json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);

                Logger.Debug("Settings saved to: {0}", SettingsFilePath);
            }
            catch (IOException ex)
            {
                Logger.Error(ex, "Failed to save settings");
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.Error(ex, "Access denied when saving settings");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unexpected error saving settings");
            }
        }

        /// <summary>
        /// Migrates settings from the legacy Properties.Settings.Default format.
        /// Call this once on application startup to transfer old settings.
        /// </summary>
        public void MigrateFromLegacySettings()
        {
            try
            {
                // Check if we've already migrated
                if (!Settings.UpgradeNeeded)
                {
                    return;
                }

                // Check if legacy settings exist
                var legacySettings = global::InventoryKamera.Properties.Settings.Default;

                // Transfer all settings
                Settings.ScanWeapons = legacySettings.ScanWeapons;
                Settings.ScanArtifacts = legacySettings.ScanArtifacts;
                Settings.ScanCharacters = legacySettings.ScanCharacters;
                Settings.ScanCharDevItems = legacySettings.ScanCharDevItems;
                Settings.ScanMaterials = legacySettings.ScanMaterials;
                Settings.ScannerDelay = legacySettings.ScannerDelay;

                Settings.MinimumWeaponRarity = legacySettings.MinimumWeaponRarity;
                Settings.MinimumArtifactRarity = legacySettings.MinimumArtifactRarity;
                Settings.MinimumWeaponLevel = legacySettings.MinimumWeaponLevel;
                Settings.MinimumArtifactLevel = legacySettings.MinimumArtifactLevel;
                Settings.SortByObtained = legacySettings.SortByObtained;
                Settings.NumOfCharToScan = legacySettings.NumOfCharToScan;

                Settings.OutputPath = legacySettings.OutputPath ?? string.Empty;
                Settings.EquipWeapons = legacySettings.EquipWeapons;
                Settings.EquipArtifacts = legacySettings.EquipArtifacts;

                Settings.InventoryKey = legacySettings.InventoryKey;
                Settings.CharacterKey = legacySettings.CharacterKey;
                Settings.Slot1Key = legacySettings.Slot1Key;

                Settings.TravelerName = legacySettings.TravelerName ?? string.Empty;
                Settings.WandererName = legacySettings.WandererName ?? "Wanderer";
                Settings.Manequin1Name = legacySettings.Manequin1Name ?? "Manequin1";
                Settings.Manequin2Name = legacySettings.Manequin2Name ?? "Manequin2";

                Settings.LogScreenshots = legacySettings.LogScreenshots;

                Settings.LastUpdateCheck = legacySettings.LastUpdateCheck;
                Settings.RemoteVersion = legacySettings.RemoteVersion ?? "0.0.0.0";

                // Convert StringCollection to List<string> with proper error handling
                if (legacySettings.Executables != null && legacySettings.Executables.Count > 0)
                {
                    var executables = new List<string>();
                    foreach (string exe in legacySettings.Executables)
                    {
                        if (!string.IsNullOrEmpty(exe))
                        {
                            executables.Add(exe);
                        }
                    }
                    Settings.Executables = executables;
                }

                // Mark migration as complete
                Settings.UpgradeNeeded = false;
                Save();

                Logger.Info("Successfully migrated settings from legacy format");
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "Failed to migrate legacy settings, using defaults");
            }
        }

        /// <summary>
        /// Resets all settings to their default values.
        /// </summary>
        public void ResetToDefaults()
        {
            Settings = new AppSettings();
            Save();
            Logger.Info("Settings reset to defaults");
        }
    }
}
