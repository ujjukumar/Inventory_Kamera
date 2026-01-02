using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InventoryKamera.Properties
{
    /// <summary>
    /// Strongly-typed application settings with INotifyPropertyChanged support for data binding.
    /// Replaces the legacy Properties.Settings.Default pattern.
    /// </summary>
    public sealed class AppSettings : INotifyPropertyChanged
    {
        #region Scanning Options

        private bool _scanWeapons = true;
        public bool ScanWeapons
        {
            get => _scanWeapons;
            set => SetField(ref _scanWeapons, value);
        }

        private bool _scanArtifacts = true;
        public bool ScanArtifacts
        {
            get => _scanArtifacts;
            set => SetField(ref _scanArtifacts, value);
        }

        private bool _scanCharacters = true;
        public bool ScanCharacters
        {
            get => _scanCharacters;
            set => SetField(ref _scanCharacters, value);
        }

        private bool _scanCharDevItems = true;
        public bool ScanCharDevItems
        {
            get => _scanCharDevItems;
            set => SetField(ref _scanCharDevItems, value);
        }

        private bool _scanMaterials = true;
        public bool ScanMaterials
        {
            get => _scanMaterials;
            set => SetField(ref _scanMaterials, value);
        }

        private int _scannerDelay;
        public int ScannerDelay
        {
            get => _scannerDelay;
            set => SetField(ref _scannerDelay, value);
        }

        #endregion

        #region Filters

        private decimal _minimumWeaponRarity = 3;
        public decimal MinimumWeaponRarity
        {
            get => _minimumWeaponRarity;
            set => SetField(ref _minimumWeaponRarity, value);
        }

        private decimal _minimumArtifactRarity = 4;
        public decimal MinimumArtifactRarity
        {
            get => _minimumArtifactRarity;
            set => SetField(ref _minimumArtifactRarity, value);
        }

        private decimal _minimumWeaponLevel = 1;
        public decimal MinimumWeaponLevel
        {
            get => _minimumWeaponLevel;
            set => SetField(ref _minimumWeaponLevel, value);
        }

        private decimal _minimumArtifactLevel;
        public decimal MinimumArtifactLevel
        {
            get => _minimumArtifactLevel;
            set => SetField(ref _minimumArtifactLevel, value);
        }

        private int _sortByObtained;
        public int SortByObtained
        {
            get => _sortByObtained;
            set => SetField(ref _sortByObtained, value);
        }

        private int _numOfCharToScan;
        public int NumOfCharToScan
        {
            get => _numOfCharToScan;
            set => SetField(ref _numOfCharToScan, value);
        }

        #endregion

        #region Export Options

        private string _outputPath = string.Empty;
        public string OutputPath
        {
            get => _outputPath;
            set => SetField(ref _outputPath, value ?? string.Empty);
        }

        private bool _equipWeapons = true;
        public bool EquipWeapons
        {
            get => _equipWeapons;
            set => SetField(ref _equipWeapons, value);
        }

        private bool _equipArtifacts = true;
        public bool EquipArtifacts
        {
            get => _equipArtifacts;
            set => SetField(ref _equipArtifacts, value);
        }

        #endregion

        #region Keybindings

        private int _inventoryKey = 66; // VirtualKeyCode.B
        public int InventoryKey
        {
            get => _inventoryKey;
            set => SetField(ref _inventoryKey, value);
        }

        private int _characterKey = 67; // VirtualKeyCode.C
        public int CharacterKey
        {
            get => _characterKey;
            set => SetField(ref _characterKey, value);
        }

        private int _slot1Key = 1; // VirtualKeyCode.D1
        public int Slot1Key
        {
            get => _slot1Key;
            set => SetField(ref _slot1Key, value);
        }

        #endregion

        #region Character Names

        private string _travelerName = string.Empty;
        public string TravelerName
        {
            get => _travelerName;
            set => SetField(ref _travelerName, value ?? string.Empty);
        }

        private string _wandererName = "Wanderer";
        public string WandererName
        {
            get => _wandererName;
            set => SetField(ref _wandererName, value ?? "Wanderer");
        }

        private string _manequin1Name = "Manequin1";
        public string Manequin1Name
        {
            get => _manequin1Name;
            set => SetField(ref _manequin1Name, value ?? "Manequin1");
        }

        private string _manequin2Name = "Manequin2";
        public string Manequin2Name
        {
            get => _manequin2Name;
            set => SetField(ref _manequin2Name, value ?? "Manequin2");
        }

        #endregion

        #region Debug Options

        private bool _logScreenshots;
        public bool LogScreenshots
        {
            get => _logScreenshots;
            set => SetField(ref _logScreenshots, value);
        }

        #endregion

        #region Game Executables

        private List<string> _executables = new List<string> { "GenshinImpact", "YuanShen" };
        public List<string> Executables
        {
            get => _executables;
            set => SetField(ref _executables, value ?? new List<string> { "GenshinImpact", "YuanShen" });
        }

        #endregion

        #region Update Tracking

        private bool _upgradeNeeded = true;
        public bool UpgradeNeeded
        {
            get => _upgradeNeeded;
            set => SetField(ref _upgradeNeeded, value);
        }

        private DateTime _lastUpdateCheck = new DateTime(1000, 1, 1);
        public DateTime LastUpdateCheck
        {
            get => _lastUpdateCheck;
            set => SetField(ref _lastUpdateCheck, value);
        }

        private string _remoteVersion = "0.0.0.0";
        public string RemoteVersion
        {
            get => _remoteVersion;
            set => SetField(ref _remoteVersion, value ?? "0.0.0.0");
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
