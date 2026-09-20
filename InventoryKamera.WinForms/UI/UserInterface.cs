using InventoryKamera.Properties;

namespace InventoryKamera;

public static class UserInterface
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	// Artifacts and Weapons
	private static PictureBox gear_PictureBox;

	private static TextBox gear_TextBox;

	// Character
	private static PictureBox cName_PictureBox;

	private static PictureBox cLevel_PictureBox;
	private static PictureBox[] cTalent_PictureBoxes = new PictureBox[3];
	private static TextBox character_TextBox;

	// Counters
	private static Label weaponCount_Label;

	private static Label weaponMax_Label;

	private static Label artifactCount_Label;
	private static Label artifactMax_Label;

	private static Label characterCount_Label;

	// Status
	private static Label programStatus_Label;

	// Log box
	private static RichTextBox error_TextBox;

	private static readonly object _logLock = new object();
	private const int MaxLogLines = 2000;

	// Current Images
	private static PictureBox navigation_PictureBox;

	public static void Init(PictureBox _gear_PictureBox, TextBox _a_textbox, PictureBox _c_name, PictureBox _c_level, PictureBox[] _c_talent, TextBox _c_textbox, Label _weaponCount, Label _weaponMax, Label _artifactCount, Label _artifactMax, Label _characterCount, Label _programStatus, RichTextBox _error_textBox, PictureBox _navigation_Image)
	{
		// Artifacts and Weapons
		gear_PictureBox = _gear_PictureBox;
		gear_TextBox = _a_textbox;

		// Characters
		cName_PictureBox = _c_name;
		cLevel_PictureBox = _c_level;
		cTalent_PictureBoxes = _c_talent;
		character_TextBox = _c_textbox;

		// Counters
		weaponCount_Label = _weaponCount;
		weaponMax_Label = _weaponMax;
		artifactCount_Label = _artifactCount;
		artifactMax_Label = _artifactMax;
		characterCount_Label = _characterCount;

		// Status
		programStatus_Label = _programStatus;

		// Log
		error_TextBox = _error_textBox;

		// Navigation Image
		navigation_PictureBox = _navigation_Image;

		// Register callbacks so the Services layer can update the UI
		RegisterBridgeCallbacks();

		// Route NLog messages to the Activity Log panel
		SetupNLogUiTarget();
	}

	private static void RegisterBridgeCallbacks()
	{
		UserInterfaceBridge.SetNavigationImage = bm => SetNavigation_Image(bm);
		UserInterfaceBridge.AddError = error => AddError(error);
		UserInterfaceBridge.UnexpectedError = error => AddError(error);
		UserInterfaceBridge.SetGearPictureBox = bm => SetGearPictureBox(bm);
		UserInterfaceBridge.SetGear = (bm, gear) =>
		{
			if (gear is Weapon weapon) SetGear(bm, weapon);
			else if (gear is Artifact artifact) SetGear(bm, artifact);
		};
		UserInterfaceBridge.IncrementWeaponCount = () => IncrementWeaponCount();
		UserInterfaceBridge.IncrementArtifactCount = () => IncrementArtifactCount();
		UserInterfaceBridge.IncrementCharacterCount = () => IncrementCharacterCount();
		UserInterfaceBridge.SetWeaponMax = max => SetWeapon_Max(max);
		UserInterfaceBridge.SetArtifactMax = max => SetArtifact_Max(max);
		UserInterfaceBridge.ResetCharacterDisplay = () => ResetCharacterDisplay();
		UserInterfaceBridge.SetCharacterNameAndElement = (bm, name, element) => SetCharacter_NameAndElement(bm, name, element);
		UserInterfaceBridge.SetCharacterLevel = (bm, level, maxLevel) => SetCharacter_Level(bm, level, maxLevel);
		UserInterfaceBridge.SetCharacterConstellation = level => SetCharacter_Constellation(level);
		UserInterfaceBridge.SetCharacterTalent = (bm, level, index) => SetCharacter_Talent(bm, level, index);
		UserInterfaceBridge.SetMainCharacterName = name => SetMainCharacterName(name);
		UserInterfaceBridge.SetMaterial = (nameplate, quantity, name, count) => SetMaterial(nameplate, quantity, name, count);
		UserInterfaceBridge.SetMora = (bm, count) => SetMora(bm, count);
	}

	private static void UpdateElements(Bitmap bm, string text, PictureBox pictureBox, TextBox textBox)
	{
		UpdatePictureBox(bm, pictureBox);
		UpdateTextBox(text, textBox);
	}

	private static void UpdatePictureBox(Bitmap bm, PictureBox pictureBox)
	{
		try
		{
			Bitmap clone = new Bitmap(bm.Width, bm.Height);
			using (var copy = Graphics.FromImage(clone))
			{
				copy.DrawImage(bm, 0, 0);
			}
			MethodInvoker pictureBoxAction = delegate
		{
			pictureBox.Image = clone;
			pictureBox.Refresh();
		};
			pictureBox.Invoke(pictureBoxAction);
		}
		catch (Exception e)
		{
			Logger.Debug($"Problem updating picturebox {0}\n{1}", pictureBox.Name, e);
		}
	}

	private static void UpdateTextBox(string text, TextBox textBox)
	{
		try
		{
			MethodInvoker textBoxAction = delegate
			{
				textBox.AppendText(text.Replace("\n", Environment.NewLine));
				textBox.AppendText(Environment.NewLine);
				textBox.Refresh();
			};
			textBox.Invoke(textBoxAction);
		}
		catch (Exception e)
		{
			Logger.Debug($"Problem updating picturebox {0}\n{1}", textBox.Name, e);
		}
	}

	private static void UpdateLabel(string text, Label label)
	{
		try
		{
			MethodInvoker labelAction =  delegate
		{
			label.Text = text;
			label.Refresh();
		};
			label.Invoke(labelAction);
		}
		catch (Exception e)
		{
			Logger.Debug($"Problem updating picturebox {0}\n{1}", label.Name, e);

		}
	}

	public static void SetGear(Bitmap bm, Weapon weapon)
	{
		ResetGearDisplay();
		SetGearPictureBox(bm);
		SetGearTextBox(weapon.ToString());
	}

	public static void SetGear(Bitmap bm, Artifact artifact)
	{
		ResetGearDisplay();
		SetGearPictureBox(bm);
		SetGearTextBox(artifact.ToString());
	}

	public static void SetGearPictureBox(Bitmap bm)
	{
		UpdatePictureBox(bm, gear_PictureBox);
	}

	public static void SetGearTextBox(string text)
	{
		UpdateTextBox(text, gear_TextBox);
	}

	internal static void SetMainCharacterName(string text)
	{
		UpdateTextBox($"Traveler name: {text}", character_TextBox);
	}

	public static void SetCharacter_NameAndElement(Bitmap bm, string name, string element)
	{
		UpdateElements(bm, $"Name: {name}\nElement: {element}", cName_PictureBox, character_TextBox);
	}

	public static void SetCharacter_Level(Bitmap bm, int level, int maxLevel)
	{
		UpdateElements(bm, $"Level: {level} / {maxLevel}", cLevel_PictureBox, character_TextBox);
	}

	public static void SetCharacter_Constellation(int level)
	{
		UpdateTextBox($"Constellation: {level}", character_TextBox);
	}

	internal static void SetMaterial(Bitmap nameplate, Bitmap quantity, string name, int count)
	{
		UpdateElements(nameplate, $"Name: {name}", cName_PictureBox, character_TextBox);
		UpdateElements(quantity, $"Count: {count}", cLevel_PictureBox, character_TextBox);
	}

	public static void SetMora(Bitmap mora, int count)
	{
		UpdateElements(mora, $"Mora: {count}", navigation_PictureBox, character_TextBox);
	}


	public static void SetCharacter_Talent(Bitmap bm, string text, int i)
	{
		if (i > -1 && i < 3)
		{
			UpdatePictureBox(bm, cTalent_PictureBoxes[i]);
			UpdateTextBox($"Talent {i + 1}: {text}", character_TextBox);
		}
	}

	public static void SetWeapon_Max(int value)
	{
		UpdateLabel(value.ToString(), weaponMax_Label);
		Logger.Info("Parsed {value} weapons to scan", value);
	}

	public static void SetArtifact_Max(int value)
	{
		UpdateLabel(value.ToString(), artifactMax_Label);
		Logger.Info("Parsed {value} artifacts to scan", value);
	}

	public static void IncrementArtifactCount()
	{
		lock (artifactCount_Label)
		{
			UpdateLabel($"{Int32.Parse(artifactCount_Label.Text) + 1}", artifactCount_Label);
		}
	}

	public static void IncrementWeaponCount()
	{
		lock (weaponCount_Label)
		{
			UpdateLabel($"{Int32.Parse(weaponCount_Label.Text) + 1}", weaponCount_Label);
		}
	}

	public static void IncrementCharacterCount()
	{
		lock (characterCount_Label)
		{
			UpdateLabel($"{Int32.Parse(characterCount_Label.Text) + 1}", characterCount_Label);
		}
	}

	public static void SetProgramStatus(string status, bool ok = true)
	{
		MethodInvoker statusAction = delegate
		{
			programStatus_Label.Text = status;
			programStatus_Label.ForeColor = ok ? Color.Green : Color.Red;
			programStatus_Label.Font = new Font(programStatus_Label.Font.FontFamily, 15);
			programStatus_Label.Refresh();
		};

		programStatus_Label.Invoke(statusAction);
	}

	public static void AddError(string error)
	{
		Logger.Error(error);
	}

	private static void SetupNLogUiTarget()
	{
		var target = new NLog.Targets.MethodCallTarget("ui", (logEvent, _) =>
		{
			var level = logEvent.Level.Name.ToUpper();
			var loggerName = logEvent.LoggerName ?? "";
			var lastDot = loggerName.LastIndexOf('.');
			if (lastDot >= 0) loggerName = loggerName[(lastDot + 1)..];
			AddLogEntry(level, loggerName, logEvent.FormattedMessage, logEvent.TimeStamp);
		});

		var config = NLog.LogManager.Configuration ?? new NLog.Config.LoggingConfiguration();
		config.AddTarget(target);

		var initialLevel = SettingsService.Instance.Settings.LogLevel ?? "Info";
		var minLevel = ParseLogLevel(initialLevel);
		config.AddRule(minLevel, NLog.LogLevel.Fatal, target);
		NLog.LogManager.Configuration = config;
		NLog.LogManager.ReconfigExistingLoggers();
	}

	public static void SetLogLevel(string levelName)
	{
		var minLevel = ParseLogLevel(levelName);
		var config = NLog.LogManager.Configuration;
		if (config == null) return;

		var target = config.FindTargetByName("ui");
		if (target != null)
		{
			foreach (var rule in config.LoggingRules.Where(r => r.Targets.Contains(target)).ToList())
			{
				config.LoggingRules.Remove(rule);
			}
			config.AddRule(minLevel, NLog.LogLevel.Fatal, target);
			NLog.LogManager.ReconfigExistingLoggers();
		}
	}

	private static NLog.LogLevel ParseLogLevel(string levelName)
	{
		return levelName.ToLowerInvariant() switch
		{
			"trace" => NLog.LogLevel.Trace,
			"debug" => NLog.LogLevel.Debug,
			"warn" or "warning" => NLog.LogLevel.Warn,
			"error" => NLog.LogLevel.Error,
			"fatal" => NLog.LogLevel.Fatal,
			_ => NLog.LogLevel.Info,
		};
	}

	public static void ClearLog()
	{
		if (error_TextBox == null || error_TextBox.IsDisposed) return;
		try
		{
			error_TextBox.Invoke((MethodInvoker)delegate
			{
				lock (_logLock)
				{
					error_TextBox.Clear();
				}
			});
		}
		catch { }
	}

	public static void AddLogEntry(string level, string loggerName, string message, DateTime timestamp)
	{
		if (error_TextBox == null || error_TextBox.IsDisposed) return;

		try
		{
			error_TextBox.Invoke((MethodInvoker)delegate
			{
				lock (_logLock)
				{
					if (error_TextBox.Lines.Length > MaxLogLines)
					{
						error_TextBox.Clear();
						error_TextBox.SelectionColor = Color.FromArgb(127, 140, 141);
						error_TextBox.AppendText($"--- Log trimmed at {MaxLogLines} lines ---{Environment.NewLine}");
					}

					var levelColor = level switch
					{
						"FATAL" or "ERROR" => Color.FromArgb(231, 76, 60),
						"WARN" => Color.FromArgb(243, 156, 18),
						"INFO" => Color.FromArgb(200, 210, 220),
						"DEBUG" => Color.FromArgb(52, 152, 219),
						"TRACE" => Color.FromArgb(149, 165, 166),
						_ => Color.FromArgb(127, 140, 141),
					};

					// Timestamp (dim)
					error_TextBox.SelectionStart = error_TextBox.TextLength;
					error_TextBox.SelectionLength = 0;
					error_TextBox.SelectionColor = Color.FromArgb(120, 130, 140);
					error_TextBox.AppendText($"[{timestamp:HH:mm:ss}] ");

					// Level tag (colored)
					error_TextBox.SelectionColor = levelColor;
					error_TextBox.AppendText($"{level,-5} ");

					// Logger name (dim)
					error_TextBox.SelectionColor = Color.FromArgb(120, 130, 140);
					error_TextBox.AppendText($"{loggerName} \u2502 ");

					// Message (colored by level)
					error_TextBox.SelectionColor = levelColor;
					error_TextBox.AppendText(message + Environment.NewLine);

					// Auto-scroll to bottom
					error_TextBox.SelectionStart = error_TextBox.TextLength;
					error_TextBox.ScrollToCaret();
				}
			});
		}
		catch
		{
			// Ignore — form may be closing
		}
	}

	public static void SetNavigation_Image(Bitmap bm)
	{
		UpdatePictureBox(bm, navigation_PictureBox);
	}

	public static void ResetCharacterDisplay()
	{
		MethodInvoker nameAction = delegate { cName_PictureBox.Image = null; };
		MethodInvoker levelAction = delegate { cLevel_PictureBox.Image = null; };
		MethodInvoker talentAction_1 = delegate { cTalent_PictureBoxes[0].Image = null; };
		MethodInvoker talentAction_2 = delegate { cTalent_PictureBoxes[1].Image = null; };
		MethodInvoker talentAction_3 = delegate { cTalent_PictureBoxes[2].Image = null; };
		MethodInvoker textAction = delegate { character_TextBox.Clear(); };

		cName_PictureBox.Invoke(nameAction);
		cLevel_PictureBox.Invoke(levelAction);
		cTalent_PictureBoxes[0].Invoke(talentAction_1);
		cTalent_PictureBoxes[1].Invoke(talentAction_2);
		cTalent_PictureBoxes[2].Invoke(talentAction_3);
		character_TextBox.Invoke(textAction);
	}

	public static void ResetGearDisplay()
	{
		MethodInvoker gearAction = delegate { gear_PictureBox.Image = null; };

		MethodInvoker textAction = delegate { gear_TextBox.Clear(); };

		gear_PictureBox.Invoke(gearAction);
		gear_TextBox.Invoke(textAction);
	}

	public static void ResetCounters()
	{
		MethodInvoker characterCountAction = delegate { characterCount_Label.Text = "0"; characterCount_Label.Refresh(); };
		MethodInvoker weaponCountAction = delegate { weaponCount_Label.Text = "0"; weaponCount_Label.Refresh(); };
		MethodInvoker weaponMaxAction = delegate { weaponMax_Label.Text = "?"; weaponMax_Label.Refresh(); };
		MethodInvoker artifactCountAction = delegate { artifactCount_Label.Text = "0"; artifactCount_Label.Refresh(); };
		MethodInvoker artifactMaxAction = delegate { artifactMax_Label.Text = "?"; artifactMax_Label.Refresh(); };

		characterCount_Label.Invoke(characterCountAction);
		weaponCount_Label.Invoke(weaponCountAction);
		weaponMax_Label.Invoke(weaponMaxAction);
		artifactCount_Label.Invoke(artifactCountAction);
		artifactMax_Label.Invoke(artifactMaxAction);
	}

	public static void ResetErrors()
	{
		MethodInvoker textAction = delegate { error_TextBox.Clear(); };

		error_TextBox.Invoke(textAction);
	}

	public static void ResetAll()
	{
		ResetGearDisplay();

		ResetCharacterDisplay();

		ResetCounters();

		ResetErrors();
	}
}