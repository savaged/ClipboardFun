using Savaged.ClipboardFun.ViewModels;
using Savaged.ClipboardFun.Views;
using System.Windows;

namespace ClipboardFun
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
            mainWindow.Show();
        }
    }
}
