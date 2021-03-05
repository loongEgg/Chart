using LoongEgg.Data;
using System.Collections.ObjectModel;

namespace LoongEgg.Chart.App
{
    public class MainViewModel
    {
        public static MainViewModel DesignInstance => _DesignInstance ?? (_DesignInstance = CreatDesignInstance());
        static MainViewModel _DesignInstance;

        static MainViewModel CreatDesignInstance()
        {
            var vm = new MainViewModel();

            vm.SignalGroup_1 = new SignalGroup() { Signal.CosSignal, Signal.SinSignal };
            vm.SignalGroup_2 = new SignalGroup() { Signal.CosSignal, Signal.SquareSignal };
            vm.SignalGroup_3 = new SignalGroup() { Signal.SquareSignal, Signal.TriangleSignal };

            vm.SignalGroups = new ObservableCollection<SignalGroup>
            {
                vm.SignalGroup_1,
                vm.SignalGroup_2,
                vm.SignalGroup_3
            };

            return vm;
        }

        public SignalGroup SignalGroup_1 { get; set; }
        public SignalGroup SignalGroup_2 { get; set; }
        public SignalGroup SignalGroup_3 { get; set; }

        public ObservableCollection<SignalGroup> SignalGroups { get; set; }
    }
}
