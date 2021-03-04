using System;
using LoongEgg.Core;

namespace LoongEgg.Chart
{
    // TODO: remove to LoongEgg.Data
    public partial class Signal : BindableObject, ISignal
    {
        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                RaisePropertyChanged(nameof(Value));
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private double _Value;
         
        public string Label
        {
            get { return _Label; }
            set { SetProperty(ref _Label, value); }
        }
        private string _Label = "[ label ]";
         
        public string Unit
        {
            get { return _Unit; }
            set { SetProperty(ref _Unit, value); }
        }
        private string _Unit = "-";


        public event EventHandler ValueChanged;
    }
}
