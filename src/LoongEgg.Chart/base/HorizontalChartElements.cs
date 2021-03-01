using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace LoongEgg.Chart
{

    public abstract class HorizontalChartElements : ChartElement, IChartElements
    {
        public HorizontalChartElements()
        {
            Orientation = System.Windows.Controls.Orientation.Horizontal;
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
                typeof(HorizontalChartElements),
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
                typeof(HorizontalChartElements),
                new PropertyMetadata(default(TickLevels), (s, e) => (s as IChartElements)?.ResetTickSource()));

        public override void OnContainerSet()
        {
            (this as IChartElements)?.ResetTickSource();
            ResetPlacement();
        }

        void IChartElements.ResetTickSource()
        {
            if (Container == null) return;
            if (TickLevel == TickLevels.Major)
            {
                Binding binding = new Binding(nameof(Container.HorizontalMajorTicks)) { Source = Container };
                SetBinding(TicksProperty, binding);
            }
            else
            {
                Binding binding = new Binding(nameof(Container.HorizontalMinorTicks)) { Source = Container };
                SetBinding(TicksProperty, binding);
            }
            Update();
        }
    }
}
