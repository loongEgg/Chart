using LoongEgg.Core;

namespace LoongEgg.Chart
{
    public partial class Signal : BindableObject, ISignal
    {
        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }
        private double _Value;

        public string Label
        {
            get { return _Label; }
            set { SetProperty(ref _Label, value); }
        }
        private string _Label;

        public string Unit
        {
            get { return _Unit; }
            set { SetProperty(ref _Unit, value); }
        }
        private string _Unit;


    }
}
