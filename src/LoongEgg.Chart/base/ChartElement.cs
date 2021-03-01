using System.Windows;
using System.Windows.Controls;

namespace LoongEgg.Chart
{
    public abstract class ChartElement : ContentControl, IChartElement
    {
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

        public bool InternalChange { get; set; } = false;

        public ChartElement()
        {
            Content = Root;
            OnInitializing();
            SizeChanged += (s, e) => Update();
            Loaded += (s, e) => Update();
        }

        public abstract void OnContainerSet();

        public abstract void OnInitializing();

        public abstract void ResetPlacement();

        public abstract void Update();

        protected static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ChartElement)?.Update();

    }
}
