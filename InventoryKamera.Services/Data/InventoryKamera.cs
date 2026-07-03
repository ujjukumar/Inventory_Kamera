using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using InventoryKamera.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InventoryKamera
{
    public class InventoryKamera
	{

		private readonly ILogger<InventoryKamera> _logger;
		private AppSettings Settings => SettingsService.Instance.Settings;
		private CancellationToken _cancellationToken;

		[JsonProperty]
		public List<Character> Characters;

		[JsonProperty]
		public Inventory Inventory;

		private List<Artifact> equippedArtifacts;
		private List<Weapon> equippedWeapons;
		public static Queue<OCRImageCollection> workerQueue;
		private List<Thread> ImageProcessors;

		private WeaponScraper weaponScraper;
		private ArtifactScraper artifactScraper;
		private CharacterScraper characterScraper;
		private MaterialScraper materialScraper;

		private volatile bool b_threadCancel;
		private readonly int NumWorkers;

		public bool HasData
        {
			get { return Characters.Count > 0 || Inventory.Size > 0; }
        }

		public InventoryKamera(ILogger<InventoryKamera> logger,
                               WeaponScraper weaponScraper,
                               ArtifactScraper artifactScraper,
                               CharacterScraper characterScraper,
                               MaterialScraper materialScraper)
		{
			_logger = logger;
			Characters = new List<Character>();
			Inventory = new Inventory();
			equippedArtifacts = new List<Artifact>();
			equippedWeapons = new List<Weapon>();
			ImageProcessors = new List<Thread>();
			workerQueue = new Queue<OCRImageCollection>();

			this.weaponScraper = weaponScraper;
			this.artifactScraper = artifactScraper;
			this.characterScraper = characterScraper;
			this.materialScraper = materialScraper;

			b_threadCancel = false;

            switch (Settings.ScannerDelay)
            {
				case 0:
					NumWorkers = 3;
					break;
				default:
					NumWorkers = 2;
					break;
            }
			_logger.LogInformation("Kamera initialized");
		}

		public void ResetLogging()
		{
			try
			{
				if (Directory.Exists("./logging/weapons"))
					Directory.Delete("./logging/weapons", true);
				if (Directory.Exists("./logging/artifacts"))
					Directory.Delete("./logging/artifacts", true);
				if (Directory.Exists("./logging/characters"))
					Directory.Delete("./logging/characters", true);
				if (Directory.Exists("./logging/materials"))
					Directory.Delete("./logging/materials", true);
			}
			catch (IOException ex)
			{
				_logger.LogWarning(ex, "Failed to delete existing logging directories");
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogWarning(ex, "Access denied when deleting logging directories");
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Unexpected error deleting logging directories");
			}

			try
			{
				Directory.CreateDirectory("./logging/weapons");
				Directory.CreateDirectory("./logging/artifacts");
				Directory.CreateDirectory("./logging/characters");
				Directory.CreateDirectory("./logging/materials");

				_logger.LogInformation("Logging directory reset");
			}
			catch (IOException ex)
			{
				_logger.LogError(ex, "Failed to create logging directories");
			}
			catch (UnauthorizedAccessException ex)
			{
				_logger.LogError(ex, "Access denied when creating logging directories");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error creating logging directories");
			}
		}

		public void StopImageProcessorWorkers()
		{
			b_threadCancel = true;
			AwaitProcessors();
			workerQueue = new Queue<OCRImageCollection>();
		}

		public void GatherData(CancellationToken cancellationToken = default)
		{
			_cancellationToken = cancellationToken;

			ResetLogging();

			GenshinProcesor.ReloadData();

			// Initize Image Processors
			for (int i = 0; i < NumWorkers; i++)
			{
				Thread processor = new Thread(ImageProcessorWorker){ IsBackground = true };
				processor.Start();
				ImageProcessors.Add(processor);
			}
			_logger.LogDebug("Added {Count} workers", ImageProcessors.Count);

			GenshinProcesor.RestartEngines();


			// Assign Traveler's custom name
			GenshinProcesor.AssignTravelerName(Settings.TravelerName);

            // Assign Wanderer's custom name
            GenshinProcesor.UpdateCharacterName("wanderer", Settings.WandererName);

			try
			{
                GenshinProcesor.UpdateCharacterName("manequin1", Settings.Manequin1Name);
                GenshinProcesor.UpdateCharacterName("manequin2", Settings.Manequin2Name);
            }
			catch(Exception)
			{
				// Source - https://stackoverflow.com/questions/33081102/how-to-add-a-new-object-to-an-existing-json-file
				// Posted by Alex, modified by community. See post 'Timeline' for change history
				// Retrieved 2025-11-07, License - CC BY-SA 3.0

				//load from file

				var path = $"./inventorylists/characters.json";

				var initialJson = File.ReadAllText(path, Encoding.Default);

				var jsonToOutput = initialJson.Remove(initialJson.Length - 3, 3);

				#region manequins
				//"manequin1": {
				//  "GOOD": "Manequin1",
				//	"ConstellationName": [
				//		"support for manequins to ommit them during scan, as of writing it (6th November 2025) GO doesn't support manequins"
				//	],
				//	"ConstellationOrder": [
				//		"burst",
				//		"skill"
				//	],
				//	"Element": [
				//		"electro",
				//		"pyro",
				//		"dendro",
				//		"geo",
				//		"hydro",
				//		"anemo"
				//	],
				//	"WeaponType": 0
				//},
				//"manequin2": {
				//	"GOOD": "Manequin2",
				//	"ConstellationName": [
				//		"support for manequins to ommit them during scan, as of writing it (6th November 2025) GO doesn't support manequins"
				//	],
				//	"ConstellationOrder": [
				//		"burst",
				//		"skill"
				//	],
				//	"Element": [
				//		"electro",
				//		"pyro",
				//		"dendro",
				//		"geo",
				//		"hydro",
				//		"anemo"
				//	],
				//	"WeaponType": 0
				//}
				#endregion


				var manequinData = ",\n" +
								" \"manequin1\":{\n" +
								"  \"GOOD\":\"Manequin1\",\n" +
								"  \"ConstellationName\":[\n" +
								"   \"support for manequins to ommit them during scan, as of writing it (6th November 2025) GO doesn't support manequins\"\n" +
								"  ],\n" +
                                "  \"ConstellationOrder\":[\n" +
                                "   \"burst\",\n" +
                                "   \"skills\"\n" +
                                "  ],\n" +
                                "  \"Element\":[\n" +
                                "   \"electro\",\n" +
                                "   \"pyro\",\n" +
                                "   \"dendro\",\n" +
                                "   \"geo\",\n" +
                                "   \"hydro\",\n" +
                                "   \"anemo\"\n" +
								"  ],\n" +
								"  \"WeaponType\": 0" +
								" },\n" +
                                " \"manequin2\":{\n" +
                                "  \"GOOD\":\"Manequin2\",\n" +
                                "  \"ConstellationName\":[\n" +
                                "   \"support for manequins to ommit them during scan, as of writing it (6th November 2025) GO doesn't support manequins\"\n" +
                                "  ],\n" +
                                "  \"ConstellationOrder\":[\n" +
                                "   \"burst\",\n" +
                                "   \"skills\"\n" +
                                "  ],\n" +
                                "  \"Element\":[\n" +
                                "   \"electro\",\n" +
                                "   \"pyro\",\n" +
                                "   \"dendro\",\n" +
                                "   \"geo\",\n" +
                                "   \"hydro\",\n" +
                                "   \"anemo\"\n" +
                                "  ],\n" +
                                "  \"WeaponType\": 0" +
                                " }\n" +
                                "}";
				jsonToOutput += manequinData;
                //save to file here
                File.WriteAllText(path, jsonToOutput);

                GatherData(_cancellationToken);
			}

			// Check for cancellation before starting scans
			_cancellationToken.ThrowIfCancellationRequested();

            if (Settings.ScanWeapons)
			{
				_logger.LogInformation("Scanning weapons...");
				// Get Weapons
				Navigation.InventoryScreen();
				Navigation.SelectWeaponInventory();
				try
				{
					_cancellationToken.ThrowIfCancellationRequested();
                    weaponScraper.ScanWeapons();
				}
				catch (FormatException ex) { UserInterface.AddError(ex.Message); }
				catch (OperationCanceledException) { throw; }
				catch (Exception ex)
				{
					UserInterface.AddError(ex.Message + "\n" + ex.StackTrace);
				}
				Navigation.MainMenuScreen();
				_logger.LogInformation("Done scanning weapons");
			}

			_cancellationToken.ThrowIfCancellationRequested();

			if (Settings.ScanArtifacts)
			{
				_logger.LogInformation("Scanning artifacts...");

				// Get Artifacts
				Navigation.InventoryScreen();
				Navigation.SelectArtifactInventory();
				try
				{
					_cancellationToken.ThrowIfCancellationRequested();
					artifactScraper.ScanArtifacts();
				}
				catch (FormatException ex) { UserInterface.AddError(ex.Message); }
				catch (OperationCanceledException) { throw; }
				catch (Exception ex)
				{
					UserInterface.AddError(ex.Message + "\n" + ex.StackTrace);
				}
				Navigation.MainMenuScreen();
				_logger.LogInformation("Done scanning artifacts");
			}

			workerQueue.Enqueue(new OCRImageCollection(null, "END", 0));

			_cancellationToken.ThrowIfCancellationRequested();

			if (Settings.ScanCharacters)
			{
				_logger.LogInformation("Scanning characters...");
				// Get characters
				Navigation.CharacterScreen();
				try
				{
					_cancellationToken.ThrowIfCancellationRequested();
					characterScraper.ScanCharacters(ref Characters);
				}
				catch (OperationCanceledException) { throw; }
				catch (Exception ex)
				{
					UserInterface.AddError(ex.Message + "\n" + ex.StackTrace);
				}
				Navigation.MainMenuScreen();
				_logger.LogInformation("Done scanning characters");
			}

			// Wait for Image Processors to finish
			AwaitProcessors();

			if (Settings.ScanCharacters)
			{
				// Assign Artifacts to Characters
				if (Settings.ScanArtifacts)
					AssignArtifacts();
				if (Settings.ScanWeapons)
					AssignWeapons();
			}

			_cancellationToken.ThrowIfCancellationRequested();

			// Scan Character Development Items
			if (Settings.ScanCharDevItems)
			{
				_logger.LogInformation("Scanning character development materials...");
				// Get Materials
				Navigation.InventoryScreen();
				Navigation.SelectCharacterDevelopmentInventory();
				HashSet<Material> devItems = new HashSet<Material>();
				try
				{
					_cancellationToken.ThrowIfCancellationRequested();
					materialScraper.SetInventoryPage(InventoryPage.CharacterDevelopmentItems);
					materialScraper.Scan_Materials(ref Inventory);
				}
				catch (FormatException ex) { UserInterface.AddError(ex.Message); }
				catch (OperationCanceledException) { throw; }
				catch (Exception ex)
				{
					UserInterface.AddError(ex.Message + "\n" + ex.StackTrace);
				}
				Navigation.MainMenuScreen();
				_logger.LogInformation("Done scanning character development materials");
			}

			_cancellationToken.ThrowIfCancellationRequested();

			// Scan Materials
			if (Settings.ScanMaterials)
			{
				_logger.LogInformation("Scanning materials...");
				// Get Materials
				Navigation.InventoryScreen();
				Navigation.SelectMaterialInventory();
				HashSet<Material> materials = new HashSet<Material>();
				try
				{
					_cancellationToken.ThrowIfCancellationRequested();
					materialScraper.SetInventoryPage(InventoryPage.Materials);
					materialScraper.Scan_Materials(ref Inventory);
				}
				catch (FormatException ex) { UserInterface.AddError(ex.Message); }
				catch (OperationCanceledException) { throw; }
				catch (Exception ex)
				{
					UserInterface.AddError(ex.Message + "\n" + ex.StackTrace);
				}
				Navigation.MainMenuScreen();
				_logger.LogInformation("Done scanning materials");
			}
		}

		private void AwaitProcessors()
		{
			// Block until every worker thread has exited. Thread.Join yields the CPU
			// while waiting instead of the previous tight RemoveAll() spin loop, which
			// pegged a core at 100% for the entire scan.
			foreach (var processor in ImageProcessors)
			{
				processor.Join();
			}
			ImageProcessors.Clear();
			b_threadCancel = false;
		}

		public void ImageProcessorWorker()
		{
			_logger.LogDebug("Thread #{ThreadId} priority: {Priority}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Priority);
			while (true)
			{
				if (b_threadCancel)
				{
					workerQueue.Clear();
					break;
				}

				if (workerQueue.TryDequeue(out OCRImageCollection imageCollection))
				{
					switch (imageCollection.Type)
					{
						case "weapon":
							if (weaponScraper.IsEnhancementMaterial(imageCollection.Bitmaps.First()))
							{
								_logger.LogDebug("Enhancement Material found for weapon #{weaponID}", imageCollection.Id);
								weaponScraper.StopScanning = true;
								break;
							}

							UserInterface.SetGearPictureBox(imageCollection.Bitmaps.Last());

							// Scan as weapon
							Weapon weapon = weaponScraper.CatalogueFromBitmapsAsync(imageCollection.Bitmaps, imageCollection.Id).Result;
							UserInterface.SetGear(imageCollection.Bitmaps.Last(), weapon);

							string weaponPath = $"./logging/weapons/weapon{weapon.Id}/";

							if (Settings.LogScreenshots) Directory.CreateDirectory(weaponPath);

							if (weapon.IsValid())
							{
								UserInterface.IncrementWeaponCount();
								Inventory.Add(weapon);
								if (!string.IsNullOrWhiteSpace(weapon.EquippedCharacter))
									equippedWeapons.Add(weapon);
							}
							else
							{
								UserInterface.AddError($"Unable to validate information for weapon ID#{weapon.Id}");
								string error = "";
								if (!weapon.HasValidWeaponName()) error += "Invalid weapon name\n"; 
								if (!weapon.HasValidRarity()) error += "Invalid weapon rarity\n";
								if (!weapon.HasValidLevel()) error += "Invalid weapon level\n";
								if (!weapon.HasValidRefinementLevel()) error += "Invalid refinement level\n";
								if (!weapon.HasValidEquippedCharacter()) error += "Inavlid equipped character\n";
								UserInterface.AddError(error + weapon.ToString());
								Directory.CreateDirectory(weaponPath);
								using (var writer = File.CreateText(weaponPath + "log.txt"))
								{
									writer.WriteLine($"Version: {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}");
									writer.WriteLine($"Resolution: {Navigation.GetWidth()}x{Navigation.GetHeight()}");
									writer.WriteLine($"Error log:\n\t{error.Replace("\n", "\n\t")}");
								}
							}

                            if (!weapon.IsValid() || Settings.LogScreenshots)
                            {
                                Directory.CreateDirectory(weaponPath + "name");
                                imageCollection.Bitmaps[0].Save(weaponPath + "name/name.png");
                                Directory.CreateDirectory(weaponPath + "rarity");
                                imageCollection.Bitmaps[0].Save(weaponPath + "rarity/rarity.png");
                                Directory.CreateDirectory(weaponPath + "level");
                                imageCollection.Bitmaps[1].Save(weaponPath + "level/level.png");
                                Directory.CreateDirectory(weaponPath + "refinement");
                                imageCollection.Bitmaps[2].Save(weaponPath + "refinement/refinement.png");
                                Directory.CreateDirectory(weaponPath + "equipped");
                                imageCollection.Bitmaps[4].Save(weaponPath + "equipped/equipped.png");

                                imageCollection.Bitmaps.Last().Save(weaponPath + "card.png");
								Task.Run(() => LogObject(weapon, weaponPath + "weapon.json"));
                            }

                            // Dispose of everything
                            imageCollection.Bitmaps.ForEach(b => b.Dispose());
							break;

						case "artifact":
							if (artifactScraper.IsEnhancementMaterial(imageCollection.Bitmaps.Last()))
							{
								_logger.LogDebug("Enhancement Material found for artifact #{artifactID}", imageCollection.Id);
								artifactScraper.StopScanning = true;
								break;
							}

							UserInterface.SetGearPictureBox(imageCollection.Bitmaps.Last());
							// Scan as artifact
							Artifact artifact = artifactScraper.CatalogueFromBitmapsAsync(imageCollection.Bitmaps, imageCollection.Id).Result;
							UserInterface.SetGear(imageCollection.Bitmaps.Last(), artifact);

							string artifactPath = $"./logging/artifacts/artifact{artifact.Id}/";

                            if (Settings.LogScreenshots) Directory.CreateDirectory(artifactPath);

							if (artifact.IsValid())
							{
								UserInterface.IncrementArtifactCount();
								Inventory.Add(artifact);
								if (!string.IsNullOrWhiteSpace(artifact.EquippedCharacter))
									equippedArtifacts.Add(artifact);
							}
							else
							{
								UserInterface.AddError($"Unable to validate information for artifact ID#{artifact.Id}");
								string error = "";
								if (!artifact.HasValidSlot()) error += "Invalid artifact gear slot\n";
								if (!artifact.HasValidSetName()) error += "Invalid artifact set name\n";
								if (!artifact.HasValidRarity()) error += "Invalid artifact rarity\n";
								if (!artifact.HasValidLevel()) error += "Invalid artifact level\n";
								if (!artifact.HasValidMainStat()) error += "Invalid artifact main stat\n";
								if (!artifact.HasValidSubStats()) error += "Invalid artifact sub stats\n";
								if (!artifact.HasValidEquippedCharacter()) error += "Invalid equipped character\n";
								UserInterface.AddError(error + artifact.ToString());
								Directory.CreateDirectory(artifactPath);
								using (var writer = File.CreateText(artifactPath + "log.txt"))
								{
									writer.WriteLine($"Version: {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}");
									writer.WriteLine($"Resolution: {Navigation.GetWidth()}x{Navigation.GetHeight()}");
									writer.WriteLine($"Error Log:\n\t{error.Replace("\n", "\n\t")}");
								}
							}

                            if (!artifact.IsValid() || Settings.LogScreenshots)
                            {
                                Directory.CreateDirectory(artifactPath + "name");
                                imageCollection.Bitmaps[0].Save(artifactPath + "name/name.png");
                                Directory.CreateDirectory(artifactPath + "slot");
								imageCollection.Bitmaps[1].Save(artifactPath + "slot/slot.png");
                                Directory.CreateDirectory(artifactPath + "mainstat");
                                imageCollection.Bitmaps[2].Save(artifactPath + "mainstat/mainstat.png");
								Directory.CreateDirectory(artifactPath + "level");
								imageCollection.Bitmaps[3].Save(artifactPath + "level/level.png");
								Directory.CreateDirectory(artifactPath + "substats");
								imageCollection.Bitmaps[4].Save(artifactPath + "substats/substats.png");
								Directory.CreateDirectory(artifactPath + "equipped");
								imageCollection.Bitmaps[5].Save(artifactPath + "equipped/equipped.png");
								Directory.CreateDirectory(artifactPath + "locked");
								imageCollection.Bitmaps[6].Save(artifactPath + "locked/locked.png");
                                Directory.CreateDirectory(artifactPath + "sanctify");
                                imageCollection.Bitmaps[7].Save(artifactPath + "sanctify/sanctify.png");


                                imageCollection.Bitmaps.Last().Save(artifactPath + "card.png");

								Task.Run(()=>LogObject(artifact, artifactPath + "artifact.json"));
							}

							// Dispose of everything
							imageCollection.Bitmaps.ForEach(b => b.Dispose());
							break;

						case "END":
							b_threadCancel = true;
							break;

						default:
							_logger.LogError("Unknown Image type for Image Processor: {Type}", imageCollection.Type);
							break;
					}
				}
				else
				{
					// Wait for more images to process
					Thread.Sleep(250);
				}
			}
			_logger.LogDebug("Thread {threadId} exit", Thread.CurrentThread.ManagedThreadId);
		}

        private static void LogObject(object obj, string path)
        {
            using (var file = new StreamWriter(path))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };
                serializer.Serialize(file, obj);
            }
        }

        public void AssignArtifacts()
		{
			foreach (Artifact artifact in equippedArtifacts)
			{
				foreach (Character character in Characters)
				{
					if (artifact.EquippedCharacter == character.NameGOOD)
					{
						character.AssignArtifact(artifact); // Do we even need to do this?
						_logger.LogDebug("Assigned {fearSlot} to {character}", artifact.GearSlot, character.NameGOOD);
						break;
					}
				}
			}
		}

		public void AssignWeapons()
		{
			foreach (Character character in Characters)
			{
				foreach (Weapon weapon in equippedWeapons)
				{
					if (weapon.EquippedCharacter == character.NameGOOD)
					{
						character.AssignWeapon(weapon);
						_logger.LogDebug("Assigned {weapon} to {character}", weapon.Name, character.NameGOOD);
						break;
					}
				}
				if (character.Weapon is null)
				{
					Inventory.Add(new Weapon(character.WeaponType, character.NameGOOD));
					_logger.LogInformation("Default weapon assigned to {character}", character.NameGOOD);
				}
			}
		}
	}
}