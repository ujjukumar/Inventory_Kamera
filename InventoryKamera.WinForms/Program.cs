using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace InventoryKamera;

internal static class Program
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    public static IServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        AttachConsole(-1);
        ConfigureLogging();

        try
        {
            var host = CreateHostBuilder(args).Build();
            ServiceProvider = host.Services;

            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var mainForm = ServiceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            Logger.Fatal(ex, "Main application crashed");
            MessageBox.Show("Inventory Kamera has encountered an error it was not meant to handle." +
                " Please check the application's log and/or upload it to GitHub when reporting your issue. Thanks!",
                "Application Crashed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<MainForm>();
                services.AddTransient<InventoryKamera>(); // Transient because it's re-created for scans
                services.AddTransient<WeaponScraper>();
                services.AddTransient<ArtifactScraper>();
                services.AddTransient<CharacterScraper>();
                services.AddTransient<MaterialScraper>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                logging.AddNLog();
            });

    private static void ConfigureLogging()
    {
        var config = new NLog.Config.LoggingConfiguration();

        var debugFile = new NLog.Targets.FileTarget("logfile")
        {
            Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.fff}|${level:uppercase=true}|${logger:shortName=True}|${message:withexception=true}",
            FileName = "./logging/InventoryKamera.debug.log",
            ArchiveFileName = "logging/archives/InventoryKamera.{####}.debug.log",
            ArchiveSuffixFormat = "yyyyMMddHHmmss",
            MaxArchiveFiles = 4,
            KeepFileOpen = true,
            ArchiveOldFileOnStartup = true
        };

        var logFile = new NLog.Targets.FileTarget("logfile")
        {
            Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.fff}|${level:uppercase=true}|${logger:shortName=True}|${message:withexception=true}",
            FileName = "./logging/InventoryKamera.log",
            ArchiveFileName = "logging/archives/InventoryKamera.{####}.log",
            ArchiveSuffixFormat = "yyyyMMddHHmmss",
            MaxArchiveFiles = 4,
            KeepFileOpen = true,
            ArchiveOldFileOnStartup = true
        };

        var logConsole = new NLog.Targets.ConsoleTarget("logconsole")
        {
            Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.fff}|${level:uppercase=true}|${logger:shortName=True}|${message:withexception=true}",
        };

        var logDebugger = new NLog.Targets.DebuggerTarget("logdebugger")
        {
            Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss.fff}|${level:uppercase=true}|${logger:shortName=True}|${message:withexception=true}",
        };

        config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logDebugger);
        config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logConsole);
        config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, debugFile);
        config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logFile);

        LogManager.Configuration = config;
    }

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool AttachConsole(int dwProcessId);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();
}
