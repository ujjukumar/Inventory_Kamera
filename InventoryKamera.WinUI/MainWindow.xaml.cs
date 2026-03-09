using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using InventoryKamera.WinUI.ViewModels;

namespace InventoryKamera.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            ViewModel = App.Current.Cast<App>().Host.Services.GetRequiredService<MainViewModel>();
        }
    }

    public static class AppExtensions
    {
        public static T Cast<T>(this Application app) where T : Application => (T)app;
    }
}
