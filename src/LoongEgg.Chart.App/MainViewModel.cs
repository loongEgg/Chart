using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoongEgg.Chart.App
{
    public class MainViewModel
    {
        public static MainViewModel DesignInstance => _DesignInstance ?? (_DesignInstance = CreatDesignInstance());
        static MainViewModel _DesignInstance;

        static MainViewModel CreatDesignInstance()
        {
            var vm = new MainViewModel();

            vm.SignalGroup_1 = new ObservableCollection<Signal>() { Signal.CosSignal, Signal.SinSignal };
            vm.SignalGroup_2 = new ObservableCollection<Signal>() { Signal.CosSignal, Signal.SquareSignal };
            vm.SignalGroup_3 = new ObservableCollection<Signal>() { Signal.SquareSignal, Signal.TriangleSignal };

            return vm;
        }

        public ObservableCollection<Signal> SignalGroup_1 { get; set; }
        public ObservableCollection<Signal> SignalGroup_2 { get; set; }
        public ObservableCollection<Signal> SignalGroup_3 { get; set; }
    }
}
