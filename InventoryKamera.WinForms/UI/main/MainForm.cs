using InventoryKamera.ui;
using NHotkey;
using NHotkey.WindowsForms;
using Octokit;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using WindowsInput.Native;
using InventoryKamera.Properties;
using Application = System.Windows.Forms.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InventoryKamera
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly DatabaseManager _databaseManager;

        private Thread _scannerThread;
        private InventoryKamera _data;

        private CancellationTokenSource _cancellationTokenSource;
        private AppSettings Settings => SettingsService.Instance.Settings;

        private int Delay;

        private bool running = false;

        public MainForm(ILogger<MainForm> logger, IServiceProvider serviceProvider, DatabaseManager databaseManager)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _databaseManager = databaseManager;

            InitializeComponent();

            Language_ComboBox.SelectedItem = "ENG";

            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
#if DEBUG
            version = Assembly.GetExecutingAssembly().GetName().Version.ToString(4);
#endif
            _logger.LogInformation("Inventory Kamera version {Version}", version);

            Text = $"Inventory Kamera V{version}";

            UserInterface.Init(
                GearPictureBox,
                ArtifactOutput_TextBox,
                CharacterName_PictureBox,
                CharacterLevel_PictureBox,
                [CharacterTalent1_PictureBox, CharacterTalent2_PictureBox, CharacterTalent3_PictureBox],
                CharacterOutput_TextBox,
                WeaponsScannedCount_Label,
                WeaponsMax_Labell,
                ArtifactsScanned_Label,
                ArtifactsMax_Label,
                CharactersScanned_Label,
                ProgramStatus_Label,
                ErrorLog_TextBox,
                Navigation_Image);
        }

        /// <summary>
        /// Parameterless constructor used only by the WinForms designer. The runtime
        /// always resolves <see cref="MainForm"/> through dependency injection.
        /// </summary>
        public MainForm() : this(
            Microsoft.Extensions.Logging.Abstractions.NullLogger<MainForm>.Instance,
            null,
            null)
        { }

        private double ScannerDelayValue(int value)
        {
            return value switch
            {
                0 => 0.5,
                1 => 1,
                2 => 1.5,
                _ => 1,
            };
        }

        private void Hotkey_Pressed(object sender, HotkeyEventArgs e)
        {
            _logger.LogInformation("Hotkey pressed");
            e.Handled = true;
            // Check if scanner is running
            if (_scannerThread != null && _scannerThread.IsAlive)
            {
                // Cancel the scanning operation
                _cancellationTokenSource?.Cancel();

                UserInterface.SetProgramStatus("Scan Stopped");

                Navigation.Reset();
            }
        }

        private void ResetUI()
        {
            Navigation.Reset();

            // Need to invoke method from the UI's handle, not the worker thread
            BeginInvoke((MethodInvoker)delegate { RemoveHotkey(); });
            _logger.LogInformation("Hotkey removed");
        }

        private void RemoveHotkey()
        {
            HotkeyManager.Current.Remove("Stop");
        }

        public static void UnexpectedError(string error)
        {
            UserInterface.AddError(error);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateKeyTextBoxes();

            Delay = ScannerDelay_TrackBar.Value;

            ProgramStatus_Label.Text = "";
            if (string.IsNullOrWhiteSpace(OutputPath_TextBox.Text))
            {
                OutputPath_TextBox.Text = Directory.GetCurrentDirectory() + @"\GenshinData";
            }

        }

        private void UpdateKeyTextBoxes()
        {
            Navigation.inventoryKey = (VirtualKeyCode)Settings.InventoryKey;
            Navigation.characterKey = (VirtualKeyCode)Settings.CharacterKey;
            Navigation.slotOneKey = (VirtualKeyCode)Settings.Slot1Key;

            inventoryToolStripTextBox.Text = new KeysConverter().ConvertToString((Keys)Navigation.inventoryKey);
            characterToolStripTextBox.Text = new KeysConverter().ConvertToString((Keys)Navigation.characterKey);
            slot1StripTextBox.Text = new KeysConverter().ConvertToString((Keys)Navigation.slotOneKey);

            // Make sure text boxes show key glyph and not "OEM..."
            if (inventoryToolStripTextBox.Text.ToUpper().Contains("OEM"))
            {
                inventoryToolStripTextBox.Text = KeyCodeToUnicode((Keys)Navigation.inventoryKey);
            }
            if (characterToolStripTextBox.Text.ToUpper().Contains("OEM"))
            {
                characterToolStripTextBox.Text = KeyCodeToUnicode((Keys)Navigation.characterKey);
            }
            if (slot1StripTextBox.Text.ToUpper().Contains("OEM"))
            {
                slot1StripTextBox.Text = KeyCodeToUnicode((Keys)Navigation.slotOneKey);
            }
        }

        private void ValidateCustomName(object sender, EventArgs e)
        {
            var textbox = sender as TextBox;
            var name = textbox.Text;

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (GenshinProcesor.Characters.ContainsKey(name.ConvertToGood().ToLower()))
                {
                    textbox.BackColor = Color.Yellow;
                }
                else
                {
                    textbox.BackColor = Color.White;
                }
            }
        }

        private void ValidateCustomName1(object sender, EventArgs e)
        {
            var textbox = sender as TextBox;
            var name = textbox.Text;

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (GenshinProcesor.Characters.ContainsKey(name.ConvertToGood().ToLower()))
                {
                    textbox.BackColor = Color.Yellow;
                }
                else
                {
                    textbox.BackColor = Color.White;
                }
            }
        }
        private void ValidateCustomName2(object sender, EventArgs e)
        {
            var textbox = sender as TextBox;
            var name = textbox.Text;

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (GenshinProcesor.Characters.ContainsKey(name.ConvertToGood().ToLower()))
                {
                    textbox.BackColor = Color.Yellow;
                }
                else
                {
                    textbox.BackColor = Color.White;
                }
            }
        }


        private void DisplayCustomNameTooltip(object sender, EventArgs e)
        {
            var textbox = sender as TextBox;
            
            if (textbox.BackColor == Color.Yellow)
            {
                var tooltip = new ToolTip();
                tooltip.Show($"{textbox.Text} already exists as a character's name.\n" +
                    $"This may affect equipping items to characters and is not fully supported yet.", textbox);
            }
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            var executables = Settings.Executables;
            bool gameRunning = false;
            if (executables != null)
            {
                foreach (var exe in executables)
                {
                    if (Process.GetProcessesByName(exe).Length > 0)
                    {
                        gameRunning = true;
                        break;
                    }
                }
            }

            if (!gameRunning)
            {
                MessageBox.Show("Genshin Impact process not found. Please ensure the game is running.", "Game Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GC.Collect();

            UserInterface.ResetAll();

            UserInterface.SetProgramStatus("Scanning");
            _logger.LogInformation("Starting scan");

            if (Directory.Exists(OutputPath_TextBox.Text) || Directory.CreateDirectory(OutputPath_TextBox.Text).Exists)
            {
                if (running)
                {
                    _logger.LogDebug("Already running");
                    return;
                }
                running = true;

                HotkeyManager.Current.AddOrReplace("Stop", Keys.Enter, Hotkey_Pressed);
                _logger.LogInformation("Hotkey registered");
                var gameVersion = _databaseManager.LocalVersion.ToString(2);
                var options =
                    $"\n\tGame Version Data:\t\t\t {gameVersion}\n" +
                    $"\tWeapons:\t\t\t\t {Settings.ScanWeapons}\n" +
                    $"\tArtifacts:\t\t\t\t {Settings.ScanArtifacts}\n" +
                    $"\tCharacters:\t\t\t\t {Settings.ScanCharacters}\n" +
                    $"\tDev Items:\t\t\t\t {Settings.ScanCharDevItems}\n" +
                    $"\tMaterials:\t\t\t\t {Settings.ScanMaterials}\n" +
                    $"\tMin Weapon Rarity:\t\t {Settings.MinimumWeaponRarity}\n" +
                    $"\tMin Weapon Level:\t\t {Settings.MinimumWeaponLevel}\n" +
                    $"\tEquip Weapons:\t\t\t {Settings.EquipWeapons}\n" +
                    $"\tMin Artifact Rarity:\t {Settings.MinimumArtifactRarity}\n" +
                    $"\tMin Artifact Level:\t\t {Settings.MinimumArtifactLevel}\n" +
                    $"\tEquip Artifacts:\t\t {Settings.EquipArtifacts}\n" +
                    $"\tDelay:\t\t\t\t\t {Settings.ScannerDelay}";

                _logger.LogInformation("Scan settings: {0}", options);

                // Create cancellation token for graceful shutdown
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = _cancellationTokenSource.Token;

                _scannerThread = new Thread(() =>
                {
                    try
                    {
                        // Get Screen Location and Size
                        Navigation.Initialize();

                        List<Size> sizes = new List<Size>
                        {
                            new Size(16,9),
                            new Size(8,5),
                        };

                        if (!sizes.Contains(Navigation.GetAspectRatio()))
                        {
                            throw new NotImplementedException($"{Navigation.GetSize().Width}x{Navigation.GetSize().Height} is an unsupported resolution.");
                        }

                        if (Navigation.GetSize() != Navigation.CaptureWindow().Size) throw new FormatException("Window size and screenshot size mismatch. Please make sure the game is not in a fullscreen mode.");

                        _data = _serviceProvider.GetRequiredService<InventoryKamera>();

                        _logger.LogInformation("Resolution: {0}x{1}", Navigation.GetSize().Width, Navigation.GetSize().Height);

                        // Add navigation delay
                        Navigation.SetDelay(ScannerDelayValue(Delay));

                        // The Data object of json object
                        _data.GatherData(cancellationToken);

                        // Check for cancellation before continuing
                        cancellationToken.ThrowIfCancellationRequested();

                        // Covert to GOOD
                        GOOD good = new GOOD(_data);
                        _logger.LogInformation("Data converted to GOOD");

                        // Make Json File
                        good.WriteToJSON(OutputPath_TextBox.Text);
                        _logger.LogInformation("Exported data");

                        Invoke((MethodInvoker)delegate
                        {
                            Clipboard.SetText(good.ToString());
                        });
                        _logger.LogInformation("Copied data to clipboard");

                        UserInterface.SetProgramStatus("Finished");
                        //OpenOptimizerDialog(good);
                    }
                    catch (OperationCanceledException)
                    {
                        // Graceful cancellation - workers may need cleanup
                        _data?.StopImageProcessorWorkers();
                        UserInterface.SetProgramStatus("Scan stopped");
                    }
                    catch (NotImplementedException ex)
                    {
                        UserInterface.AddError(ex.ToString());
                    }
                    catch (Exception ex)
                    {
                        // Workers can get stuck if an exception is raised
                        _data?.StopImageProcessorWorkers();
                        while (ex.InnerException != null) ex = ex.InnerException;
                        UserInterface.AddError(ex.ToString());
                        UserInterface.SetProgramStatus("Scan aborted", ok: false);
                    }
                    finally
                    {
                        ResetUI();
                        running = false;
                        ManualExportButton.Invoke((MethodInvoker)delegate
                        {
                            ManualExportButton.Enabled = _data.HasData;
                        });
                        MainForm_Activate();
                    }
                })
                {
                    IsBackground = true
                };
                _scannerThread.Start();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(OutputPath_TextBox.Text))
                    UserInterface.AddError("Please set an output directory");
                else
                    UserInterface.AddError($"{OutputPath_TextBox.Text} is not a valid directory");
            }
        }

        private void OpenOptimizerDialog(GOOD data, bool skip = false)
        {
            if (!skip)
            {
                var message = "Scan complete! Would you like to upload the database to Genshin Optimizer?";
                var result = MessageBox.Show(message, "Scan Complete", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;
            }
            var t = new Thread(() => Clipboard.SetText(data.ToString()));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            MessageBox.Show("Content copied to your clipboard! Paste the content into the textbox when prompted.", "Data Copied", MessageBoxButtons.OK);
            Process.Start(new ProcessStartInfo("https://frzyc.github.io/genshin-optimizer/#/setting") { UseShellExecute = true });
        }

        private void Github_Label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/taiwenlee/Inventory_Kamera/") { UseShellExecute = true });
        }

        private void Releases_Label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/taiwenlee/Inventory_Kamera/releases") { UseShellExecute = true });
        }

        private void IssuesPage_Label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/taiwenlee/Inventory_Kamera/issues") { UseShellExecute = true });
        }

        private void FileSelectButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new()
            {
                InitialDirectory = !Directory.Exists(OutputPath_TextBox.Text) ? Directory.GetCurrentDirectory() : OutputPath_TextBox.Text,
            };

            if (d.ShowDialog() == DialogResult.OK)
            {
                OutputPath_TextBox.Text = d.SelectedPath;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            RemoveHotkey();
            _cancellationTokenSource?.Dispose();
            NLog.LogManager.Shutdown();
        }

        private void SaveSettings()
        {
            SettingsService.Instance.Save();
        }

        private void ScannerDelay_TrackBar_ValueChanged(object sender, EventArgs e)
        {
            Delay = ((TrackBar)sender).Value;
        }

        private void Exit_MenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OptionsMenuItem_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;

            // Virtual keys for 0-9, A-Z
            bool vk = e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.Z;
            // Numpad keys and function keys (internally accepts up to F24)
            bool np = e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.F24;
            // OEM keys (Keys that vary depending on keyboard layout)
            bool oem = e.KeyCode >= Keys.Oem1 && e.KeyCode <= Keys.Oem7;
            // Arrow keys, spacebar, INS, DEL, HOME, END, PAGEUP, PAGEDOWN
            bool misc = e.KeyCode == Keys.Space || (e.KeyCode >= Keys.Left && e.KeyCode <= Keys.Down) || (e.KeyCode >= Keys.Prior && e.KeyCode <= Keys.Home) || e.KeyCode == Keys.Insert || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back;

            // Validate that key is an acceptable Genshin keybind.
            if (!vk && !np && !oem && !misc)
            {
                _logger.LogDebug("Invalid {key} key pressed", e.KeyCode);
                return;
            }
            ToolStripTextBox s = (ToolStripTextBox)sender;

            // Needed to differentiate between NUMPAD numbers and numbers at top of keyboard
            s.Text = np || e.KeyCode == Keys.Back ? new KeysConverter().ConvertToString(e.KeyCode) : KeyCodeToUnicode(e.KeyData);

            // Spacebar or upper navigation keys (INSERT-PAGEDOWN keys) make textbox empty
            if (string.IsNullOrWhiteSpace(s.Text) || string.IsNullOrEmpty(s.Text))
            {
                s.Text = new KeysConverter().ConvertToString(e.KeyCode);
            }


            switch (s.Tag)
            {
                case "InventoryKey":
                    Navigation.inventoryKey = (VirtualKeyCode)e.KeyCode;
                    _logger.LogDebug("Inv key set to: {key}", Navigation.inventoryKey);
                    Settings.InventoryKey = e.KeyValue;
                    break;

                case "CharacterKey":
                    Navigation.characterKey = (VirtualKeyCode)e.KeyCode;
                    _logger.LogDebug("Char key set to: {key}", Navigation.characterKey);
                    Settings.CharacterKey = e.KeyValue;
                    break;

                case "slot1Key":
                    Navigation.slotOneKey = (VirtualKeyCode)e.KeyCode;
                    _logger.LogDebug("Slot 1 key set to: {key}", Navigation.slotOneKey);
                    Settings.Slot1Key = e.KeyValue;
                    break;

                default:
                    break;
            }
        }

        private async void DatabaseUpdateMenuItem_Click(object sender, EventArgs e)
        {
            var status = await Task.Run(() => _databaseManager.UpdateGameData());
            switch (status)
            {
                case UpdateStatus.Fail:
                    MessageBox.Show("Unable to update game data. Please check the log for more details", "Update failed", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Stop);
                    break;
                case UpdateStatus.Success:
                    MessageBox.Show($"Update for game version {_databaseManager.LocalVersion.ToString(2)} successful.", "Update status", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                    _logger.LogInformation("Updated game date to {0}", _databaseManager.LocalVersion.ToString(2));
                    break;
                case UpdateStatus.Skipped:
                    if (MessageBox.Show($"No update necessary! You are already using the latest game data ({_databaseManager.LocalVersion.ToString(2)})." +
                        $" Would you like to force an update?",
                        "Already Up to Date",
                        buttons: MessageBoxButtons.YesNo,
                        icon: MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        status = await Task.Run(() => _databaseManager.UpdateGameData(force: true));
                        switch (status)
                        {
                            case UpdateStatus.Fail:
                                MessageBox.Show("Unable to update game data. Please check the log for more details", "Update failed", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Stop);
                                break;
                            default:
                                MessageBox.Show($"Update for game version {_databaseManager.LocalVersion.ToString(2)} successful.", "Update success", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                                _logger.LogInformation("Successfully updated game data to {0}", _databaseManager.LocalVersion.ToString(2));
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #region Unicode Helper Functions

        // Needed to display OEM keys as glyphs from keyboard. Should work for other languages
        // and keyboard layouts but only tested with QWERTY layout.
        private string KeyCodeToUnicode(Keys key)
        {
            byte[] keyboardState = new byte[255];
            bool keyboardStateStatus = GetKeyboardState(keyboardState);

            if (!keyboardStateStatus)
            {
                return "";
            }
            uint virtualKeyCode = (uint)key;
            uint scanCode = MapVirtualKey(virtualKeyCode, 0);
            IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);

            StringBuilder result = new StringBuilder();
            ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, 5, 0, inputLocaleIdentifier);

            return result.ToString();
        }

        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        #endregion Unicode Helper Functions

        private void ExportFolderMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(OutputPath_TextBox.Text) || Directory.CreateDirectory(OutputPath_TextBox.Text).Exists)
            {
                Process.Start(new ProcessStartInfo(OutputPath_TextBox.Text) { UseShellExecute = true });
            }
            else
            {
                Process.Start(new ProcessStartInfo("explorer.exe") { UseShellExecute = true });
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
#if !DEBUG
            if (Settings.CheckForUpdates)
            {
                CheckForKameraUpdates();
            }
#endif
            if (Settings.CheckForUpdates)
            {
                CheckForGenshinUpdates();
            }
        }

        private async void CheckForKameraUpdates()
        {
            var client = new GitHubClient(new ProductHeaderValue("Inventory_Kamera"));
            try
            {
                var releases = await client.Repository.Release.GetAll("Andrewthe13th", "Inventory_Kamera");
                var latest = releases.First();

                Version latestVersion = new Version(Regex.Replace(latest.TagName, "[a-zA-Z]", string.Empty));
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                if (currentVersion.CompareTo(latestVersion) < 0)
                {
                    var message = $"A new version of Inventory Kamera is available.\n\n" +
                        $"Current Version: {currentVersion}\nLatest Version: {latestVersion}\n\n" +
                        $"Would you like to download the update?";
                    var result = MessageBox.Show(message, "Inventory Kamera Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(latest.HtmlUrl) { UseShellExecute = true });
                    }
                }
            }
            catch (RateLimitExceededException) { _logger.LogWarning("Rate limit exceeded checking for Kamera Update!!!! This warning should be resolved in an hour."); }
        }

        private void CheckForGenshinUpdates()
        {
            try
            {
                var updatesAvailable = _databaseManager.UpdateAvailable();
                if (updatesAvailable)
                {
                    var message = "A new version for Genshin Impact has been found. Would you like to update Kamera's lookup tables? (Recommended)";
                    var result = MessageBox.Show(message, "Game Version Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        switch (_databaseManager.UpdateGameData())
                        {
                            case UpdateStatus.Fail:
                                MessageBox.Show("Unable to update game data. Please check the log for more details", "Update failed", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Stop);
                                break;
                            case UpdateStatus.Success:
                                MessageBox.Show($"Update for game version {_databaseManager.LocalVersion.ToString(2) } successful.", "Update status", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                                _logger.LogInformation("Updated game data to {0}", _databaseManager.LocalVersion.ToString(2));
                                break;
                            default:
                                break;
                        }
                    }
                    else if (result == DialogResult.No)
                    {
                        MessageBox.Show("Update skipped. Please know that skipping this update will likely result in incorrect scans.\n" +
                            "\nYou may check for updates again on restarting this application or by using the update manager found" +
                            " under 'options'", "Update declined", MessageBoxButtons.OK);
                    }
                }
                else
                    _logger.LogInformation("Current game data is up to date with data for {0}", _databaseManager.LocalVersion.ToString(2));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not check for list updates");
                MessageBox.Show("Could not check for updates. Consider trying again in an hour or so.", "Game Version Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Settings.LastUpdateCheck = DateTime.Now;
        }

        private void Export_Button_Click(object sender, EventArgs e)
        {
            OpenOptimizerDialog(new GOOD(_data), true);
        }

        private void MainForm_Activate()
        {
            BeginInvoke((MethodInvoker)delegate { Activate(); });
        }

        private void UpdateExecutablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ExecutablesForm().Show();
        }

        private void ErrorLog_Label_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo(@"logging") { UseShellExecute = true });
        }
    }
}