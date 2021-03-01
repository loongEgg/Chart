using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace LoongEgg.Chart
{
    public abstract class ChartLines : ChartElement, IChartLines
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="Stroke"/>
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                nameof(Stroke),
                typeof(Brush),
                typeof(ChartLines),
                new PropertyMetadata(Brushes.Green, OnParameterChanged));


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="StrokeThickness"/>
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                nameof(StrokeThickness),
                typeof(double),
                typeof(ChartLines),
                new PropertyMetadata(0.5, OnParameterChanged));

    }
}
