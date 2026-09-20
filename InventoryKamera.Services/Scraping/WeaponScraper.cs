using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace InventoryKamera
{
    public partial class WeaponScraper : InventoryScraper
    {
        [GeneratedRegex(@"(?![\d/]).")]
        private static partial Regex DigitsOrSlashRegex();

        [GeneratedRegex(@"\D")]
        private static partial Regex NonDigitRegex();

        [GeneratedRegex(@"[\W]")]
        private static partial Regex NonWordRegex();

		public WeaponScraper(ILogger<WeaponScraper> logger) : base(logger)
        {
            inventoryPage = InventoryPage.Weapons;
            SortByLevel = Settings.MinimumWeaponLevel > 1;
        }

        public void ScanWeapons(int count = 0, CancellationToken cancellationToken = default)
        {
            // Determine maximum number of weapons to scan
            int weaponCount = count == 0 ? ScanItemCount() : count;
            int page = 0;

            cancellationToken.ThrowIfCancellationRequested();

            var (rectangles, cols, rows) = GetPageOfItems(page);
            int fullPage = cols * rows;
            int totalRows = (int)Math.Ceiling(weaponCount / (decimal)cols);
            int cardsQueued = 0;
            int rowsQueued = 0;
            int offset = 0;
            UserInterface.SetWeapon_Max(weaponCount);

            // Determine Delay if delay has not been found before
            // Scraper.FindDelay(rectangles);

            StopScanning = false;

            _logger.LogInformation("Found {WeaponCount} for weapon count.", weaponCount);

            SelectSortingMethod();

            // Go through weapon list
            while (cardsQueued < weaponCount)
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogDebug("Scanning weapon page {Page}", page);
                _logger.LogDebug("Located {Count} possible item locations on page.", rectangles.Count);

                int cardsRemaining = weaponCount - cardsQueued;
                // Go through each "page" of items and queue. In the event that not a full page of
                // items are scrolled to, offset the index of rectangle to start clicking from
                for (int i = cardsRemaining < fullPage ? (rows - (totalRows - rowsQueued)) * cols : 0; i < rectangles.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    Rectangle item = rectangles[i];
                    Navigation.SelectItemAdaptive(item, offset, cancellationToken);

                    // Queue card for scanning
                    QueueScan(cardsQueued);
                    cardsQueued++;
                    if (cardsQueued >= weaponCount || this.StopScanning)
                    {
                        if (StopScanning) _logger.LogInformation("Stopping weapon scan based on filtering");
                        else _logger.LogInformation("Stopping weapon scan based on scans queued ({Queued} of {Total})", cardsQueued, weaponCount);
                        return;
                    }
                }
                _logger.LogDebug("Finished queuing page of weapons. Scrolling...");

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
                    if (Navigation.GetAspectRatio() == new Size(8, 5))
                    {
                        offset = 35; // Lazy fix
                    }
                    for (int i = 0; i < 10 * (totalRows - rowsQueued) - 1; i++)
                    {
                        Navigation.sim.Mouse.VerticalScroll(-1);
                        Navigation.SystemWait(Navigation.Speed.InventoryScroll);
                    }
                    Navigation.SystemWait(Navigation.Speed.Normal);
                }
                else
                {
                    // Scroll back one to keep it from getting too crazy
                    if (rowsQueued % 15 == 0)
                    {
                        Navigation.sim.Mouse.VerticalScroll(1);
                        Navigation.SystemWait(Navigation.Speed.InventoryScroll);
                    }
                    for (int i = 0; i < 10 * rows - 1; i++)
                    {
                        Navigation.sim.Mouse.VerticalScroll(-1);
                        Navigation.SystemWait(Navigation.Speed.InventoryScroll);
                    }
                    Navigation.SystemWait(Navigation.Speed.Normal);
                }
                cancellationToken.ThrowIfCancellationRequested();
                ++page;
                (rectangles, cols, rows) = GetPageOfItems(page, acceptLess: totalRows - rowsQueued <= fullPage);
            }

            void SelectLevelSorting()
            {
                Navigation.SetCursor(
                    X: (int)(230 / 1280.0 * Navigation.GetWidth()),
                    Y: (int)(680 / 720.0 * Navigation.GetHeight()));
                Navigation.Click();
                Navigation.Wait();
                Navigation.SetCursor(
                    X: (int)(250 / 1280.0 * Navigation.GetWidth()),
                    Y: (int)(575 / 720.0 * Navigation.GetHeight()));
                Navigation.Click();
                Navigation.Wait();
            }

            void SelectQualitySorting()
            {
                Navigation.SetCursor(
                                        X: (int)(230 / 1280.0 * Navigation.GetWidth()),
                                        Y: (int)(680 / 720.0 * Navigation.GetHeight()));
                Navigation.Click();
                Navigation.Wait();
                Navigation.SetCursor(
                    X: (int)(250 / 1280.0 * Navigation.GetWidth()),
                    Y: (int)(615 / 720.0 * Navigation.GetHeight()));
                Navigation.Click();
                Navigation.Wait();
            }

            void SelectSortingMethod()
            {
                if (SortByLevel)
                {
                    _logger.LogDebug("Sorting by level to optimize scan time.");
                    // Check if sorted by level
                    if (CurrentSortingMethod() != "level")
                    {
                        _logger.LogDebug("Not already sorting by level...");
                        // If not, sort by level
                        SelectLevelSorting();
                    }
                    _logger.LogDebug("Inventory is sorted by level.");
                }
                else
                {
                    _logger.LogDebug("Sorting by quality to optimize scan time.");
                    // Check if sorted by quality
                    if (CurrentSortingMethod() != "quality")
                    {
                        _logger.LogDebug("Not already sorting by quality...");
                        // If not, sort by quality
                        SelectQualitySorting();
                    }
                    _logger.LogDebug("Inventory is sorted by quality");
                }
            }
        }

        private void QueueScan(int id)
        {
			var card = GetItemCard();

            Bitmap name, level, refinement, equipped, locked;
            name = GetItemNameBitmap(card);
            locked = GetLockedBitmap(card);
            equipped = GetEquippedBitmap(card);
            level = GetLevelBitmap(card);
            refinement = GetRefinementBitmap(card);

            //Navigation.DisplayBitmap(name);
            //Navigation.DisplayBitmap(locked);
            //Navigation.DisplayBitmap(equipped);
            //Navigation.DisplayBitmap(level);
            //Navigation.DisplayBitmap(refinement);

            // Separate to all pieces of card
            List<Bitmap> weaponImages = new List<Bitmap>
            {
                name, //0
                level,
                refinement,
                locked,
                equipped,
                card //5
            };

            bool a = false;

            int weaponRarity = GetQuality(name);
            int weaponLevel = ScanLevel(level, ref a);

            _logger.LogDebug("Queuing weapon #{Id}: Initial Rarity={Rarity}★, Level={Level}, Ascended={Ascended}", 
                id, weaponRarity, weaponLevel, a);

            bool belowRarity = weaponRarity < Settings.MinimumWeaponRarity;
            bool belowLevel = weaponLevel < Settings.MinimumWeaponLevel;
            StopScanning = (SortByLevel && belowLevel) || (!SortByLevel && belowRarity);

            if (StopScanning || belowRarity || belowLevel)
            {
                _logger.LogDebug("Skipping weapon #{Id}. StopScanning: {StopScanning}, BelowRarity: {BelowRarity} (min {MinRarity}), BelowLevel: {BelowLevel} (min {MinLevel})",
                    id, StopScanning, belowRarity, Settings.MinimumWeaponRarity, belowLevel, Settings.MinimumWeaponLevel);
                weaponImages.ForEach(i => i.Dispose());
                return;
            }

            // Send images to worker queue
            InventoryKamera.Enqueue(new OCRImageCollection(weaponImages, "weapon", id));
        }

        Bitmap GetLevelBitmap(Bitmap card)
        {
            return GenshinProcesor.CopyBitmap(card,
                new Rectangle(
                    x: (int)(card.Width * 0.060),
                    y: (int)(card.Height * (Navigation.IsNormal ? 0.367 : 0.320)),
                    width: (int)(card.Width * 0.262),
                    height: (int)(card.Height * (Navigation.IsNormal ? 0.035 : 0.033))));
        }

        Bitmap GetRefinementBitmap(Bitmap card)
        {
            return GenshinProcesor.CopyBitmap(card,
                new Rectangle(
                    x: (int)(card.Width * (Navigation.IsNormal ? 0.058 : 0.057)),
                    y: (int)(card.Height * (Navigation.IsNormal ? 0.417 : 0.364)),
                    width: (int)(card.Width * (Navigation.IsNormal ? 0.074 : 0.075)),
                    height: (int)(card.Height * (Navigation.IsNormal ? 0.038 : 0.034))));
        }

        public async Task<Weapon> CatalogueFromBitmapsAsync(List<Bitmap> bm, int id)
		{
			// Init Variables
			string name = null;
			int level = -1;
			bool ascended = false;
			int refinementLevel = -1;
			bool locked = false;
			string equippedCharacter = null;
			int rarity = 0;

			if (bm.Count >= 4)
			{
				int w_name = 0; int w_level = 1; int w_refinement = 2; int w_lock = 3; int w_equippedCharacter = 4;

				// Check for Rarity
				rarity = GetQuality(bm[w_name]);
				_logger.LogDebug("Weapon #{Id} detected rarity: {Rarity}★", id, rarity);

				// Check for equipped color
				Color equippedColor = Color.FromArgb(255, 255, 231, 187);
				Color equippedStatus = bm[w_equippedCharacter].GetPixel(5, 5);
				bool b_equipped = GenshinProcesor.CompareColors(equippedColor, equippedStatus);

				// Check for lock color
				Color lockedColor = Color.FromArgb(255, 70, 80, 100); // Dark area around red lock
				Color lockStatus = bm[w_lock].GetPixel(10, 10);
				locked = GenshinProcesor.CompareColors(lockedColor, lockStatus);
				_logger.LogDebug("Weapon #{Id} status: Equipped={Equipped}, Locked={Locked}", id, b_equipped, locked);

				List<Task> tasks = new List<Task>();

				var taskName = Task.Run(() =>
				{
					name = ScanWeaponName(ScanItemName(bm[w_name]));
				});
				var taskLevel = Task.Run(() => level = ScanLevel(bm[w_level], ref ascended));
				var taskRefinement = Task.Run(() => refinementLevel = ScanRefinement(bm[w_refinement]));
				var taskEquipped = Task.Run(() => equippedCharacter = ScanEquippedCharacter(bm[w_equippedCharacter]));

				tasks.Add(taskName);
				tasks.Add(taskLevel);
				tasks.Add(taskRefinement);

				if (b_equipped)
				{
					tasks.Add(taskEquipped);
				}

				await Task.WhenAll(tasks.ToArray());

				_logger.LogDebug("Weapon #{Id} catalogued: Name='{Name}', Rarity={Rarity}★, Level={Level}, Ascended={Ascended}, Refinement=R{Refinement}, Equipped='{Equipped}'",
					id, name, rarity, level, ascended, refinementLevel, equippedCharacter ?? "None");
			}
			return new Weapon(name, level, ascended, refinementLevel, locked, equippedCharacter, id, rarity);
		}

        public bool IsEnhancementMaterial(Bitmap nameBitmap)
		{
			string material = ScanEnchancementOreName(nameBitmap);
			bool isOre = !string.IsNullOrWhiteSpace(material) && GenshinProcesor.enhancementMaterials.Contains(material.ToLower());
			if (isOre)
			{
				_logger.LogDebug("Detected enhancement material ore: '{Material}'", material);
			}
			return isOre;
		}

		public string ScanEnchancementOreName(Bitmap bm)
		{
			// Analyze
			string name = GenshinProcesor.FindClosestMaterialName(ScanItemName(bm), minConfidence: 95);

			return name;
		}

        #region Task Methods

		private string ScanWeaponName(string name)
        {
            var matched = GenshinProcesor.FindClosestWeapon(name);
            _logger.LogDebug("Weapon name parsed: OCR='{Raw}' -> Matched='{Matched}'", name, matched);
            return matched;
        }

        public int ScanLevel(Bitmap bm, ref bool ascended)
		{
			using Bitmap n = GenshinProcesor.ConvertToGrayscale(bm);
			GenshinProcesor.SetInvert(n);

			string text = GenshinProcesor.AnalyzeText(n).Trim();
			text = DigitsOrSlashRegex().Replace(text, string.Empty);
			_logger.LogDebug("Weapon level OCR text: '{Text}'", text);

			if (text.Contains('/'))
			{
				string[] temp = text.Split(new[] { '/' }, 2);

				if (temp.Length == 2)
				{
					if (int.TryParse(temp[0], out int level) && int.TryParse(temp[1], out int maxLevel))
					{
						maxLevel = (int)Math.Round(maxLevel / 10.0, MidpointRounding.AwayFromZero) * 10;
						ascended = 20 <= level && level < maxLevel;
						return level;
					}
				}
			}
			return -1;
		}

		public int ScanRefinement(Bitmap image)
		{
			for (double factor = 1; factor <= 2; factor += 0.1)
			{
				using (Bitmap up = GenshinProcesor.ScaleImage(image, factor))
				using (Bitmap n = GenshinProcesor.ConvertToGrayscale(up))
				{
					GenshinProcesor.SetInvert(n);

					string text = GenshinProcesor.AnalyzeText(n).Trim();
					text = NonDigitRegex().Replace(text, string.Empty);

					// Parse Int
					if (int.TryParse(text, out int refinementLevel) && 1 <= refinementLevel && refinementLevel <= 5)
					{
						_logger.LogDebug("Weapon refinement parsed as R{Refinement} (scale {Factor:F1})", refinementLevel, factor);
						return refinementLevel;
					}
				}
			}
			_logger.LogDebug("Failed to determine weapon refinement, defaulting to -1");
			return -1;
		}

		public string ScanEquippedCharacter(Bitmap bm)
		{
			using Bitmap n = GenshinProcesor.ConvertToGrayscale(bm);
			GenshinProcesor.SetContrast(60.0, n);

			string extractedString = GenshinProcesor.AnalyzeText(n);

			if (!string.IsNullOrEmpty(extractedString))
			{
				if (extractedString.Contains("Equipped:", StringComparison.OrdinalIgnoreCase))
				{
					var name = extractedString.Split(':')[1];

					name = NonWordRegex().Replace(name, string.Empty).ToLower();
					var character = GenshinProcesor.FindClosestCharacterName(name);
					_logger.LogDebug("Weapon equipped character parsed: '{Character}'", character);
					return character;
				}
			}
			// weapon has no equipped character
			_logger.LogDebug("Weapon equipped character: None");
			return null;
		}

		#endregion Task Methods
	}
}