using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace LoongEgg.Chart
{
    public abstract class AxisTicks : AxisElement, IAxisTicks
    {
        public override void OnContainerSet()
        {
            ResetPlacement();
            ResetTickSource();
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public IEnumerable<double> Ticks
        {
            get { return (IEnumerable<double>)GetValue(TicksProperty); }
            set { SetValue(TicksProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="Ticks"/>
        /// </summary>
        public static readonly DependencyProperty TicksProperty =
            DependencyProperty.Register(
                nameof(Ticks),
                typeof(IEnumerable<double>),
                typeof(AxisTicks),
                new PropertyMetadata(default(IEnumerable<double>), OnParameterChanged));

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public TickLevels TickLevel
        {
            get { return (TickLevels)GetValue(TickLevelProperty); }
            set { SetValue(TickLevelProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="TickLevel"/>
        /// </summary>
        public static readonly DependencyProperty TickLevelProperty =
            DependencyProperty.Register(
                nameof(TickLevel),
                typeof(TickLevels),
                typeof(AxisTicks),
                new PropertyMetadata(default(TickLevels), (s, e) => (s as AxisTicks)?.ResetTickSource()));

        public void ResetTickSource()
        {
            if (Container == null) return;
            Binding binding;
            if (Placement == Placements.Top || Placement == Placements.Bottom)
            {
                if (TickLevel == TickLevels.Major)
                    binding = new Binding(nameof(Chart.HorizontalMajorTicks));
                else
                    binding = new Binding(nameof(Chart.HorizontalMinorTicks));
            }
            else
            {
                if (TickLevel == TickLevels.Major)
                    binding = new Binding(nameof(Chart.VerticalMajorTicks));
                else
                    binding = new Binding(nameof(Chart.VerticalMinorTicks));
            }
            binding.Source = Container;
            SetBinding(TicksProperty, binding);
        }

    }
}
