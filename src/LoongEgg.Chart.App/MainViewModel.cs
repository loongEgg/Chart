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

            vm.SignalGroup_1 = new SignalGroup() { Label = "Signal 1" };
            vm.SignalGroup_1.Signals.Add(Signal.CosSignal);
            vm.SignalGroup_1.Signals.Add(Signal.SinSignal);

            vm.SignalGroup_2 = new SignalGroup() { Label = "Signal 2" };
            vm.SignalGroup_2.Signals.Add(Signal.CosSignal);
            vm.SignalGroup_2.Signals.Add(Signal.SquareSignal);

            vm.SignalGroup_3 = new SignalGroup() { Label = "Signal 3" };
            vm.SignalGroup_3.Signals.Add(Signal.SquareSignal);
            vm.SignalGroup_3.Signals.Add(Signal.TriangleSignal);

            vm.SignalGroups = new ObservableCollection<SignalGroup>
            {
                vm.SignalGroup_1,
                vm.SignalGroup_2,
                vm.SignalGroup_3
            };

            vm.MonitoredGroups.Add(vm.SignalGroup_1);
            //vm.MonitoredGroups.Add(vm.SignalGroup_2);
            //vm.MonitoredGroups.Add(vm.SignalGroup_3);

            return vm;
        }

        public SignalGroup SignalGroup_1 { get; set; }
        public SignalGroup SignalGroup_2 { get; set; }
        public SignalGroup SignalGroup_3 { get; set; }

        public Signal SelectedSignal { get; set; }

        public SignalGroup SelectedGroup
        {
            get { return _SelectedGroup; }
            set { _SelectedGroup = value; }
        }
        private SignalGroup _SelectedGroup;
        public ObservableCollection<SignalGroup> SignalGroups { get; set; }

        public ObservableCollection<SignalGroup> MonitoredGroups { get; set; } = new ObservableCollection<SignalGroup>();
    }
}
