namespace InventoryKamera;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        // Use the singleton settings instance so UI data bindings update the same
        // object that the service layer reads from.
        global::InventoryKamera.Properties.AppSettings appSettings1 = global::InventoryKamera.Properties.SettingsService.Instance.Settings;
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

        // Instantiate all controls
        menuStrip1 = new MenuStrip();
        fileToolStripMenuItem = new ToolStripMenuItem();
        Quit_MenuItem = new ToolStripMenuItem();
        keysToolStripMenuItem = new ToolStripMenuItem();
        inventoryToolStripMenuItem = new ToolStripMenuItem();
        inventoryToolStripTextBox = new ToolStripTextBox();
        characterScreenToolStripMenuItem = new ToolStripMenuItem();
        characterToolStripTextBox = new ToolStripTextBox();
        characterSlot1KeyToolStripMenuItem = new ToolStripMenuItem();
        slot1StripTextBox = new ToolStripTextBox();
        DatabaseUpdateMenuItem = new ToolStripMenuItem();
        toolStripMenuItem1 = new ToolStripMenuItem();
        updateExecutablesToolStripMenuItem = new ToolStripMenuItem();

        grpScanItems = new GroupBox();
        Language_Label = new Label();
        Language_ComboBox = new ComboBox();
        Weapons_CheckBox = new CheckBox();
        Artifacts_Checkbox = new CheckBox();
        Characters_CheckBox = new CheckBox();
        CharDevItems_CheckBox = new CheckBox();
        Materials_CheckBox = new CheckBox();
        LogScreenshotsCheckBox = new CheckBox();

        grpScanSpeed = new GroupBox();
        ScannerDelay_TrackBar = new TrackBar();
        ScannerDelay_Label = new Label();
        ScanItemsList_Label = new Label();
        FastScannerDelay_Label = new Label();
        MidScannerDelay_Label = new Label();
        SlowScannerDelay_Label = new Label();

        grpFilters = new GroupBox();
        label2 = new Label();
        WeaponRarityControl = new NumericUpDown();
        label3 = new Label();
        ArtifactRarityControl = new NumericUpDown();
        MinimumWeaponLevelLabel = new Label();
        MinimumWeaponLevelControl = new NumericUpDown();
        MinimumArtifactLevelLabel = new Label();
        numericUpDown1 = new NumericUpDown();
        EquipWeaponsCheckBox = new CheckBox();
        EquipArtifactsCheckBox = new CheckBox();
        label5 = new Label();
        NumOfCharToScanControl = new NumericUpDown();
        SortByObtained = new Label();
        SortByObtainedControl = new NumericUpDown();

        grpNames = new GroupBox();
        label1 = new Label();
        travelerNameTextBox = new TextBox();
        label4 = new Label();
        wandererNameTextBox = new TextBox();
        label6 = new Label();
        textBox1 = new TextBox();
        label7 = new Label();
        textBox2 = new TextBox();

        Github_Label = new LinkLabel();
        Releases_Label = new LinkLabel();
        Navigation_Label = new Label();
        Navigation_Image = new PictureBox();

        StartScan_Button = new Button();
        ScannerCancelInstructions_Label = new Label();
        ProgramStatus_Label = new Label();

        WeaponsScanned_Label = new Label();
        WeaponsScannedCount_Label = new Label();
        WeaponsScannedSlash_Label = new Label();
        WeaponsMax_Labell = new Label();
        Artifacts_Label = new Label();
        ArtifactsScanned_Label = new Label();
        ArtifactsScannedSlash_Label = new Label();
        ArtifactsMax_Label = new Label();
        Characters_Label = new Label();
        CharactersScanned_Label = new Label();

        ManualExportButton = new Button();
        button1 = new Button();

        FileLocation_Label = new Label();
        ScannerOutput_Panel = new Panel();
        FileSelectButton = new Button();
        OutputPath_TextBox = new TextBox();
        WeaponArtifact_Label = new Label();
        WeaponArtifactOutput_TextBox_Label = new Label();
        GearPictureBox = new PictureBox();
        ArtifactOutput_TextBox = new TextBox();
        Character_Label = new Label();
        CharacterOutput_TextBox_Label = new Label();
        CharacterName_PictureBox = new PictureBox();
        CharacterLevel_PictureBox = new PictureBox();
        CharacterTalent1_PictureBox = new PictureBox();
        CharacterTalent2_PictureBox = new PictureBox();
        CharacterTalent3_PictureBox = new PictureBox();
        CharacterOutput_TextBox = new TextBox();
        ErrorLog_Label = new Label();
        ErrorReport_Label = new Label();
        IssuesPage_Label = new LinkLabel();
        ErrorLog_TextBox = new RichTextBox();
        LogLevel_Label = new Label();
        LogLevel_ComboBox = new ComboBox();
        ClearLog_Button = new Button();

        folderBrowserDialog1 = new FolderBrowserDialog();
        equipWeaponToolTip = new ToolTip(components);
        equipArtifactsToolTip = new ToolTip(components);
        screenshotsToolTip = new ToolTip(components);
        ZeroMeansAllTooltips = new ToolTip(components);

        // BeginInit
        ((System.ComponentModel.ISupportInitialize)ScannerDelay_TrackBar).BeginInit();
        ((System.ComponentModel.ISupportInitialize)WeaponRarityControl).BeginInit();
        ((System.ComponentModel.ISupportInitialize)ArtifactRarityControl).BeginInit();
        ((System.ComponentModel.ISupportInitialize)MinimumWeaponLevelControl).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)NumOfCharToScanControl).BeginInit();
        ((System.ComponentModel.ISupportInitialize)SortByObtainedControl).BeginInit();
        ((System.ComponentModel.ISupportInitialize)Navigation_Image).BeginInit();
        ((System.ComponentModel.ISupportInitialize)GearPictureBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)CharacterName_PictureBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)CharacterLevel_PictureBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)CharacterTalent1_PictureBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)CharacterTalent2_PictureBox).BeginInit();
        ((System.ComponentModel.ISupportInitialize)CharacterTalent3_PictureBox).BeginInit();
        grpScanItems.SuspendLayout();
        grpScanSpeed.SuspendLayout();
        grpFilters.SuspendLayout();
        grpNames.SuspendLayout();
        ScannerOutput_Panel.SuspendLayout();
        menuStrip1.SuspendLayout();
        SuspendLayout();

        // =============================================
        // MENU STRIP
        // =============================================
        menuStrip1.BackColor = Color.FromArgb(248, 249, 250);
        menuStrip1.ImageScalingSize = new Size(20, 20);
        menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, keysToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Padding = new Padding(5, 2, 0, 2);
        menuStrip1.Size = new Size(870, 24);
        menuStrip1.TabIndex = 0;
        //
        // fileToolStripMenuItem
        //
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { Quit_MenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new Size(37, 20);
        fileToolStripMenuItem.Text = "File";
        //
        // Quit_MenuItem
        //
        Quit_MenuItem.Name = "Quit_MenuItem";
        Quit_MenuItem.Size = new Size(97, 22);
        Quit_MenuItem.Text = "Quit";
        Quit_MenuItem.Click += Exit_MenuItem_Click;
        //
        // keysToolStripMenuItem
        //
        keysToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { inventoryToolStripMenuItem, characterScreenToolStripMenuItem, characterSlot1KeyToolStripMenuItem, DatabaseUpdateMenuItem, toolStripMenuItem1, updateExecutablesToolStripMenuItem });
        keysToolStripMenuItem.Name = "keysToolStripMenuItem";
        keysToolStripMenuItem.Size = new Size(61, 20);
        keysToolStripMenuItem.Text = "Options";
        //
        // inventoryToolStripMenuItem
        //
        inventoryToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { inventoryToolStripTextBox });
        inventoryToolStripMenuItem.Name = "inventoryToolStripMenuItem";
        inventoryToolStripMenuItem.Size = new Size(191, 22);
        inventoryToolStripMenuItem.Text = "Inventory Key";
        //
        // inventoryToolStripTextBox
        //
        inventoryToolStripTextBox.CharacterCasing = CharacterCasing.Upper;
        inventoryToolStripTextBox.MaxLength = 2;
        inventoryToolStripTextBox.Name = "inventoryToolStripTextBox";
        inventoryToolStripTextBox.Size = new Size(90, 23);
        inventoryToolStripTextBox.Tag = "InventoryKey";
        inventoryToolStripTextBox.Text = "B";
        inventoryToolStripTextBox.ToolTipText = "Key to open inventory";
        inventoryToolStripTextBox.KeyDown += OptionsMenuItem_KeyDown;
        //
        // characterScreenToolStripMenuItem
        //
        characterScreenToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { characterToolStripTextBox });
        characterScreenToolStripMenuItem.Name = "characterScreenToolStripMenuItem";
        characterScreenToolStripMenuItem.Size = new Size(191, 22);
        characterScreenToolStripMenuItem.Text = "Character Screen Key";
        //
        // characterToolStripTextBox
        //
        characterToolStripTextBox.CharacterCasing = CharacterCasing.Upper;
        characterToolStripTextBox.MaxLength = 2;
        characterToolStripTextBox.Name = "characterToolStripTextBox";
        characterToolStripTextBox.Size = new Size(90, 23);
        characterToolStripTextBox.Tag = "CharacterKey";
        characterToolStripTextBox.Text = "C";
        characterToolStripTextBox.ToolTipText = "Key to open character screen";
        characterToolStripTextBox.KeyDown += OptionsMenuItem_KeyDown;
        //
        // characterSlot1KeyToolStripMenuItem
        //
        characterSlot1KeyToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { slot1StripTextBox });
        characterSlot1KeyToolStripMenuItem.Name = "characterSlot1KeyToolStripMenuItem";
        characterSlot1KeyToolStripMenuItem.Size = new Size(191, 22);
        characterSlot1KeyToolStripMenuItem.Text = "Character Slot 1 Key";
        //
        // slot1StripTextBox
        //
        slot1StripTextBox.CharacterCasing = CharacterCasing.Upper;
        slot1StripTextBox.MaxLength = 10;
        slot1StripTextBox.Name = "slot1StripTextBox";
        slot1StripTextBox.Size = new Size(90, 23);
        slot1StripTextBox.Tag = "slot1Key";
        slot1StripTextBox.Text = "1";
        slot1StripTextBox.ToolTipText = "Key to Select First Character";
        slot1StripTextBox.KeyDown += OptionsMenuItem_KeyDown;
        //
        // DatabaseUpdateMenuItem
        //
        DatabaseUpdateMenuItem.Name = "DatabaseUpdateMenuItem";
        DatabaseUpdateMenuItem.Size = new Size(191, 22);
        DatabaseUpdateMenuItem.Text = "Update Lookup Tables";
        DatabaseUpdateMenuItem.Click += DatabaseUpdateMenuItem_Click;
        //
        // toolStripMenuItem1
        //
        toolStripMenuItem1.Name = "toolStripMenuItem1";
        toolStripMenuItem1.Size = new Size(191, 22);
        toolStripMenuItem1.Text = "Open Export Folder";
        toolStripMenuItem1.Click += ExportFolderMenuItem_Click;
        //
        // updateExecutablesToolStripMenuItem
        //
        updateExecutablesToolStripMenuItem.Name = "updateExecutablesToolStripMenuItem";
        updateExecutablesToolStripMenuItem.Size = new Size(191, 22);
        updateExecutablesToolStripMenuItem.Text = "Update Executables";
        updateExecutablesToolStripMenuItem.Click += UpdateExecutablesToolStripMenuItem_Click;

        // =============================================
        // GROUP: SCAN ITEMS (top-left)
        // =============================================
        grpScanItems.Controls.Add(Language_Label);
        grpScanItems.Controls.Add(Language_ComboBox);
        grpScanItems.Controls.Add(Weapons_CheckBox);
        grpScanItems.Controls.Add(Artifacts_Checkbox);
        grpScanItems.Controls.Add(Characters_CheckBox);
        grpScanItems.Controls.Add(CharDevItems_CheckBox);
        grpScanItems.Controls.Add(Materials_CheckBox);
        grpScanItems.Controls.Add(LogScreenshotsCheckBox);
        grpScanItems.ForeColor = Color.FromArgb(44, 62, 80);
        grpScanItems.Location = new Point(12, 30);
        grpScanItems.Name = "grpScanItems";
        grpScanItems.Padding = new Padding(8);
        grpScanItems.Size = new Size(176, 198);
        grpScanItems.TabIndex = 200;
        grpScanItems.TabStop = false;
        grpScanItems.Text = "Scan Items";
        //
        // Language_Label
        //
        Language_Label.AutoSize = true;
        Language_Label.ForeColor = Color.FromArgb(44, 62, 80);
        Language_Label.Location = new Point(10, 22);
        Language_Label.Name = "Language_Label";
        Language_Label.Size = new Size(62, 15);
        Language_Label.TabIndex = 0;
        Language_Label.Text = "Language:";
        //
        // Language_ComboBox
        //
        Language_ComboBox.BackColor = Color.White;
        Language_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        Language_ComboBox.Enabled = false;
        Language_ComboBox.FormattingEnabled = true;
        Language_ComboBox.Items.AddRange(new object[] { "ENG" });
        Language_ComboBox.Location = new Point(80, 19);
        Language_ComboBox.Name = "Language_ComboBox";
        Language_ComboBox.Size = new Size(82, 23);
        Language_ComboBox.Sorted = true;
        Language_ComboBox.TabIndex = 1;
        //
        // Weapons_CheckBox
        //
        Weapons_CheckBox.AutoSize = true;
        Weapons_CheckBox.Checked = true;
        Weapons_CheckBox.CheckState = CheckState.Checked;
        Weapons_CheckBox.DataBindings.Add(new Binding("Checked", appSettings1, "ScanWeapons", true, DataSourceUpdateMode.OnPropertyChanged));
        Weapons_CheckBox.ForeColor = Color.FromArgb(44, 62, 80);
        Weapons_CheckBox.Location = new Point(10, 48);
        Weapons_CheckBox.Name = "Weapons_CheckBox";
        Weapons_CheckBox.Size = new Size(76, 19);
        Weapons_CheckBox.TabIndex = 2;
        Weapons_CheckBox.Text = "Weapons";
        Weapons_CheckBox.UseVisualStyleBackColor = true;
        //
        // Artifacts_Checkbox
        //
        Artifacts_Checkbox.AutoSize = true;
        Artifacts_Checkbox.Checked = true;
        Artifacts_Checkbox.CheckState = CheckState.Checked;
        Artifacts_Checkbox.DataBindings.Add(new Binding("Checked", appSettings1, "ScanArtifacts", true, DataSourceUpdateMode.OnPropertyChanged));
        Artifacts_Checkbox.ForeColor = Color.FromArgb(44, 62, 80);
        Artifacts_Checkbox.Location = new Point(10, 70);
        Artifacts_Checkbox.Name = "Artifacts_Checkbox";
        Artifacts_Checkbox.Size = new Size(71, 19);
        Artifacts_Checkbox.TabIndex = 3;
        Artifacts_Checkbox.Text = "Artifacts";
        Artifacts_Checkbox.UseVisualStyleBackColor = true;
        //
        // Characters_CheckBox
        //
        Characters_CheckBox.AutoSize = true;
        Characters_CheckBox.Checked = true;
        Characters_CheckBox.CheckState = CheckState.Checked;
        Characters_CheckBox.DataBindings.Add(new Binding("Checked", appSettings1, "ScanCharacters", true, DataSourceUpdateMode.OnPropertyChanged));
        Characters_CheckBox.ForeColor = Color.FromArgb(44, 62, 80);
        Characters_CheckBox.Location = new Point(10, 92);
        Characters_CheckBox.Name = "Characters_CheckBox";
        Characters_CheckBox.Size = new Size(82, 19);
        Characters_CheckBox.TabIndex = 4;
        Characters_CheckBox.Text = "Characters";
        Characters_CheckBox.UseVisualStyleBackColor = true;
        //
        // CharDevItems_CheckBox
        //
        CharDevItems_CheckBox.AutoSize = true;
        CharDevItems_CheckBox.Checked = true;
        CharDevItems_CheckBox.CheckState = CheckState.Checked;
        CharDevItems_CheckBox.DataBindings.Add(new Binding("Checked", appSettings1, "ScanCharDevItems", true, DataSourceUpdateMode.OnPropertyChanged));
        CharDevItems_CheckBox.ForeColor = Color.FromArgb(44, 62, 80);
        CharDevItems_CheckBox.Location = new Point(10, 114);
        CharDevItems_CheckBox.Name = "CharDevItems_CheckBox";
        CharDevItems_CheckBox.Size = new Size(133, 19);
        CharDevItems_CheckBox.TabIndex = 5;
        CharDevItems_CheckBox.Text = "Char Dev Items";
        CharDevItems_CheckBox.UseVisualStyleBackColor = true;
        //
        // Materials_CheckBox
        //
        Materials_CheckBox.AutoSize = true;
        Materials_CheckBox.Checked = true;
        Materials_CheckBox.CheckState = CheckState.Checked;
        Materials_CheckBox.DataBindings.Add(new Binding("Checked", appSettings1, "ScanMaterials", true, DataSourceUpdateMode.OnPropertyChanged));
        Materials_CheckBox.ForeColor = Color.FromArgb(44, 62, 80);
        Materials_CheckBox.Location = new Point(10, 136);
        Materials_CheckBox.Name = "Materials_CheckBox";
        Materials_CheckBox.Size = new Size(74, 19);
        Materials_CheckBox.TabIndex = 6;
        Materials_CheckBox.Text = "Materials";
        Materials_CheckBox.UseVisualStyleBackColor = true;
        //
        // LogScreenshotsCheckBox
        //
        LogScreenshotsCheckBox.AutoSize = true;
        LogScreenshotsCheckBox.DataBindings.Add(new Binding("Checked", appSettings1, "LogScreenshots", true, DataSourceUpdateMode.OnPropertyChanged));
        LogScreenshotsCheckBox.ForeColor = Color.FromArgb(100, 100, 100);
        LogScreenshotsCheckBox.Location = new Point(10, 164);
        LogScreenshotsCheckBox.Name = "LogScreenshotsCheckBox";
        LogScreenshotsCheckBox.Size = new Size(129, 19);
        LogScreenshotsCheckBox.TabIndex = 7;
        LogScreenshotsCheckBox.Text = "Log Screenshots";
        screenshotsToolTip.SetToolTip(LogScreenshotsCheckBox, "Debug tool. If enabled, all screenshots will be logged to local files.\r\nAll screenshots will be cleared when a new scan is started.\r\n");
        LogScreenshotsCheckBox.UseVisualStyleBackColor = true;

        // =============================================
        // GROUP: SCANNER SPEED (below scan items)
        // =============================================
        grpScanSpeed.Controls.Add(ScannerDelay_TrackBar);
        grpScanSpeed.Controls.Add(FastScannerDelay_Label);
        grpScanSpeed.Controls.Add(MidScannerDelay_Label);
        grpScanSpeed.Controls.Add(SlowScannerDelay_Label);
        grpScanSpeed.ForeColor = Color.FromArgb(44, 62, 80);
        grpScanSpeed.Location = new Point(12, 234);
        grpScanSpeed.Name = "grpScanSpeed";
        grpScanSpeed.Size = new Size(176, 78);
        grpScanSpeed.TabIndex = 201;
        grpScanSpeed.TabStop = false;
        grpScanSpeed.Text = "Scanner Speed";
        //
        // ScannerDelay_TrackBar
        //
        ScannerDelay_TrackBar.DataBindings.Add(new Binding("Value", appSettings1, "ScannerDelay", true, DataSourceUpdateMode.OnPropertyChanged, null, "N0"));
        ScannerDelay_TrackBar.Location = new Point(8, 18);
        ScannerDelay_TrackBar.Maximum = 2;
        ScannerDelay_TrackBar.Name = "ScannerDelay_TrackBar";
        ScannerDelay_TrackBar.Size = new Size(156, 45);
        ScannerDelay_TrackBar.TabIndex = 0;
        ScannerDelay_TrackBar.ValueChanged += ScannerDelay_TrackBar_ValueChanged;
        //
        // FastScannerDelay_Label
        //
        FastScannerDelay_Label.AutoSize = true;
        FastScannerDelay_Label.Font = new Font("Segoe UI", 7F);
        FastScannerDelay_Label.ForeColor = Color.FromArgb(100, 100, 100);
        FastScannerDelay_Label.Location = new Point(10, 58);
        FastScannerDelay_Label.Name = "FastScannerDelay_Label";
        FastScannerDelay_Label.Size = new Size(26, 12);
        FastScannerDelay_Label.TabIndex = 1;
        FastScannerDelay_Label.Text = "Fast";
        //
        // MidScannerDelay_Label
        //
        MidScannerDelay_Label.AutoSize = true;
        MidScannerDelay_Label.Font = new Font("Segoe UI", 7F);
        MidScannerDelay_Label.ForeColor = Color.FromArgb(100, 100, 100);
        MidScannerDelay_Label.Location = new Point(66, 58);
        MidScannerDelay_Label.Name = "MidScannerDelay_Label";
        MidScannerDelay_Label.Size = new Size(23, 12);
        MidScannerDelay_Label.TabIndex = 2;
        MidScannerDelay_Label.Text = "Mid";
        //
        // SlowScannerDelay_Label
        //
        SlowScannerDelay_Label.AutoSize = true;
        SlowScannerDelay_Label.Font = new Font("Segoe UI", 7F);
        SlowScannerDelay_Label.ForeColor = Color.FromArgb(100, 100, 100);
        SlowScannerDelay_Label.Location = new Point(130, 58);
        SlowScannerDelay_Label.Name = "SlowScannerDelay_Label";
        SlowScannerDelay_Label.Size = new Size(29, 12);
        SlowScannerDelay_Label.TabIndex = 3;
        SlowScannerDelay_Label.Text = "Slow";
        //
        // ScannerDelay_Label (hidden, replaced by GroupBox title)
        //
        ScannerDelay_Label.AutoSize = true;
        ScannerDelay_Label.Location = new Point(0, 0);
        ScannerDelay_Label.Name = "ScannerDelay_Label";
        ScannerDelay_Label.Size = new Size(0, 15);
        ScannerDelay_Label.TabIndex = 999;
        ScannerDelay_Label.Visible = false;
        //
        // ScanItemsList_Label (hidden, replaced by GroupBox title)
        //
        ScanItemsList_Label.AutoSize = true;
        ScanItemsList_Label.Location = new Point(0, 0);
        ScanItemsList_Label.Name = "ScanItemsList_Label";
        ScanItemsList_Label.Size = new Size(0, 15);
        ScanItemsList_Label.TabIndex = 998;
        ScanItemsList_Label.Visible = false;

        // =============================================
        // START SCAN BUTTON + CANCEL LABEL
        // =============================================
        //
        // StartScan_Button
        //
        StartScan_Button.BackColor = Color.FromArgb(46, 204, 113);
        StartScan_Button.Cursor = Cursors.Hand;
        StartScan_Button.FlatAppearance.BorderSize = 0;
        StartScan_Button.FlatStyle = FlatStyle.Flat;
        StartScan_Button.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        StartScan_Button.ForeColor = Color.White;
        StartScan_Button.Location = new Point(12, 320);
        StartScan_Button.Name = "StartScan_Button";
        StartScan_Button.Size = new Size(176, 38);
        StartScan_Button.TabIndex = 1;
        StartScan_Button.Text = "▶  Scan Genshin";
        StartScan_Button.UseVisualStyleBackColor = false;
        StartScan_Button.Click += StartButton_Clicked;
        //
        // ScannerCancelInstructions_Label
        //
        ScannerCancelInstructions_Label.AutoSize = true;
        ScannerCancelInstructions_Label.Font = new Font("Segoe UI", 7.5F);
        ScannerCancelInstructions_Label.ForeColor = Color.FromArgb(140, 140, 140);
        ScannerCancelInstructions_Label.Location = new Point(14, 362);
        ScannerCancelInstructions_Label.Name = "ScannerCancelInstructions_Label";
        ScannerCancelInstructions_Label.Size = new Size(123, 13);
        ScannerCancelInstructions_Label.TabIndex = 2;
        ScannerCancelInstructions_Label.Text = "Press 'ENTER' to cancel";

        // =============================================
        // STATUS + COUNTS (left column, below start)
        // =============================================
        //
        // ProgramStatus_Label
        //
        ProgramStatus_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        ProgramStatus_Label.AutoSize = true;
        ProgramStatus_Label.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
        ProgramStatus_Label.ForeColor = Color.FromArgb(46, 204, 113);
        ProgramStatus_Label.Location = new Point(8, 385);
        ProgramStatus_Label.Name = "ProgramStatus_Label";
        ProgramStatus_Label.Size = new Size(150, 25);
        ProgramStatus_Label.TabIndex = 3;
        ProgramStatus_Label.Text = "Scanning Status";
        //
        // WeaponsScanned_Label
        //
        WeaponsScanned_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        WeaponsScanned_Label.AutoSize = true;
        WeaponsScanned_Label.ForeColor = Color.FromArgb(80, 80, 80);
        WeaponsScanned_Label.Location = new Point(12, 418);
        WeaponsScanned_Label.Name = "WeaponsScanned_Label";
        WeaponsScanned_Label.Size = new Size(62, 15);
        WeaponsScanned_Label.TabIndex = 4;
        WeaponsScanned_Label.Text = "Weapons: ";
        //
        // WeaponsScannedCount_Label
        //
        WeaponsScannedCount_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        WeaponsScannedCount_Label.AutoSize = true;
        WeaponsScannedCount_Label.ForeColor = Color.FromArgb(44, 62, 80);
        WeaponsScannedCount_Label.Location = new Point(82, 418);
        WeaponsScannedCount_Label.Name = "WeaponsScannedCount_Label";
        WeaponsScannedCount_Label.Size = new Size(13, 15);
        WeaponsScannedCount_Label.TabIndex = 5;
        WeaponsScannedCount_Label.Text = "0";
        //
        // WeaponsScannedSlash_Label
        //
        WeaponsScannedSlash_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        WeaponsScannedSlash_Label.AutoSize = true;
        WeaponsScannedSlash_Label.ForeColor = Color.FromArgb(80, 80, 80);
        WeaponsScannedSlash_Label.Location = new Point(100, 418);
        WeaponsScannedSlash_Label.Name = "WeaponsScannedSlash_Label";
        WeaponsScannedSlash_Label.Size = new Size(12, 15);
        WeaponsScannedSlash_Label.TabIndex = 6;
        WeaponsScannedSlash_Label.Text = "/";
        //
        // WeaponsMax_Labell
        //
        WeaponsMax_Labell.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        WeaponsMax_Labell.AutoSize = true;
        WeaponsMax_Labell.ForeColor = Color.FromArgb(44, 62, 80);
        WeaponsMax_Labell.Location = new Point(115, 418);
        WeaponsMax_Labell.Name = "WeaponsMax_Labell";
        WeaponsMax_Labell.Size = new Size(13, 15);
        WeaponsMax_Labell.TabIndex = 7;
        WeaponsMax_Labell.Text = "0";
        //
        // Artifacts_Label
        //
        Artifacts_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        Artifacts_Label.AutoSize = true;
        Artifacts_Label.ForeColor = Color.FromArgb(80, 80, 80);
        Artifacts_Label.Location = new Point(12, 436);
        Artifacts_Label.Name = "Artifacts_Label";
        Artifacts_Label.Size = new Size(57, 15);
        Artifacts_Label.TabIndex = 8;
        Artifacts_Label.Text = "Artifacts: ";
        //
        // ArtifactsScanned_Label
        //
        ArtifactsScanned_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        ArtifactsScanned_Label.AutoSize = true;
        ArtifactsScanned_Label.ForeColor = Color.FromArgb(44, 62, 80);
        ArtifactsScanned_Label.Location = new Point(82, 436);
        ArtifactsScanned_Label.Name = "ArtifactsScanned_Label";
        ArtifactsScanned_Label.Size = new Size(13, 15);
        ArtifactsScanned_Label.TabIndex = 9;
        ArtifactsScanned_Label.Text = "0";
        //
        // ArtifactsScannedSlash_Label
        //
        ArtifactsScannedSlash_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        ArtifactsScannedSlash_Label.AutoSize = true;
        ArtifactsScannedSlash_Label.ForeColor = Color.FromArgb(80, 80, 80);
        ArtifactsScannedSlash_Label.Location = new Point(100, 436);
        ArtifactsScannedSlash_Label.Name = "ArtifactsScannedSlash_Label";
        ArtifactsScannedSlash_Label.Size = new Size(12, 15);
        ArtifactsScannedSlash_Label.TabIndex = 10;
        ArtifactsScannedSlash_Label.Text = "/";
        //
        // ArtifactsMax_Label
        //
        ArtifactsMax_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        ArtifactsMax_Label.AutoSize = true;
        ArtifactsMax_Label.ForeColor = Color.FromArgb(44, 62, 80);
        ArtifactsMax_Label.Location = new Point(115, 436);
        ArtifactsMax_Label.Name = "ArtifactsMax_Label";
        ArtifactsMax_Label.Size = new Size(13, 15);
        ArtifactsMax_Label.TabIndex = 11;
        ArtifactsMax_Label.Text = "0";
        //
        // Characters_Label
        //
        Characters_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        Characters_Label.AutoSize = true;
        Characters_Label.ForeColor = Color.FromArgb(80, 80, 80);
        Characters_Label.Location = new Point(12, 454);
        Characters_Label.Name = "Characters_Label";
        Characters_Label.Size = new Size(69, 15);
        Characters_Label.TabIndex = 12;
        Characters_Label.Text = "Characters: ";
        //
        // CharactersScanned_Label
        //
        CharactersScanned_Label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        CharactersScanned_Label.AutoSize = true;
        CharactersScanned_Label.ForeColor = Color.FromArgb(44, 62, 80);
        CharactersScanned_Label.Location = new Point(82, 454);
        CharactersScanned_Label.Name = "CharactersScanned_Label";
        CharactersScanned_Label.Size = new Size(13, 15);
        CharactersScanned_Label.TabIndex = 13;
        CharactersScanned_Label.Text = "0";
        //
        // ManualExportButton
        //
        ManualExportButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        ManualExportButton.BackColor = Color.FromArgb(52, 152, 219);
        ManualExportButton.Enabled = false;
        ManualExportButton.FlatAppearance.BorderSize = 0;
        ManualExportButton.FlatStyle = FlatStyle.Flat;
        ManualExportButton.Font = new Font("Segoe UI", 8.25F);
        ManualExportButton.ForeColor = Color.White;
        ManualExportButton.Location = new Point(12, 478);
        ManualExportButton.Name = "ManualExportButton";
        ManualExportButton.Size = new Size(176, 28);
        ManualExportButton.TabIndex = 14;
        ManualExportButton.Text = "Open Genshin Optimizer";
        ManualExportButton.UseVisualStyleBackColor = false;
        ManualExportButton.Click += Export_Button_Click;
        //
        // button1
        //
        button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        button1.BackColor = Color.FromArgb(108, 117, 125);
        button1.FlatAppearance.BorderSize = 0;
        button1.FlatStyle = FlatStyle.Flat;
        button1.Font = new Font("Segoe UI", 8.25F);
        button1.ForeColor = Color.White;
        button1.Location = new Point(12, 510);
        button1.Name = "button1";
        button1.Size = new Size(176, 28);
        button1.TabIndex = 15;
        button1.Text = "Open Export Folder";
        button1.UseVisualStyleBackColor = false;
        button1.Click += ExportFolderMenuItem_Click;

        // =============================================
        // GROUP: FILTER SETTINGS (top-center)
        // =============================================
        grpFilters.Controls.Add(label2);
        grpFilters.Controls.Add(WeaponRarityControl);
        grpFilters.Controls.Add(label3);
        grpFilters.Controls.Add(ArtifactRarityControl);
        grpFilters.Controls.Add(MinimumWeaponLevelLabel);
        grpFilters.Controls.Add(MinimumWeaponLevelControl);
        grpFilters.Controls.Add(MinimumArtifactLevelLabel);
        grpFilters.Controls.Add(numericUpDown1);
        grpFilters.Controls.Add(EquipWeaponsCheckBox);
        grpFilters.Controls.Add(EquipArtifactsCheckBox);
        grpFilters.Controls.Add(label5);
        grpFilters.Controls.Add(NumOfCharToScanControl);
        grpFilters.Controls.Add(SortByObtained);
        grpFilters.Controls.Add(SortByObtainedControl);
        grpFilters.ForeColor = Color.FromArgb(44, 62, 80);
        grpFilters.Location = new Point(198, 30);
        grpFilters.Name = "grpFilters";
        grpFilters.Padding = new Padding(8);
        grpFilters.Size = new Size(258, 248);
        grpFilters.TabIndex = 202;
        grpFilters.TabStop = false;
        grpFilters.Text = "Filter Settings";
        // appSettings1 is the singleton — do NOT overwrite its properties here.
        // Defaults are already set in AppSettings field initializers, and saved
        // values are loaded by SettingsService on startup.
        //
        // label2 (Min Weapon Rarity)
        //
        label2.AutoSize = true;
        label2.ForeColor = Color.FromArgb(44, 62, 80);
        label2.Location = new Point(10, 24);
        label2.Name = "label2";
        label2.Size = new Size(140, 15);
        label2.TabIndex = 0;
        label2.Text = "Min Weapon Rarity";
        //
        // WeaponRarityControl
        //
        WeaponRarityControl.DataBindings.Add(new Binding("Value", appSettings1, "MinimumWeaponRarity", true, DataSourceUpdateMode.OnPropertyChanged));
        WeaponRarityControl.Location = new Point(196, 22);
        WeaponRarityControl.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
        WeaponRarityControl.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        WeaponRarityControl.Name = "WeaponRarityControl";
        WeaponRarityControl.Size = new Size(50, 23);
        WeaponRarityControl.TabIndex = 1;
        WeaponRarityControl.Value = new decimal(new int[] { 3, 0, 0, 0 });
        //
        // label3 (Min Artifact Rarity)
        //
        label3.AutoSize = true;
        label3.ForeColor = Color.FromArgb(44, 62, 80);
        label3.Location = new Point(10, 52);
        label3.Name = "label3";
        label3.Size = new Size(135, 15);
        label3.TabIndex = 2;
        label3.Text = "Min Artifact Rarity";
        //
        // ArtifactRarityControl
        //
        ArtifactRarityControl.DataBindings.Add(new Binding("Value", appSettings1, "MinimumArtifactRarity", true, DataSourceUpdateMode.OnPropertyChanged));
        ArtifactRarityControl.Location = new Point(196, 50);
        ArtifactRarityControl.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
        ArtifactRarityControl.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        ArtifactRarityControl.Name = "ArtifactRarityControl";
        ArtifactRarityControl.Size = new Size(50, 23);
        ArtifactRarityControl.TabIndex = 3;
        ArtifactRarityControl.Value = new decimal(new int[] { 4, 0, 0, 0 });
        //
        // MinimumWeaponLevelLabel
        //
        MinimumWeaponLevelLabel.AutoSize = true;
        MinimumWeaponLevelLabel.ForeColor = Color.FromArgb(44, 62, 80);
        MinimumWeaponLevelLabel.Location = new Point(10, 80);
        MinimumWeaponLevelLabel.Name = "MinimumWeaponLevelLabel";
        MinimumWeaponLevelLabel.Size = new Size(137, 15);
        MinimumWeaponLevelLabel.TabIndex = 4;
        MinimumWeaponLevelLabel.Text = "Min Weapon Level";
        //
        // MinimumWeaponLevelControl
        //
        MinimumWeaponLevelControl.DataBindings.Add(new Binding("Value", appSettings1, "MinimumWeaponLevel", true, DataSourceUpdateMode.OnPropertyChanged));
        MinimumWeaponLevelControl.Location = new Point(196, 78);
        MinimumWeaponLevelControl.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
        MinimumWeaponLevelControl.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        MinimumWeaponLevelControl.Name = "MinimumWeaponLevelControl";
        MinimumWeaponLevelControl.Size = new Size(50, 23);
        MinimumWeaponLevelControl.TabIndex = 5;
        MinimumWeaponLevelControl.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // MinimumArtifactLevelLabel
        //
        MinimumArtifactLevelLabel.AutoSize = true;
        MinimumArtifactLevelLabel.ForeColor = Color.FromArgb(44, 62, 80);
        MinimumArtifactLevelLabel.Location = new Point(10, 108);
        MinimumArtifactLevelLabel.Name = "MinimumArtifactLevelLabel";
        MinimumArtifactLevelLabel.Size = new Size(132, 15);
        MinimumArtifactLevelLabel.TabIndex = 6;
        MinimumArtifactLevelLabel.Text = "Min Artifact Level";
        //
        // numericUpDown1 (Artifact Level)
        //
        numericUpDown1.DataBindings.Add(new Binding("Value", appSettings1, "MinimumArtifactLevel", true, DataSourceUpdateMode.OnPropertyChanged));
        numericUpDown1.Location = new Point(196, 106);
        numericUpDown1.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        numericUpDown1.Name = "numericUpDown1";
        numericUpDown1.Size = new Size(50, 23);
        numericUpDown1.TabIndex = 7;
        //
        // EquipWeaponsCheckBox
        //
        EquipWeaponsCheckBox.AutoSize = true;
        EquipWeaponsCheckBox.Checked = true;
        EquipWeaponsCheckBox.CheckState = CheckState.Checked;
        EquipWeaponsCheckBox.DataBindings.Add(new Binding("Checked", appSettings1, "EquipWeapons", true, DataSourceUpdateMode.OnPropertyChanged));
        EquipWeaponsCheckBox.ForeColor = Color.FromArgb(44, 62, 80);
        EquipWeaponsCheckBox.Location = new Point(10, 136);
        EquipWeaponsCheckBox.Name = "EquipWeaponsCheckBox";
        EquipWeaponsCheckBox.Size = new Size(108, 19);
        EquipWeaponsCheckBox.TabIndex = 8;
        EquipWeaponsCheckBox.Text = "Equip Weapons";
        equipWeaponToolTip.SetToolTip(EquipWeaponsCheckBox, "Keeps weapons equipped to characters in export");
        EquipWeaponsCheckBox.UseVisualStyleBackColor = true;
        //
        // EquipArtifactsCheckBox
        //
        EquipArtifactsCheckBox.AutoSize = true;
        EquipArtifactsCheckBox.Checked = true;
        EquipArtifactsCheckBox.CheckState = CheckState.Checked;
        EquipArtifactsCheckBox.DataBindings.Add(new Binding("Checked", appSettings1, "EquipArtifacts", true, DataSourceUpdateMode.OnPropertyChanged));
        EquipArtifactsCheckBox.ForeColor = Color.FromArgb(44, 62, 80);
        EquipArtifactsCheckBox.Location = new Point(130, 136);
        EquipArtifactsCheckBox.Name = "EquipArtifactsCheckBox";
        EquipArtifactsCheckBox.Size = new Size(103, 19);
        EquipArtifactsCheckBox.TabIndex = 9;
        EquipArtifactsCheckBox.Text = "Equip Artifacts";
        equipArtifactsToolTip.SetToolTip(EquipArtifactsCheckBox, "Keeps artifacts equipped to characters in export");
        EquipArtifactsCheckBox.UseVisualStyleBackColor = true;
        //
        // label5 (Num Characters to Scan)
        //
        label5.AutoSize = true;
        label5.ForeColor = Color.FromArgb(44, 62, 80);
        label5.Location = new Point(10, 166);
        label5.Name = "label5";
        label5.Size = new Size(163, 15);
        label5.TabIndex = 10;
        label5.Text = "# Characters To Scan";
        //
        // NumOfCharToScanControl
        //
        NumOfCharToScanControl.DataBindings.Add(new Binding("Value", appSettings1, "NumOfCharToScan", true, DataSourceUpdateMode.OnPropertyChanged));
        NumOfCharToScanControl.Location = new Point(196, 164);
        NumOfCharToScanControl.Name = "NumOfCharToScanControl";
        NumOfCharToScanControl.Size = new Size(50, 23);
        NumOfCharToScanControl.TabIndex = 11;
        ZeroMeansAllTooltips.SetToolTip(NumOfCharToScanControl, "Selecting 0 scans all characters");
        //
        // SortByObtained (Num Artifact Pages)
        //
        SortByObtained.AutoSize = true;
        SortByObtained.ForeColor = Color.FromArgb(44, 62, 80);
        SortByObtained.Location = new Point(10, 196);
        SortByObtained.Name = "SortByObtained";
        SortByObtained.Size = new Size(161, 15);
        SortByObtained.TabIndex = 12;
        SortByObtained.Text = "# Artifact Pages To Scan";
        //
        // SortByObtainedControl
        //
        SortByObtainedControl.DataBindings.Add(new Binding("Value", appSettings1, "SortByObtained", true, DataSourceUpdateMode.OnPropertyChanged));
        SortByObtainedControl.Location = new Point(196, 194);
        SortByObtainedControl.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        SortByObtainedControl.Name = "SortByObtainedControl";
        SortByObtainedControl.Size = new Size(50, 23);
        SortByObtainedControl.TabIndex = 13;
        ZeroMeansAllTooltips.SetToolTip(SortByObtainedControl, "Selecting 0 scans all Artifacts");

        // =============================================
        // GROUP: CUSTOM NAMES (top-right)
        // =============================================
        grpNames.Controls.Add(label1);
        grpNames.Controls.Add(travelerNameTextBox);
        grpNames.Controls.Add(label4);
        grpNames.Controls.Add(wandererNameTextBox);
        grpNames.Controls.Add(label6);
        grpNames.Controls.Add(textBox1);
        grpNames.Controls.Add(label7);
        grpNames.Controls.Add(textBox2);
        grpNames.ForeColor = Color.FromArgb(44, 62, 80);
        grpNames.Location = new Point(466, 30);
        grpNames.Name = "grpNames";
        grpNames.Padding = new Padding(8);
        grpNames.Size = new Size(392, 140);
        grpNames.TabIndex = 203;
        grpNames.TabStop = false;
        grpNames.Text = "Custom Names";
        //
        // label1 (Traveler)
        //
        label1.AutoSize = true;
        label1.ForeColor = Color.FromArgb(44, 62, 80);
        label1.Location = new Point(10, 24);
        label1.Name = "label1";
        label1.Size = new Size(94, 15);
        label1.TabIndex = 0;
        label1.Text = "Traveler's Name:";
        //
        // travelerNameTextBox
        //
        travelerNameTextBox.DataBindings.Add(new Binding("Text", appSettings1, "TravelerName", true, DataSourceUpdateMode.OnPropertyChanged));
        travelerNameTextBox.Location = new Point(140, 21);
        travelerNameTextBox.Name = "travelerNameTextBox";
        travelerNameTextBox.Size = new Size(240, 23);
        travelerNameTextBox.TabIndex = 1;
        travelerNameTextBox.TextChanged += ValidateCustomName;
        travelerNameTextBox.MouseHover += DisplayCustomNameTooltip;
        travelerNameTextBox.ParentChanged += ValidateCustomName;
        //
        // label4 (Wanderer)
        //
        label4.AutoSize = true;
        label4.ForeColor = Color.FromArgb(44, 62, 80);
        label4.Location = new Point(10, 52);
        label4.Name = "label4";
        label4.Size = new Size(104, 15);
        label4.TabIndex = 2;
        label4.Text = "Wanderer's Name:";
        //
        // wandererNameTextBox
        //
        wandererNameTextBox.DataBindings.Add(new Binding("Text", appSettings1, "WandererName", true, DataSourceUpdateMode.OnPropertyChanged));
        wandererNameTextBox.Location = new Point(140, 49);
        wandererNameTextBox.Name = "wandererNameTextBox";
        wandererNameTextBox.Size = new Size(240, 23);
        wandererNameTextBox.TabIndex = 3;
        wandererNameTextBox.Text = "Wanderer";
        wandererNameTextBox.TextChanged += ValidateCustomName;
        wandererNameTextBox.MouseHover += DisplayCustomNameTooltip;
        wandererNameTextBox.ParentChanged += ValidateCustomName2;
        //
        // label6 (Manequin f)
        //
        label6.AutoSize = true;
        label6.ForeColor = Color.FromArgb(44, 62, 80);
        label6.Location = new Point(10, 80);
        label6.Name = "label6";
        label6.Size = new Size(122, 15);
        label6.TabIndex = 4;
        label6.Text = "Manequin (f) Name:";
        //
        // textBox1 (Manequin f)
        //
        textBox1.DataBindings.Add(new Binding("Text", appSettings1, "Manequin1Name", true, DataSourceUpdateMode.OnPropertyChanged));
        textBox1.Location = new Point(140, 77);
        textBox1.Name = "textBox1";
        textBox1.Size = new Size(240, 23);
        textBox1.TabIndex = 5;
        textBox1.Text = "Manequin1";
        textBox1.TextChanged += ValidateCustomName1;
        //
        // label7 (Manequin m)
        //
        label7.AutoSize = true;
        label7.ForeColor = Color.FromArgb(44, 62, 80);
        label7.Location = new Point(10, 108);
        label7.Name = "label7";
        label7.Size = new Size(129, 15);
        label7.TabIndex = 6;
        label7.Text = "Manequin (m) Name:";
        //
        // textBox2 (Manequin m)
        //
        textBox2.DataBindings.Add(new Binding("Text", appSettings1, "Manequin2Name", true, DataSourceUpdateMode.OnPropertyChanged));
        textBox2.Location = new Point(140, 105);
        textBox2.Name = "textBox2";
        textBox2.Size = new Size(240, 23);
        textBox2.TabIndex = 7;
        textBox2.Text = "Manequin2";
        textBox2.TextChanged += ValidateCustomName2;

        // =============================================
        // LINKS + NAVIGATION (below custom names)
        // =============================================
        //
        // Github_Label
        //
        Github_Label.AutoSize = true;
        Github_Label.Font = new Font("Segoe UI", 8.25F);
        Github_Label.LinkColor = Color.FromArgb(52, 152, 219);
        Github_Label.Location = new Point(468, 178);
        Github_Label.Name = "Github_Label";
        Github_Label.Size = new Size(93, 13);
        Github_Label.TabIndex = 30;
        Github_Label.TabStop = true;
        Github_Label.Text = "Github Mainpage";
        Github_Label.LinkClicked += Github_Label_LinkClicked;
        //
        // Releases_Label
        //
        Releases_Label.AutoSize = true;
        Releases_Label.Font = new Font("Segoe UI", 8.25F);
        Releases_Label.LinkColor = Color.FromArgb(52, 152, 219);
        Releases_Label.Location = new Point(570, 178);
        Releases_Label.Name = "Releases_Label";
        Releases_Label.Size = new Size(84, 13);
        Releases_Label.TabIndex = 31;
        Releases_Label.TabStop = true;
        Releases_Label.Text = "Latest Releases";
        Releases_Label.LinkClicked += Releases_Label_LinkClicked;
        //
        // Navigation_Label
        //
        Navigation_Label.AutoSize = true;
        Navigation_Label.ForeColor = Color.FromArgb(100, 100, 100);
        Navigation_Label.Location = new Point(468, 200);
        Navigation_Label.Name = "Navigation_Label";
        Navigation_Label.Size = new Size(68, 15);
        Navigation_Label.TabIndex = 32;
        Navigation_Label.Text = "Navigation:";
        //
        // Navigation_Image
        //
        Navigation_Image.BackColor = Color.FromArgb(230, 230, 235);
        Navigation_Image.Location = new Point(468, 218);
        Navigation_Image.Name = "Navigation_Image";
        Navigation_Image.Size = new Size(90, 44);
        Navigation_Image.SizeMode = PictureBoxSizeMode.StretchImage;
        Navigation_Image.TabIndex = 33;
        Navigation_Image.TabStop = false;

        // =============================================
        // OUTPUT LOCATION LABEL
        // =============================================
        //
        // FileLocation_Label
        //
        FileLocation_Label.AutoSize = true;
        FileLocation_Label.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        FileLocation_Label.ForeColor = Color.FromArgb(44, 62, 80);
        FileLocation_Label.Location = new Point(198, 274);
        FileLocation_Label.Name = "FileLocation_Label";
        FileLocation_Label.Size = new Size(96, 15);
        FileLocation_Label.TabIndex = 40;
        FileLocation_Label.Text = "Output Location";

        // =============================================
        // SCANNER OUTPUT PANEL (bottom area)
        // =============================================
        ScannerOutput_Panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        ScannerOutput_Panel.BackColor = Color.FromArgb(44, 62, 80);
        ScannerOutput_Panel.Controls.Add(FileSelectButton);
        ScannerOutput_Panel.Controls.Add(OutputPath_TextBox);
        ScannerOutput_Panel.Controls.Add(WeaponArtifact_Label);
        ScannerOutput_Panel.Controls.Add(WeaponArtifactOutput_TextBox_Label);
        ScannerOutput_Panel.Controls.Add(GearPictureBox);
        ScannerOutput_Panel.Controls.Add(ArtifactOutput_TextBox);
        ScannerOutput_Panel.Controls.Add(Character_Label);
        ScannerOutput_Panel.Controls.Add(CharacterOutput_TextBox_Label);
        ScannerOutput_Panel.Controls.Add(CharacterName_PictureBox);
        ScannerOutput_Panel.Controls.Add(CharacterLevel_PictureBox);
        ScannerOutput_Panel.Controls.Add(CharacterTalent1_PictureBox);
        ScannerOutput_Panel.Controls.Add(CharacterTalent2_PictureBox);
        ScannerOutput_Panel.Controls.Add(CharacterTalent3_PictureBox);
        ScannerOutput_Panel.Controls.Add(CharacterOutput_TextBox);
        ScannerOutput_Panel.Controls.Add(ErrorLog_Label);
        ScannerOutput_Panel.Controls.Add(ErrorReport_Label);
        ScannerOutput_Panel.Controls.Add(IssuesPage_Label);
        ScannerOutput_Panel.Controls.Add(LogLevel_Label);
        ScannerOutput_Panel.Controls.Add(LogLevel_ComboBox);
        ScannerOutput_Panel.Controls.Add(ClearLog_Button);
        ScannerOutput_Panel.Controls.Add(ErrorLog_TextBox);
        ScannerOutput_Panel.Location = new Point(198, 292);
        ScannerOutput_Panel.Name = "ScannerOutput_Panel";
        ScannerOutput_Panel.Size = new Size(660, 398);
        ScannerOutput_Panel.TabIndex = 50;
        //
        // FileSelectButton
        //
        FileSelectButton.BackColor = Color.FromArgb(52, 73, 94);
        FileSelectButton.FlatAppearance.BorderColor = Color.FromArgb(80, 100, 120);
        FileSelectButton.FlatStyle = FlatStyle.Flat;
        FileSelectButton.Font = new Font("Segoe UI", 7.5F);
        FileSelectButton.ForeColor = Color.White;
        FileSelectButton.Location = new Point(4, 4);
        FileSelectButton.Name = "FileSelectButton";
        FileSelectButton.Size = new Size(56, 24);
        FileSelectButton.TabIndex = 0;
        FileSelectButton.Text = "Select...";
        FileSelectButton.UseVisualStyleBackColor = false;
        FileSelectButton.Click += FileSelectButton_Click;
        //
        // OutputPath_TextBox
        //
        OutputPath_TextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        OutputPath_TextBox.BackColor = Color.FromArgb(52, 73, 94);
        OutputPath_TextBox.BorderStyle = BorderStyle.FixedSingle;
        OutputPath_TextBox.DataBindings.Add(new Binding("Text", appSettings1, "OutputPath", true, DataSourceUpdateMode.OnPropertyChanged));
        OutputPath_TextBox.Font = new Font("Segoe UI", 8F);
        OutputPath_TextBox.ForeColor = Color.FromArgb(200, 210, 220);
        OutputPath_TextBox.Location = new Point(64, 5);
        OutputPath_TextBox.Name = "OutputPath_TextBox";
        OutputPath_TextBox.Size = new Size(590, 22);
        OutputPath_TextBox.TabIndex = 1;
        //
        // WeaponArtifact_Label
        //
        WeaponArtifact_Label.AutoSize = true;
        WeaponArtifact_Label.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold);
        WeaponArtifact_Label.ForeColor = Color.FromArgb(189, 195, 199);
        WeaponArtifact_Label.Location = new Point(4, 34);
        WeaponArtifact_Label.Name = "WeaponArtifact_Label";
        WeaponArtifact_Label.Size = new Size(98, 13);
        WeaponArtifact_Label.TabIndex = 2;
        WeaponArtifact_Label.Text = "Weapon / Artifact";
        //
        // WeaponArtifactOutput_TextBox_Label
        //
        WeaponArtifactOutput_TextBox_Label.AutoSize = true;
        WeaponArtifactOutput_TextBox_Label.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold);
        WeaponArtifactOutput_TextBox_Label.ForeColor = Color.FromArgb(189, 195, 199);
        WeaponArtifactOutput_TextBox_Label.Location = new Point(124, 34);
        WeaponArtifactOutput_TextBox_Label.Name = "WeaponArtifactOutput_TextBox_Label";
        WeaponArtifactOutput_TextBox_Label.Size = new Size(45, 13);
        WeaponArtifactOutput_TextBox_Label.TabIndex = 3;
        WeaponArtifactOutput_TextBox_Label.Text = "Output";
        //
        // GearPictureBox
        //
        GearPictureBox.BackColor = Color.FromArgb(52, 73, 94);
        GearPictureBox.Location = new Point(4, 52);
        GearPictureBox.Name = "GearPictureBox";
        GearPictureBox.Size = new Size(116, 164);
        GearPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        GearPictureBox.TabIndex = 4;
        GearPictureBox.TabStop = false;
        //
        // ArtifactOutput_TextBox
        //
        ArtifactOutput_TextBox.BackColor = Color.FromArgb(52, 73, 94);
        ArtifactOutput_TextBox.BorderStyle = BorderStyle.FixedSingle;
        ArtifactOutput_TextBox.ForeColor = Color.FromArgb(200, 210, 220);
        ArtifactOutput_TextBox.Location = new Point(124, 52);
        ArtifactOutput_TextBox.Multiline = true;
        ArtifactOutput_TextBox.Name = "ArtifactOutput_TextBox";
        ArtifactOutput_TextBox.ReadOnly = true;
        ArtifactOutput_TextBox.ScrollBars = ScrollBars.Vertical;
        ArtifactOutput_TextBox.Size = new Size(170, 164);
        ArtifactOutput_TextBox.TabIndex = 5;
        //
        // Character_Label
        //
        Character_Label.AutoSize = true;
        Character_Label.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold);
        Character_Label.ForeColor = Color.FromArgb(189, 195, 199);
        Character_Label.Location = new Point(306, 34);
        Character_Label.Name = "Character_Label";
        Character_Label.Size = new Size(61, 13);
        Character_Label.TabIndex = 6;
        Character_Label.Text = "Character";
        //
        // CharacterOutput_TextBox_Label
        //
        CharacterOutput_TextBox_Label.AutoSize = true;
        CharacterOutput_TextBox_Label.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold);
        CharacterOutput_TextBox_Label.ForeColor = Color.FromArgb(189, 195, 199);
        CharacterOutput_TextBox_Label.Location = new Point(428, 34);
        CharacterOutput_TextBox_Label.Name = "CharacterOutput_TextBox_Label";
        CharacterOutput_TextBox_Label.Size = new Size(45, 13);
        CharacterOutput_TextBox_Label.TabIndex = 7;
        CharacterOutput_TextBox_Label.Text = "Output";
        //
        // CharacterName_PictureBox
        //
        CharacterName_PictureBox.BackColor = Color.FromArgb(52, 73, 94);
        CharacterName_PictureBox.Location = new Point(306, 52);
        CharacterName_PictureBox.Name = "CharacterName_PictureBox";
        CharacterName_PictureBox.Size = new Size(116, 24);
        CharacterName_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        CharacterName_PictureBox.TabIndex = 8;
        CharacterName_PictureBox.TabStop = false;
        //
        // CharacterLevel_PictureBox
        //
        CharacterLevel_PictureBox.BackColor = Color.FromArgb(52, 73, 94);
        CharacterLevel_PictureBox.Location = new Point(306, 80);
        CharacterLevel_PictureBox.Name = "CharacterLevel_PictureBox";
        CharacterLevel_PictureBox.Size = new Size(116, 20);
        CharacterLevel_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        CharacterLevel_PictureBox.TabIndex = 9;
        CharacterLevel_PictureBox.TabStop = false;
        //
        // CharacterTalent1_PictureBox
        //
        CharacterTalent1_PictureBox.BackColor = Color.FromArgb(52, 73, 94);
        CharacterTalent1_PictureBox.Location = new Point(306, 104);
        CharacterTalent1_PictureBox.Name = "CharacterTalent1_PictureBox";
        CharacterTalent1_PictureBox.Size = new Size(116, 18);
        CharacterTalent1_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        CharacterTalent1_PictureBox.TabIndex = 10;
        CharacterTalent1_PictureBox.TabStop = false;
        //
        // CharacterTalent2_PictureBox
        //
        CharacterTalent2_PictureBox.BackColor = Color.FromArgb(52, 73, 94);
        CharacterTalent2_PictureBox.Location = new Point(306, 126);
        CharacterTalent2_PictureBox.Name = "CharacterTalent2_PictureBox";
        CharacterTalent2_PictureBox.Size = new Size(116, 18);
        CharacterTalent2_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        CharacterTalent2_PictureBox.TabIndex = 11;
        CharacterTalent2_PictureBox.TabStop = false;
        //
        // CharacterTalent3_PictureBox
        //
        CharacterTalent3_PictureBox.BackColor = Color.FromArgb(52, 73, 94);
        CharacterTalent3_PictureBox.Location = new Point(306, 148);
        CharacterTalent3_PictureBox.Name = "CharacterTalent3_PictureBox";
        CharacterTalent3_PictureBox.Size = new Size(116, 18);
        CharacterTalent3_PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        CharacterTalent3_PictureBox.TabIndex = 12;
        CharacterTalent3_PictureBox.TabStop = false;
        //
        // CharacterOutput_TextBox
        //
        CharacterOutput_TextBox.BackColor = Color.FromArgb(52, 73, 94);
        CharacterOutput_TextBox.BorderStyle = BorderStyle.FixedSingle;
        CharacterOutput_TextBox.ForeColor = Color.FromArgb(200, 210, 220);
        CharacterOutput_TextBox.Location = new Point(428, 52);
        CharacterOutput_TextBox.Multiline = true;
        CharacterOutput_TextBox.Name = "CharacterOutput_TextBox";
        CharacterOutput_TextBox.ReadOnly = true;
        CharacterOutput_TextBox.ScrollBars = ScrollBars.Vertical;
        CharacterOutput_TextBox.Size = new Size(226, 164);
        CharacterOutput_TextBox.TabIndex = 13;
        //
        // ErrorLog_Label
        //
        ErrorLog_Label.AutoSize = true;
        ErrorLog_Label.Cursor = Cursors.Hand;
        ErrorLog_Label.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold);
        ErrorLog_Label.ForeColor = Color.FromArgb(189, 195, 199);
        ErrorLog_Label.Location = new Point(4, 224);
        ErrorLog_Label.Name = "ErrorLog_Label";
        ErrorLog_Label.Size = new Size(160, 13);
        ErrorLog_Label.TabIndex = 14;
        ErrorLog_Label.Text = "Activity Log  (scroll / click me)";
        ErrorLog_Label.Click += ErrorLog_Label_Click;
        //
        // ErrorReport_Label
        //
        ErrorReport_Label.AutoSize = true;
        ErrorReport_Label.ForeColor = Color.FromArgb(189, 195, 199);
        ErrorReport_Label.Location = new Point(220, 224);
        ErrorReport_Label.Name = "ErrorReport_Label";
        ErrorReport_Label.Size = new Size(133, 15);
        ErrorReport_Label.TabIndex = 15;
        ErrorReport_Label.Text = "Got Errors? Report Here:";
        //
        // IssuesPage_Label
        //
        IssuesPage_Label.AutoSize = true;
        IssuesPage_Label.LinkColor = Color.FromArgb(46, 204, 113);
        IssuesPage_Label.Location = new Point(358, 224);
        IssuesPage_Label.Name = "IssuesPage_Label";
        IssuesPage_Label.Size = new Size(62, 15);
        IssuesPage_Label.TabIndex = 16;
        IssuesPage_Label.TabStop = true;
        IssuesPage_Label.Text = "Issue Page";
        IssuesPage_Label.LinkClicked += IssuesPage_Label_LinkClicked;
        // 
        // LogLevel_Label
        // 
        LogLevel_Label.AutoSize = true;
        LogLevel_Label.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Bold);
        LogLevel_Label.ForeColor = Color.FromArgb(189, 195, 199);
        LogLevel_Label.Location = new Point(436, 224);
        LogLevel_Label.Name = "LogLevel_Label";
        LogLevel_Label.Size = new Size(36, 13);
        LogLevel_Label.TabIndex = 18;
        LogLevel_Label.Text = "Level:";
        // 
        // LogLevel_ComboBox
        // 
        LogLevel_ComboBox.BackColor = Color.FromArgb(52, 73, 94);
        LogLevel_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        LogLevel_ComboBox.FlatStyle = FlatStyle.Flat;
        LogLevel_ComboBox.Font = new Font("Segoe UI", 8F);
        LogLevel_ComboBox.ForeColor = Color.FromArgb(236, 240, 241);
        LogLevel_ComboBox.FormattingEnabled = true;
        LogLevel_ComboBox.Items.AddRange(new object[] { "Debug", "Info", "Warn", "Error" });
        LogLevel_ComboBox.Location = new Point(474, 220);
        LogLevel_ComboBox.Name = "LogLevel_ComboBox";
        LogLevel_ComboBox.Size = new Size(74, 21);
        LogLevel_ComboBox.TabIndex = 19;
        LogLevel_ComboBox.SelectedIndexChanged += LogLevel_ComboBox_SelectedIndexChanged;
        // 
        // ClearLog_Button
        // 
        ClearLog_Button.BackColor = Color.FromArgb(52, 73, 94);
        ClearLog_Button.FlatAppearance.BorderColor = Color.FromArgb(80, 100, 120);
        ClearLog_Button.FlatStyle = FlatStyle.Flat;
        ClearLog_Button.Font = new Font("Segoe UI", 7.5F);
        ClearLog_Button.ForeColor = Color.FromArgb(189, 195, 199);
        ClearLog_Button.Location = new Point(554, 220);
        ClearLog_Button.Name = "ClearLog_Button";
        ClearLog_Button.Size = new Size(48, 21);
        ClearLog_Button.TabIndex = 20;
        ClearLog_Button.Text = "Clear";
        ClearLog_Button.UseVisualStyleBackColor = false;
        ClearLog_Button.Click += ClearLog_Button_Click;
        // 
        // ErrorLog_TextBox
        // 
        ErrorLog_TextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        ErrorLog_TextBox.BackColor = Color.FromArgb(52, 73, 94);
        ErrorLog_TextBox.BorderStyle = BorderStyle.None;
        ErrorLog_TextBox.Font = new Font("Cascadia Mono", 8F, FontStyle.Regular, GraphicsUnit.Point);
        ErrorLog_TextBox.ForeColor = Color.FromArgb(200, 210, 220);
        ErrorLog_TextBox.Location = new Point(4, 242);
        ErrorLog_TextBox.Name = "ErrorLog_TextBox";
        ErrorLog_TextBox.ReadOnly = true;
        ErrorLog_TextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
        ErrorLog_TextBox.Size = new Size(650, 150);
        ErrorLog_TextBox.TabIndex = 17;
        //
        // screenshotsToolTip
        //
        screenshotsToolTip.ToolTipIcon = ToolTipIcon.Warning;

        // =============================================
        // MAIN FORM
        // =============================================
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(245, 246, 250);
        ClientSize = new Size(870, 700);
        Font = new Font("Segoe UI", 9F);
        Controls.Add(grpScanItems);
        Controls.Add(grpScanSpeed);
        Controls.Add(StartScan_Button);
        Controls.Add(ScannerCancelInstructions_Label);
        Controls.Add(ProgramStatus_Label);
        Controls.Add(WeaponsScanned_Label);
        Controls.Add(WeaponsScannedCount_Label);
        Controls.Add(WeaponsScannedSlash_Label);
        Controls.Add(WeaponsMax_Labell);
        Controls.Add(Artifacts_Label);
        Controls.Add(ArtifactsScanned_Label);
        Controls.Add(ArtifactsScannedSlash_Label);
        Controls.Add(ArtifactsMax_Label);
        Controls.Add(Characters_Label);
        Controls.Add(CharactersScanned_Label);
        Controls.Add(ManualExportButton);
        Controls.Add(button1);
        Controls.Add(grpFilters);
        Controls.Add(grpNames);
        Controls.Add(Github_Label);
        Controls.Add(Releases_Label);
        Controls.Add(Navigation_Label);
        Controls.Add(Navigation_Image);
        Controls.Add(FileLocation_Label);
        Controls.Add(ScannerOutput_Panel);
        Controls.Add(menuStrip1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuStrip1;
        Margin = new Padding(4);
        MinimumSize = new Size(886, 739);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Inventory Kamera V#";
        FormClosing += Form1_FormClosing;
        Load += MainForm_Load;
        Shown += MainForm_Shown;
        // EndInit
        ((System.ComponentModel.ISupportInitialize)ScannerDelay_TrackBar).EndInit();
        ((System.ComponentModel.ISupportInitialize)WeaponRarityControl).EndInit();
        ((System.ComponentModel.ISupportInitialize)ArtifactRarityControl).EndInit();
        ((System.ComponentModel.ISupportInitialize)MinimumWeaponLevelControl).EndInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
        ((System.ComponentModel.ISupportInitialize)NumOfCharToScanControl).EndInit();
        ((System.ComponentModel.ISupportInitialize)SortByObtainedControl).EndInit();
        ((System.ComponentModel.ISupportInitialize)Navigation_Image).EndInit();
        ((System.ComponentModel.ISupportInitialize)GearPictureBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)CharacterName_PictureBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)CharacterLevel_PictureBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)CharacterTalent1_PictureBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)CharacterTalent2_PictureBox).EndInit();
        ((System.ComponentModel.ISupportInitialize)CharacterTalent3_PictureBox).EndInit();
        grpScanItems.ResumeLayout(false);
        grpScanItems.PerformLayout();
        grpScanSpeed.ResumeLayout(false);
        grpScanSpeed.PerformLayout();
        grpFilters.ResumeLayout(false);
        grpFilters.PerformLayout();
        grpNames.ResumeLayout(false);
        grpNames.PerformLayout();
        ScannerOutput_Panel.ResumeLayout(false);
        ScannerOutput_Panel.PerformLayout();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Button StartScan_Button;
    private System.Windows.Forms.Label WeaponArtifact_Label;
    private System.Windows.Forms.Label WeaponArtifactOutput_TextBox_Label;
    private System.Windows.Forms.Label ScannerCancelInstructions_Label;
    private System.Windows.Forms.ComboBox Language_ComboBox;
    private System.Windows.Forms.TextBox ArtifactOutput_TextBox;
    private System.Windows.Forms.TextBox CharacterOutput_TextBox;
    private System.Windows.Forms.PictureBox CharacterTalent3_PictureBox;
    private System.Windows.Forms.PictureBox CharacterTalent2_PictureBox;
    private System.Windows.Forms.PictureBox CharacterTalent1_PictureBox;
    private System.Windows.Forms.PictureBox CharacterLevel_PictureBox;
    private System.Windows.Forms.PictureBox CharacterName_PictureBox;
    private System.Windows.Forms.Label CharacterOutput_TextBox_Label;
    private System.Windows.Forms.Label Character_Label;
    private System.Windows.Forms.Label WeaponsScanned_Label;
    private System.Windows.Forms.Label Artifacts_Label;
    private System.Windows.Forms.Label Characters_Label;
    private System.Windows.Forms.Label CharactersScanned_Label;
    private System.Windows.Forms.Label ArtifactsScanned_Label;
    private System.Windows.Forms.Label WeaponsScannedCount_Label;
    private System.Windows.Forms.Label WeaponsScannedSlash_Label;
    private System.Windows.Forms.Label ArtifactsScannedSlash_Label;
    private System.Windows.Forms.Label WeaponsMax_Labell;
    private System.Windows.Forms.Label ArtifactsMax_Label;
    private System.Windows.Forms.Label ProgramStatus_Label;
    private System.Windows.Forms.LinkLabel Github_Label;
    private System.Windows.Forms.RichTextBox ErrorLog_TextBox;
    private System.Windows.Forms.Label ErrorLog_Label;
    private System.Windows.Forms.Label ErrorReport_Label;
    private System.Windows.Forms.LinkLabel IssuesPage_Label;
    private System.Windows.Forms.Label LogLevel_Label;
    private System.Windows.Forms.ComboBox LogLevel_ComboBox;
    private System.Windows.Forms.Button ClearLog_Button;
    private System.Windows.Forms.LinkLabel Releases_Label;
    private System.Windows.Forms.PictureBox Navigation_Image;
    private System.Windows.Forms.Label Navigation_Label;
    private System.Windows.Forms.Panel ScannerOutput_Panel;
    private System.Windows.Forms.TrackBar ScannerDelay_TrackBar;
    private System.Windows.Forms.Label ScannerDelay_Label;
    private System.Windows.Forms.CheckBox Weapons_CheckBox;
    private System.Windows.Forms.Label ScanItemsList_Label;
    private System.Windows.Forms.CheckBox Artifacts_Checkbox;
    private System.Windows.Forms.CheckBox Characters_CheckBox;
    private System.Windows.Forms.Label FastScannerDelay_Label;
    private System.Windows.Forms.Label MidScannerDelay_Label;
    private System.Windows.Forms.Label SlowScannerDelay_Label;
    private System.Windows.Forms.Button FileSelectButton;
    private System.Windows.Forms.Label FileLocation_Label;
    private System.Windows.Forms.TextBox OutputPath_TextBox;
    private System.Windows.Forms.Label Language_Label;
    private System.Windows.Forms.CheckBox CharDevItems_CheckBox;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem keysToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem Quit_MenuItem;
    private System.Windows.Forms.ToolStripTextBox inventoryToolStripTextBox;
    private System.Windows.Forms.ToolStripMenuItem characterScreenToolStripMenuItem;
    private System.Windows.Forms.ToolStripTextBox characterToolStripTextBox;
    private System.Windows.Forms.PictureBox GearPictureBox;
    private System.Windows.Forms.CheckBox Materials_CheckBox;
    private System.Windows.Forms.ToolStripMenuItem DatabaseUpdateMenuItem;
    private System.Windows.Forms.NumericUpDown WeaponRarityControl;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.NumericUpDown ArtifactRarityControl;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    private System.Windows.Forms.Label MinimumWeaponLevelLabel;
    private System.Windows.Forms.Label MinimumArtifactLevelLabel;
    private System.Windows.Forms.NumericUpDown MinimumWeaponLevelControl;
    private System.Windows.Forms.NumericUpDown numericUpDown1;
    private System.Windows.Forms.CheckBox LogScreenshotsCheckBox;
    private System.Windows.Forms.Button ManualExportButton;
    private System.Windows.Forms.CheckBox EquipArtifactsCheckBox;
    private System.Windows.Forms.CheckBox EquipWeaponsCheckBox;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TextBox travelerNameTextBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox wandererNameTextBox;
    private System.Windows.Forms.ToolTip equipWeaponToolTip;
    private System.Windows.Forms.ToolTip equipArtifactsToolTip;
    private System.Windows.Forms.ToolTip screenshotsToolTip;
    private System.Windows.Forms.ToolStripMenuItem updateExecutablesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem characterSlot1KeyToolStripMenuItem;
    private System.Windows.Forms.ToolStripTextBox slot1StripTextBox;
    private System.Windows.Forms.Label SortByObtained;
    private System.Windows.Forms.NumericUpDown SortByObtainedControl;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.NumericUpDown NumOfCharToScanControl;
    private System.Windows.Forms.ToolTip ZeroMeansAllTooltips;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.GroupBox grpScanItems;
    private System.Windows.Forms.GroupBox grpScanSpeed;
    private System.Windows.Forms.GroupBox grpFilters;
    private System.Windows.Forms.GroupBox grpNames;
}
