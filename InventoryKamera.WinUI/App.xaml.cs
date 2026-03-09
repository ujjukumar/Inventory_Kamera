using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;
using InventoryKamera.WinUI.ViewModels;

namespace InventoryKamera.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;
        public IHost Host { get; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register Views
                    services.AddSingleton<MainWindow>();

                    // Register ViewModels
                    services.AddSingleton<MainViewModel>();

                    // Register InventoryKamera Services
                    services.AddSingleton<global::InventoryKamera.WeaponScraper>();
                    services.AddSingleton<global::InventoryKamera.ArtifactScraper>();
                    services.AddSingleton<global::InventoryKamera.CharacterScraper>();
                    services.AddSingleton<global::InventoryKamera.MaterialScraper>();
                    services.AddSingleton<global::InventoryKamera.InventoryKamera>();
                })
                .Build();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = Host.Services.GetRequiredService<MainWindow>();
            _window.Activate();
        }
    }
}
