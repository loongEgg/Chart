using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using LoongEgg.Log;

namespace LoongEgg.Chart
{
    public class AxisLines : AxisTicks, IAxisLines
    {
        public AxisLines()
        {
            SetCurrentValue(LengthProperty, 10d);
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
                typeof(AxisTicks),
                new PropertyMetadata(Brushes.DarkGreen, OnParameterChanged));

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
                typeof(AxisTicks),
                new PropertyMetadata(2d, OnParameterChanged));


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="Length"/>
        /// </summary>
        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.Register(
                nameof(Length),
                typeof(double),
                typeof(AxisTicks),
                new PropertyMetadata(
                    10d,
                    (s, e) =>
                    {
                        var self = s as AxisLines;
                        if (self == null) return;
                        self.OnLengthSet();
                        self.Update();
                    }));



        private Path Lines = new Path();
        public override void OnInitializing()
        {
            Binding binding;
            binding = new Binding(nameof(Stroke)) { Source = this };
            Lines.SetBinding(Path.StrokeProperty, binding);

            binding = new Binding(nameof(StrokeThickness)) { Source = this };
            Lines.SetBinding(Path.StrokeThicknessProperty, binding);

        }

        public override void Update()
        {
            if (InternalChange == true) return;
            if (Root == null) return;
            Root.Children.Clear();
            if (RenderSize.Height == 0 && RenderSize.Width == 0) return;
            Logger.Dbug($"AxisLines[{this.GetHashCode()}] update x {++UpdateCount}");
            GeometryGroup group = new GeometryGroup();
            ValueToScreen valueToScreen = ValueToScreen;

            if (Placement == Placements.Top || Placement == Placements.Bottom)
            {
                double height = RenderSize.Height;
                if (valueToScreen != null)
                {
                    double x;
                    foreach (var t in Ticks)
                    {
                        x = valueToScreen(t);
                        LineGeometry line = new LineGeometry(new Point(x, 0), new Point(x, height));
                        group.Children.Add(line);
                    }

                }
                else  // if (valueToScreen == null)
                {
                    foreach (var x in Ticks)
                    {
                        LineGeometry line = new LineGeometry(new Point(x, 0), new Point(x, height));
                        group.Children.Add(line);
                    }
                }
            }
            else
            {
                double width = Length;
                if (valueToScreen != null)
                {
                    double y;
                    foreach (var t in Ticks)
                    {
                        y = valueToScreen(t);
                        LineGeometry line = new LineGeometry(new Point(0, y), new Point(width, y));
                        group.Children.Add(line);
                    }

                }
                else  // if (valueToScreen == null)
                {
                    foreach (var y in Ticks)
                    {
                        LineGeometry line = new LineGeometry(new Point(0, y), new Point(width, y));
                        group.Children.Add(line);
                    }
                }
            }

            Lines.Data = group;
            Root.Children.Add(Lines);

            if (Placement == Placements.Top)
                Canvas.SetBottom(Lines, Gap);
            else if (Placement == Placements.Bottom)
                Canvas.SetTop(Lines, Gap);
            else if (Placement == Placements.Left)
                Canvas.SetRight(Lines, Gap);
            else if (Placement == Placements.Right)
                Canvas.SetLeft(Lines, Gap);
                
        }

        public override void OnContainerSet()
        {
            base.OnContainerSet();
            OnLengthSet();
        }
        public void OnLengthSet()
        {
            if (Placement == Placements.Top || Placement == Placements.Bottom)
            {
                Width = Double.NaN;
                Height = Length;
            }
            else
            {
                Width = Length;
                Height = Double.NaN;
            }
        }
    }
}
