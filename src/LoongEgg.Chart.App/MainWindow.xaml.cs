using System.Windows;
using System.Windows.Controls;

namespace LoongEgg.Chart.App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
       }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            var btn = e.OriginalSource as Button;
            if (vm != null)
            {
                if (vm.SignalGroups.Count >= 1 && btn.Content.ToString() == "Delete")
                    vm.SignalGroups.RemoveAt(0);
                else if (btn.Content.ToString() == "Add")
                    vm.SignalGroups.Add(vm.SignalGroup_1);
            }
        }
    }
}
