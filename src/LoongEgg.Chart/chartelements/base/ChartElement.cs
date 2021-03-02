using System.Windows;
using System.Windows.Controls;
using LoongEgg.Log;

namespace LoongEgg.Chart
{
    public abstract class ChartElement : ContentControl, IChartElement, ICountOnUpdate
    {
        public int UpdateCount { get; protected set; } = 0;
        public Panel Root { get; internal set; } = new Canvas();

        public IChart Container
        {
            get { return _Container; }
            set
            {
                if (_Container == value) return;
                _Container = value;
                OnContainerSet();
            }
        }
        private IChart _Container;

        public bool InternalChange { get; set; } = true;

        public ChartElement()
        {
            Content = Root;
            OnInitializing();
            SizeChanged += (s, e) =>
            {
                Logger.Dbug($"{ this.GetType() }[{ this.GetHashCode() }] size changed");
                Update();
            };
            Loaded += (s, e) =>
            {
                InternalChange = false;
                Logger.Dbug($"{ this.GetType() }[{ this.GetHashCode() }] loaded");
                Update();
            };
        }

        public abstract void OnContainerSet();

        public abstract void OnInitializing();

        public abstract void ResetPlacement();

        public abstract void Update();

        protected static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ChartElement)?.Update();

    }
}
