using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LoongEgg.Chart
{
    public abstract class ChartElement : ContentControl, IChartElement
    {
        public Panel Root { get; } = new Canvas();

        public IChart Container
        {
            get { return _Container; }
            internal set
            {
                if (value == _Container) return;
                _Container = value;
                OnContainerSet();
            }
        }
        private IChart _Container;

        public ValueToScreen ValueToScreen
        {
            get
            {
                if (Container == null) return null;
                if (Orientation == Orientation.Horizontal)
                    return Container.HorizontalValueToScreen;
                else
                    return Container.VerticalValueToScreen;
            }
        }

        [DefaultValue(true)]
        public bool InternalChange { get; set; } = false;

        public ChartElement()
        {
            Content = Root;
            OnInitializing();
        }

        public abstract void Update();

        public abstract void OnInitializing();

        public abstract void ResetMargin();

        public abstract void OnContainerSet();

        public abstract void ResetPlacement();

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public double Gap
        {
            get { return (double)GetValue(GapProperty); }
            set { SetValue(GapProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="Gap"/>
        /// </summary>
        public static readonly DependencyProperty GapProperty =
            DependencyProperty.Register(
                nameof(Gap),
                typeof(double),
                typeof(ChartElement),
                new PropertyMetadata(
                    default(double),
                    (s, e) => (s as ChartElement)?.ResetMargin()));

        public Orientation Orientation { get; internal set; }

        protected static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as ChartElement)?.Update();
    }
}
