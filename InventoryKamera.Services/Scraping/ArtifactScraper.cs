using InventoryKamera.Helpers;
using System.Text.RegularExpressions;
using static InventoryKamera.Artifact;
using Microsoft.Extensions.Logging;

namespace InventoryKamera
{
    public partial class ArtifactScraper : InventoryScraper
    {
        [GeneratedRegex(@"[\W]")]
        private static partial Regex NonWordRegex();

        [GeneratedRegex(@"[\W_]")]
        private static partial Regex NonWordUnderscoreRegex();

        [GeneratedRegex(@"[\W_0-9]")]
        private static partial Regex NonAlphaRegex();

        [GeneratedRegex(@"\D")]
        private static partial Regex NonDigitRegex();

        [GeneratedRegex(@"(piece|set|2-)")]
        private static partial Regex SetMarkerRegex();

        [GeneratedRegex(@"^[A-Za-z\s]+:$")]
        private static partial Regex HeaderColonRegex();

        [GeneratedRegex(@"^[^a-zA-Z]*")]
        private static partial Regex LeadingNonAlphaRegex();

        [GeneratedRegex(@"^(.*?)(\d+.*)")]
        private static partial Regex SubstatSplitRegex();

        [GeneratedRegex(@"[^0-9]")]
        private static partial Regex PureDigitsRegex();

        public ArtifactScraper(ILogger<ArtifactScraper> logger) : base(logger)
        {
            inventoryPage = InventoryPage.Artifacts;
            SortByLevel = Settings.MinimumArtifactLevel > 0;
            SortByObtained = Settings.SortByObtained;
        }

        public void ScanArtifacts(int count = 0, CancellationToken cancellationToken = default)
        {
            // Get Max artifacts from screen
            int artifactCount = count == 0 ? ScanItemCount() : count;
            int page = 1;

            SetSort();
            ClearFilters();

            cancellationToken.ThrowIfCancellationRequested();

            var (rectangles, cols, rows) = GetPageOfItems(page);
            int fullPage = cols * rows;
            // lowers the artifact count if user is scanning in recently obtained pages
            artifactCount = (SortByObtained * fullPage <= artifactCount && SortByObtained > 0) ? SortByObtained * fullPage : artifactCount;
            int totalRows = (int)Math.Ceiling(artifactCount / (decimal)cols);
            int cardsQueued = 0;
            int rowsQueued = 0;
            UserInterface.SetArtifact_Max(artifactCount);

            StopScanning = false;

            _logger.LogInformation("Found {Count} for artifact count.", artifactCount);

            // Go through artifact list
            while (cardsQueued < artifactCount)
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogDebug("Scanning artifact page {Page}", page);
                _logger.LogDebug("Located {Count} possible item locations on page.", rectangles.Count);

                int cardsRemaining = artifactCount - cardsQueued;
                // Go through each "page" of items and queue. In the event that not a full page of
                // items are scrolled to, offset the index of rectangle to start clicking from
                for (int i = cardsRemaining < fullPage ? ( rows - ( totalRows - rowsQueued ) ) * cols : 0; i < rectangles.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    Rectangle item = rectangles[i];
                    Navigation.SetCursor(item.Center().X, item.Center().Y);
                    Navigation.Click();
                    Navigation.SystemWait(Navigation.Speed.SelectNextInventoryItem);

                    // Queue card for scanning
                    QueueScan(cardsQueued);
                    cardsQueued++;
                    if (cardsQueued >= artifactCount || StopScanning)
                    {
                        if (StopScanning) _logger.LogInformation("Stopping artifact scan based on filtering");
                        else _logger.LogInformation("Stopping artifact scan based on scans queued ({Queued} of {Total})", cardsQueued, artifactCount);
                        return;
                    }
                }

                _logger.LogDebug("Finished queuing page of artifacts. Scrolling...");

                rowsQueued += rows;

                cancellationToken.ThrowIfCancellationRequested();

                // Position cursor safely in the middle of the grid to ensure scroll events target the inventory list
                Navigation.SetCursor((int)(Navigation.GetWidth() * 0.35), (int)(Navigation.GetHeight() * 0.5));
                Navigation.Wait(50);

                // Page done, now scroll
                // If the number of remaining scans is shorter than a full page then
                // only scroll a few rows
                if (totalRows - rowsQueued <= rows)
                {
                    for (int i = 0; i < 10 * ( totalRows - rowsQueued ) - 1; i++)
                    {
                        Navigation.sim.Mouse.VerticalScroll(-1);
                        Navigation.SystemWait(Navigation.Speed.InventoryScroll);
                    }
                    Navigation.SystemWait(Navigation.Speed.Normal);
                }
                else
                {
                    for (int i = 0; i < 10 * rows - 1; i++)
                    {
                        Navigation.sim.Mouse.VerticalScroll(-1);
                        Navigation.SystemWait(Navigation.Speed.InventoryScroll);
                    }
                    // Scroll back one to keep it from getting too crazy
                    var rollbackPeriod = Navigation.IsNormal ? 9 : 3;
                    if (page % rollbackPeriod == 0)
                    {
                        _logger.LogDebug("Scrolled back one");
                        Navigation.sim.Mouse.VerticalScroll(1);
                        Navigation.SystemWait(Navigation.Speed.InventoryScroll);
                    }
                    Navigation.SystemWait(Navigation.Speed.Normal);
                }
                cancellationToken.ThrowIfCancellationRequested();
                ++page;
                (rectangles, cols, rows) = GetPageOfItems(page, acceptLess: totalRows - rowsQueued <= fullPage);
            }
        }

        private void ClearFilters()
        {

            using (var x = Navigation.CaptureRegion(
                x: (int)((Navigation.IsNormal ? 0.0750 : 0.0757) * Navigation.GetWidth()),
                y: (int)((Navigation.IsNormal ? 0.8522 : 0.8678) * Navigation.GetHeight()),
                width: (int)((Navigation.IsNormal ? 0.2244 : 0.2236) * Navigation.GetWidth()),
                height: (int)((Navigation.IsNormal ? 0.0422 : 0.0367) * Navigation.GetHeight())))
            {
                //Navigation.DisplayBitmap(x);
                var t = GenshinProcesor.AnalyzeText(x).Trim().ToLower();
                if (t != null && t.Contains("filter"))
                {
                    Navigation.ClearArtifactFilters();
                }
                Navigation.SystemWait(Navigation.Speed.Slow);
            }
        }

        private void SetSort()
        {
            using (var x = Navigation.CaptureRegion(
                x: (int)( 0.6250 * Navigation.GetWidth()),
                y: (int)((Navigation.IsNormal ? 0.1111 : 0.1000) * Navigation.GetHeight()),
                width: (int)( 0.0375 * Navigation.GetWidth()),
                height: (int)(0.0347 * Navigation.GetHeight())))
            {
                //Navigation.DisplayBitmap(x);	
                Color sortObtainedTrue = Color.FromArgb(255, 224, 198, 147);
                Color sortObtainedStatus = x.GetPixel((int)x.Width / 2,(int) x.Height / 2);
                var sortObtained = GenshinProcesor.CompareColors(sortObtainedTrue, sortObtainedStatus);
                bool willToggle = SortByObtained > 0 ^ sortObtained;

                // Diagnostic logging for the "sort by obtained" detection. If the scanner is
                // wrongly clicking this button, these lines reveal whether the hardcoded sample
                // region/color no longer matches the current resolution or game UI.
                _logger.LogInformation(
                    "SetSort diagnostics: resolution {W}x{H} (IsNormal={IsNormal}), SortByObtained setting={Setting}, " +
                    "sampled pixel=ARGB({A},{R},{G},{B}) expected=ARGB(255,224,198,147), detectedButtonOn={Detected}, willToggle={Toggle}",
                    Navigation.GetWidth(), Navigation.GetHeight(), Navigation.IsNormal, SortByObtained,
                    sortObtainedStatus.A, sortObtainedStatus.R, sortObtainedStatus.G, sortObtainedStatus.B,
                    sortObtained, willToggle);
                SaveInventoryBitmap(x, $"SetSort_Region_{Navigation.GetWidth()}x{Navigation.GetHeight()}.png");

                if( willToggle )
                {
                    _logger.LogInformation("SetSort toggling 'sort by obtained' button (setting requires {Setting}, detected {Detected}).", SortByObtained > 0, sortObtained);
                    Navigation.ChangeArtifactSortObtained();
                }
                Navigation.SystemWait(Navigation.Speed.Slow);
            }
        }


        public Task QueueScan(int id)
        {
            var card = GetItemCard();
            Bitmap name, gearSlot, mainStat, subStats, level, equipped, locked, sanctify;
            bool _sanctify;

            name = GetItemNameBitmap(card);
            equipped = GetEquippedBitmap(card);
            gearSlot = GetGearSlotBitmap(card);
            mainStat = GetMainStatBitmap(card);

            sanctify = GetSanctifyBitmap(card);
            Color sanctifiedColor = Color.FromArgb(255, 220, 192, 255);
            Color sanctifyStatus = sanctify.GetPixel(10, 10);
            _sanctify = GenshinProcesor.CompareColors(sanctifiedColor, sanctifyStatus);

            // may change because of sanctifying
            locked = GetLockedBitmap(card, _sanctify);
            level = GetLevelBitmap(card, _sanctify);
            subStats = GetSubstatsBitmap(card, _sanctify);

            //Navigation.DisplayBitmap(name);
            //Navigation.DisplayBitmap(locked);
            //Navigation.DisplayBitmap(equipped);
            //Navigation.DisplayBitmap(mainStat);
            //Navigation.DisplayBitmap(subStats);
            //Navigation.DisplayBitmap(level);
            //Navigation.DisplayBitmap(sanctify);

            // Separate to all pieces of artifact and add to pics
            List<Bitmap> artifactImages = new List<Bitmap>
            {
                name, //0
                gearSlot,
                mainStat,
                level,
                subStats,
                equipped, //5
                locked,
                sanctify,
                card
            };

            int artifactRarity = GetRarity(name);
            int artifactLevel = ScanArtifactLevel(level);

            _logger.LogInformation("Artifact {Id} - Rarity: {Rarity}, Level: {Level}", id, artifactRarity, artifactLevel);

            bool belowRarity = artifactRarity > 0 && artifactRarity < Settings.MinimumArtifactRarity;
            bool belowLevel = artifactLevel >= 0 && artifactLevel < Settings.MinimumArtifactLevel;
            StopScanning = (SortByLevel && belowLevel) || (!SortByLevel && belowRarity);

            if (StopScanning || belowRarity || belowLevel)
            {
                _logger.LogInformation("Skipping Artifact {Id}. StopScanning: {StopScanning}, BelowRarity: {BelowRarity}, BelowLevel: {BelowLevel}", id, StopScanning, belowRarity, belowLevel);
                artifactImages.ForEach(i => i.Dispose());
                return Task.CompletedTask;
            }
            // Send images to Worker Queue
            InventoryKamera.workerQueue.Enqueue(new OCRImageCollection(artifactImages, "artifact", id));
            return Task.CompletedTask;
        }

        private Bitmap GetSubstatsBitmap(Bitmap card, bool isSanctified = false)
        {
            double baseY = Navigation.IsNormal ? 0.4216 : 0.3682;
            double sanctifiedShift = Navigation.IsNormal ? 0.0520 : 0.0471;
            double yShift = isSanctified ? sanctifiedShift : 0.0;

            return GenshinProcesor.CopyBitmap(card,new Rectangle(
                x:(int)(card.Width * 0.0605),
                y:(int)(card.Height * (baseY + yShift)),
                width:(int)(card.Width * 0.8297),
                height:(int)(card.Height * (Navigation.IsNormal ? 0.2301 : 0.1573))));
        }

        private Bitmap GetMainStatBitmap(Bitmap card)
        {
            return GenshinProcesor.CopyBitmap(card, new Rectangle(
                x: (int)(card.Width * 0.0405),
                y: (int)(card.Height * (Navigation.IsNormal ? 0.1722 : 0.1477)),
                width: (int)(card.Width * 0.4555),
                height: (int)(card.Height * (Navigation.IsNormal ? 0.0416 : 0.0416))));
        }

        private Bitmap GetLevelBitmap(Bitmap card, bool isSanctified = false)
        {
            double baseY = Navigation.IsNormal ? 0.3634 : 0.3197;
            double sanctifiedShift = Navigation.IsNormal ? 0.0520 : 0.0465;
            double yShift = isSanctified ? sanctifiedShift : 0.0;

            return GenshinProcesor.CopyBitmap(card, new Rectangle(
                x: (int)(card.Width * 0.0506),
                y: (int)(card.Height * (baseY + yShift)),
                width: (int)(card.Width * 0.1417),
                height: (int)(card.Height * (Navigation.IsNormal ? 0.0416 : 0.0347))));
        }

        private Bitmap GetSanctifyBitmap(Bitmap card)
        {
            return GenshinProcesor.CopyBitmap(card, new Rectangle(
                x: (int)(card.Width * 0.0),
                y: (int)(card.Height * (Navigation.IsNormal ? 0.3333 : 0.2941)),
                width: (int)(card.Width * 0.0606),
                height: (int)(card.Height * (Navigation.IsNormal ? 0.0526 : 0.0470))));
        }

        private Bitmap GetGearSlotBitmap(Bitmap card)
        {
            return GenshinProcesor.CopyBitmap(card, new Rectangle(
                x: (int)(card.Width * 0.0405),
                y: (int)(card.Height * (Navigation.IsNormal ? 0.07720 : 0.0663)),
                width: (int)(card.Width * 0.4757),
                height: (int)(card.Height * (Navigation.IsNormal ? 0.0475 : 0.0809))));
        }

        public async Task<Artifact> CatalogueFromBitmapsAsync(List<Bitmap> bm, int id)
        {
            // Init Variables
            string gearSlot = null;
            string mainStat = null;
            string setName = null;
            string equippedCharacter = null;
            List<SubStat> subStats = new List<SubStat>();
            List<SubStat> unactivatedSubStats = new List<SubStat>();
            int rarity = 0;
            int level = 0;
            bool _lock = false;

            if (bm.Count >= 6)
            {
                int a_name = 0; int a_gearSlot = 1; int a_mainStat = 2; int a_level = 3; int a_subStats = 4; int a_equippedCharacter = 5; int a_lock = 6; 
                // Get Rarity
                rarity = GetRarity(bm[a_name]);

                // Check for equipped color
                Color equippedColor = Color.FromArgb(255, 255, 231, 187);
                Color equippedStatus = bm[a_equippedCharacter].GetPixel(5, 5);
                bool b_equipped = GenshinProcesor.CompareColors(equippedColor, equippedStatus);

                // Check for lock color
                Color lockedColor = Color.FromArgb(255, 70, 80, 100); // Dark area around red lock
                Color lockStatus = bm[a_lock].GetPixel(10, 10);
                _lock = GenshinProcesor.CompareColors(lockedColor, lockStatus);

                // Improved Scanning using multi threading
                List<Task> tasks = new List<Task>();

                var taskGear  = Task.Run(() => gearSlot = ScanArtifactGearSlot(bm[a_gearSlot]));
                var taskMain  = taskGear.ContinueWith( (antecedent) => mainStat = ScanArtifactMainStat(bm[a_mainStat], antecedent.Result));
                var taskLevel = Task.Run(() => level = ScanArtifactLevel(bm[a_level]));
                var taskSubs  = Task.Run(() => (subStats, unactivatedSubStats) = ScanArtifactSubStats(bm[a_subStats]));
                var taskEquip = Task.Run(() => equippedCharacter = ScanArtifactEquippedCharacter(bm[a_equippedCharacter]));
                var taskName = Task.Run(() => setName = ScanArtifactSet(bm[a_name]));

                tasks.Add(taskGear);
                tasks.Add(taskMain);
                tasks.Add(taskLevel);
                tasks.Add(taskSubs);
                tasks.Add(taskName);
                if (b_equipped)
                {
                    tasks.Add(taskEquip);
                }

                await Task.WhenAll(tasks.ToArray());
            }
            return new Artifact(setName, rarity, level, gearSlot, mainStat, subStats, unactivatedSubStats, equippedCharacter, id, _lock);
        }

        private static int GetRarity(Bitmap bm)
        {
            var averageColor = BitmapHelper.GetAverageColor(bm);

            Color fiveStar = Color.FromArgb(255, 188, 105, 50);
            Color fourStar = Color.FromArgb(255, 161, 86, 224);
            Color threeStar = Color.FromArgb(255, 81, 127, 203);
            Color twoStar = Color.FromArgb(255, 42, 143, 114);
            Color oneStar = Color.FromArgb(255, 114, 119, 138);

            var colors = new List<Color> { Color.Black, oneStar, twoStar, threeStar, fourStar, fiveStar };

            var c = GenshinProcesor.ClosestColor(colors, averageColor);

            return colors.IndexOf(c);
        }

        public bool IsEnhancementMaterial(Bitmap card)
        {
            RECT reference = Navigation.GetAspectRatio() == new Size(16, 9) ?
                new RECT(new Rectangle(862, 80, 327, 560)) : (RECT)new Rectangle(862, 80, 328, 640);
            Bitmap nameBitmap = card.Clone(new RECT(
                Left: 0,
                Top: 0,
                Right: card.Width,
                Bottom: (int)( 38.0 / reference.Height * card.Height )), card.PixelFormat);
            string material = ScanEnhancementMaterialName(nameBitmap);
            return !string.IsNullOrWhiteSpace(material) && GenshinProcesor.enhancementMaterials.Contains(material.ToLower());
        }

        private static string ScanEnhancementMaterialName(Bitmap bm)
        {
            using Bitmap copy = (Bitmap)bm.Clone();
            GenshinProcesor.SetGamma(0.2, 0.2, 0.2, copy);
            using Bitmap n = GenshinProcesor.ConvertToGrayscale(copy);
            GenshinProcesor.SetInvert(n);

            // Analyze
            string name = NonWordRegex().Replace(GenshinProcesor.AnalyzeText(n).ToLower(), string.Empty);
            return GenshinProcesor.FindClosestMaterialName(name);
        }

        #region Task Methods

        private static string ScanArtifactGearSlot(Bitmap bm)
        {
            // Process Img
            using Bitmap n = GenshinProcesor.ConvertToGrayscale(bm);
            GenshinProcesor.SetContrast(80.0, n);
            GenshinProcesor.SetInvert(n);

            string gearSlot = GenshinProcesor.AnalyzeText(n).Trim().ToLower();
            gearSlot = NonWordUnderscoreRegex().Replace(gearSlot, string.Empty);
            return GenshinProcesor.FindClosestGearSlot(gearSlot);
        }

        private static string ScanArtifactMainStat(Bitmap bm, string gearSlot)
        {
            switch (gearSlot)
            {
                // Flower of Life. Flat HP
                case "flower":
                    return GenshinProcesor.Stats["hp"];

                // Plume of Death. Flat ATK
                case "plume":
                    return GenshinProcesor.Stats["atk"];

                // Otherwise it's either sands, goblet or circlet.
                default:
                {
                    using Bitmap copy = (Bitmap)bm.Clone();
                    GenshinProcesor.SetContrast(100.0, copy);
                    using Bitmap n = GenshinProcesor.ConvertToGrayscale(copy);
                    
                    GenshinProcesor.SetThreshold(135, n);
                    GenshinProcesor.SetInvert(n);

                    // Get Main Stat
                    string mainStat = GenshinProcesor.AnalyzeText(n).ToLower().Trim();

                    // Remove anything not a-z as well as removes spaces/underscores
                    mainStat = NonAlphaRegex().Replace(mainStat, string.Empty);

                    mainStat = GenshinProcesor.FindClosestStat(mainStat, 80);

                    if (mainStat == "def" || mainStat == "atk" || mainStat == "hp")
                    {
                        mainStat += "_";
                    }
                    return mainStat;
                }
            }
        }

        private static int ScanArtifactLevel(Bitmap bm)
        {
            // Process Img
            using Bitmap n = GenshinProcesor.ConvertToGrayscale(bm);
            GenshinProcesor.SetContrast(80.0, n);
            GenshinProcesor.SetInvert(n);

            // numbersOnly = true => seems to interpret the '+' as a '4'
            string text = GenshinProcesor.AnalyzeText(n, Tesseract.PageSegMode.SingleWord).Trim().ToLower();

            // Get rid of all non digits
            text = NonDigitRegex().Replace(text, string.Empty);

            return int.TryParse(text, out int level) ? level : -1;
        }

        private (List<SubStat> active, List<SubStat> unactivated) ScanArtifactSubStats(Bitmap artifactImage)
        {
            using Bitmap bm = (Bitmap)artifactImage.Clone();
            List<string> lines = new List<string>();
            List<SubStat> substats = new List<SubStat>();
            List<SubStat> unactivated = new List<SubStat>();
            string text;
            bool hasUnactivated = false;

            GenshinProcesor.SetBrightness(-30, bm);
            GenshinProcesor.SetContrast(85, bm);
            using (var n = GenshinProcesor.ConvertToGrayscale(bm))
            {
                text = GenshinProcesor.AnalyzeText(n, Tesseract.PageSegMode.Auto).ToLower();
            }

            if (text.Contains("(unactivated)"))
            {
                hasUnactivated = true;
            }

            lines = new List<string>(text.Split('\n'));
            lines.RemoveAll(line => string.IsNullOrWhiteSpace(line));

            var index = lines.FindIndex(line =>
                SetMarkerRegex().IsMatch(line) ||
                HeaderColonRegex().IsMatch(line.Trim())
            );
            if (index >= 0)
            {
                lines.RemoveRange(index, lines.Count - index);
            }

            for (int i = 0; i < lines.Count; i++)
            {
                var line = LeadingNonAlphaRegex().Replace(lines[i], string.Empty).Replace(" ", string.Empty);

                if (line.Any(char.IsDigit))
                {
                    _logger.LogDebug("Parsing artifact substat: {Line}", line);

                    SubStat substat = new SubStat();
                    var result = SubstatSplitRegex().Match(line);

                    // Without a stat/value split there is nothing usable on this line;
                    // skip it rather than inserting a blank, invalid substat.
                    if (!result.Success)
                    {
                        _logger.LogDebug("No stat/value match on substat line: {Line}", line);
                        continue;
                    }

                    var stat = NonWordRegex().Replace(result.Groups[1].Value, string.Empty);
                    var value = result.Groups[2].Value;

                    string name = line.Contains("%") ? stat + "%" : stat;

                    substat.stat = GenshinProcesor.FindClosestStat(name, 80) ?? "";

                    // Remove any non digits.
                    value = PureDigitsRegex().Replace(value, string.Empty);

                    // Try to parse number
                    if (!decimal.TryParse(value, out substat.value))
                    {
                        _logger.LogDebug("Failed to parse stat value from: {Line}", line);
                        substat.value = -1;
                    }

                    if (substat.value != -1 && substat.stat.Contains("_"))
                    {
                        substat.value /= 10;
                    }

                    if (string.IsNullOrWhiteSpace(substat.stat) || substat.value == -1)
                    {
                        _logger.LogDebug("Failed to parse stat from: {Line}", line);
                        continue;
                    }

                    substats.Add(substat);
                }
            }

            if (substats.Count == 0)
            {
                _logger.LogDebug("Failed to obtain substats");
            }

            // If there's an unactivated substat, move the last one to the unactivated list
            if (hasUnactivated && substats.Count > 0)
            {
                SubStat lastSubstat = substats[substats.Count - 1];
                unactivated.Insert(0, lastSubstat);
                substats.Remove(lastSubstat);
            }

            return (substats, unactivated);
        }

        private static string ScanArtifactEquippedCharacter(Bitmap bm)
        {
            using Bitmap n = GenshinProcesor.ConvertToGrayscale(bm);
            GenshinProcesor.SetContrast(60.0, n);

            string equippedCharacter = GenshinProcesor.AnalyzeText(n).ToLower();

            if (!string.IsNullOrEmpty(equippedCharacter))
            {
                if (equippedCharacter.Contains("equipped") && equippedCharacter.Contains(":"))
                {
                    equippedCharacter = NonWordRegex().Replace(equippedCharacter.Split(':')[1], string.Empty);
                    return GenshinProcesor.FindClosestCharacterName(equippedCharacter);
                }
            }
            // artifact has no equipped character
            return null;
        }

        private static string ScanArtifactSet(Bitmap itemName)
        {
            using Bitmap copy = (Bitmap)itemName.Clone();
            GenshinProcesor.SetGamma(0.2, 0.2, 0.2, copy);
            using Bitmap grayscale = GenshinProcesor.ConvertToGrayscale(copy);
            GenshinProcesor.SetInvert(grayscale);

            var scannedText = GenshinProcesor.AnalyzeText(grayscale, Tesseract.PageSegMode.Auto).ToLower().Replace("\n", " ");
            string text = NonWordRegex().Replace(scannedText, string.Empty);
            return GenshinProcesor.FindClosestArtifactSetFromArtifactName(text);
        }

        #endregion Task Methods
    }
}
