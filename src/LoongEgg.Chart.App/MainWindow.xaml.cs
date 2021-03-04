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

            //chart1.SignalGroup = new System.Collections.ObjectModel.ObservableCollection<Signal> { Signal.CosSignal };
            //chart2.SignalGroup = new System.Collections.ObjectModel.ObservableCollection<Signal> { Signal.SinSignal };
            //chart3.SignalGroup = new System.Collections.ObjectModel.ObservableCollection<Signal> { Signal.SquareSignal, Signal.TriangleSignal };
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
