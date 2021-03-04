using System.Windows;
using LoongEgg.Log;

namespace LoongEgg.Chart.App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Logger.Enable(Loggers.Console);
            this.MainWindow = new MainWindow() { DataContext = MainViewModel.DesignInstance };
            this.MainWindow.Show();
        }
    }
}
