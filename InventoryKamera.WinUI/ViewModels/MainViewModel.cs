using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InventoryKamera;
using InventoryKamera.Properties;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryKamera.WinUI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ILogger<MainViewModel> _logger;
        private readonly global::InventoryKamera.InventoryKamera _kamera;
        private CancellationTokenSource? _cancellationTokenSource;

        public MainViewModel(ILogger<MainViewModel> logger, global::InventoryKamera.InventoryKamera kamera)
        {
            _logger = logger;
            _kamera = kamera;
            Logs = new ObservableCollection<string>();

            // Initialize Settings
            var settings = SettingsService.Instance.Settings;

            ScanCharacters = settings.ScanCharacters;
            CharacterCount = settings.NumOfCharToScan;
            
            ScanArtifacts = settings.ScanArtifacts;
            MinArtifactLevel = (int)settings.MinimumArtifactLevel;
            EquipArtifacts = settings.EquipArtifacts;
            MinArtifactRarity = (int)settings.MinimumArtifactRarity;

            ScanWeapons = settings.ScanWeapons;
            MinWeaponLevel = (int)settings.MinimumWeaponLevel;
            EquipWeapons = settings.EquipWeapons;
            MinWeaponRarity = (int)settings.MinimumWeaponRarity;

            ScanMaterials = settings.ScanMaterials;
            ScanCharacterDevelopmentItems = settings.ScanCharDevItems;

            TravelerName = settings.TravelerName;
            WandererName = settings.WandererName;
            MannequinName1 = settings.Manequin1Name;
            MannequinName2 = settings.Manequin2Name;
        }

        // Configuration Properties
        [ObservableProperty]
        private bool _scanCharacters;

        [ObservableProperty]
        private int _characterCount;

        [ObservableProperty]
        private bool _scanArtifacts;

        [ObservableProperty]
        private int _minArtifactLevel;

        [ObservableProperty]
        private bool _equipArtifacts;

        [ObservableProperty]
        private int _minArtifactRarity;

        [ObservableProperty]
        private bool _scanWeapons;

        [ObservableProperty]
        private int _minWeaponLevel;

        [ObservableProperty]
        private bool _equipWeapons;

        [ObservableProperty]
        private int _minWeaponRarity;

        [ObservableProperty]
        private bool _scanMaterials;

        [ObservableProperty]
        private bool _scanCharacterDevelopmentItems;

        // Character Names
        [ObservableProperty]
        private string _travelerName;

        [ObservableProperty]
        private string _wandererName;

        [ObservableProperty]
        private string _mannequinName1;

        [ObservableProperty]
        private string _mannequinName2;

        // Status Properties
        [ObservableProperty]
        private string _statusMessage = "Waiting for Genshin Impact...";

        [ObservableProperty]
        private int _scanProgress = 0;

        [ObservableProperty]
        private bool _isScanning = false;

        // Log Collection
        public ObservableCollection<string> Logs { get; }

        private void SaveSettings()
        {
            var settings = SettingsService.Instance.Settings;
            settings.ScanCharacters = ScanCharacters;
            settings.NumOfCharToScan = CharacterCount;
            
            settings.ScanArtifacts = ScanArtifacts;
            settings.MinimumArtifactLevel = MinArtifactLevel;
            settings.EquipArtifacts = EquipArtifacts;
            settings.MinimumArtifactRarity = MinArtifactRarity;

            settings.ScanWeapons = ScanWeapons;
            settings.MinimumWeaponLevel = MinWeaponLevel;
            settings.EquipWeapons = EquipWeapons;
            settings.MinimumWeaponRarity = MinWeaponRarity;

            settings.ScanMaterials = ScanMaterials;
            settings.ScanCharDevItems = ScanCharacterDevelopmentItems;

            settings.TravelerName = TravelerName;
            settings.WandererName = WandererName;
            settings.Manequin1Name = MannequinName1;
            settings.Manequin2Name = MannequinName2;

            SettingsService.Instance.Save();
        }

        [RelayCommand]
        private async Task StartScanAsync()
        {
            if (IsScanning)
            {
                _cancellationTokenSource?.Cancel();
                StatusMessage = "Cancelling scan...";
                return;
            }
            
            SaveSettings();

            IsScanning = true;
            StatusMessage = "Scanning started...";
            Logs.Add($"[{System.DateTime.Now:HH:mm:ss}] Scan initiated.");
            
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Run(() => 
                {
                    _kamera.GatherData(_cancellationTokenSource.Token);
                });
                
                StatusMessage = "Scan complete!";
                Logs.Add($"[{System.DateTime.Now:HH:mm:ss}] Scan completed successfully.");
            }
            catch (OperationCanceledException)
            {
                StatusMessage = "Scan cancelled.";
                Logs.Add($"[{System.DateTime.Now:HH:mm:ss}] Scan was cancelled by user.");
            }
            catch (Exception ex)
            {
                StatusMessage = "Scan failed!";
                Logs.Add($"[{System.DateTime.Now:HH:mm:ss}] Error: {ex.Message}");
                _logger.LogError(ex, "Error during scan");
            }
            finally
            {
                IsScanning = false;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }
    }
}


