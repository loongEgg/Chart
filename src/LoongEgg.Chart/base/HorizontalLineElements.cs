using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LoongEgg.Chart
{
    public abstract class HorizontalLineElements : HorizontalChartElements
    {
        protected readonly Path Lines = new Path();

        public override void OnInitializing()
        {
            Binding binding = new Binding(nameof(Stroke)) { Source = this };
            Lines.SetBinding(Path.StrokeProperty, binding);
            binding = new Binding(nameof(StrokeThickness)) { Source = this };
            Lines.SetBinding(Path.StrokeThicknessProperty, binding);
        }

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
                typeof(HorizontalLineElements),
                new PropertyMetadata(Brushes.LightGray, OnParameterChanged)); 

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
                typeof(HorizontalLineElements),
                new PropertyMetadata(0.5d, OnParameterChanged));

    }
}
