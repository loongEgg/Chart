using LoongEgg.Log;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LoongEgg.Chart
{
    public class AxisLabels : AxisTicks
    {
        public override void OnInitializing()
        {
            SetCurrentValue(GapProperty, 4d);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == FontSizeProperty
                || e.Property == FontFamilyProperty
                || e.Property == FontWeightProperty)
                Update();
        }
        public override void Update()
        {
            if (InternalChange == true) return;
            if (Root == null) return;
            Root.Children.Clear();
            if (RenderSize.Height == 0 && RenderSize.Width == 0) return;
            if (Ticks == null || Ticks.Count() <= 1) return;
            Logger.Dbug($"AxisLabels[{this.GetHashCode()}] update x {++UpdateCount}");
            ValueToScreen valueToScreen = ValueToScreen;

            if (Placement == Placements.Top || Placement == Placements.Bottom)
            {
                TextBlock text;
                var textAlignment = TextAlignment.Center;

                Action<double, FrameworkElement> setLeft;
                if (valueToScreen != null)
                    setLeft = (v, e) => Canvas.SetLeft(e, valueToScreen(v) - GetSize(e).Width / 2.0);
                else
                    setLeft = (v, e) => Canvas.SetLeft(e, v - GetSize(e).Width / 2.0);

                Action<double, FrameworkElement> setTopOrBottom;
                if (Placement == Placements.Top)
                    setTopOrBottom = (v, e) => Canvas.SetBottom(e, v);
                else
                    setTopOrBottom = (v, e) => Canvas.SetTop(e, v);

                foreach (var x in Ticks)
                {
                    text = new TextBlock
                    {
                        FontFamily = FontFamily,
                        FontSize = FontSize,
                        Foreground = Foreground,
                        Text = x.ToString(),
                        TextAlignment = textAlignment
                    };
                    Root.Children.Add(text);
                    setLeft(x, text);
                    setTopOrBottom(Gap, text);
                }

            }

            else if (Placement == Placements.Left || Placement == Placements.Right)
            {
                TextBlock text;

                TextAlignment textAlignment;
                if (Placement == Placements.Left)
                    textAlignment = TextAlignment.Right;
                else
                    textAlignment = TextAlignment.Left;

                Action<double, FrameworkElement> setTop;
                if (valueToScreen != null)
                    setTop = (v, e) => Canvas.SetTop(e, valueToScreen(v) - GetSize(e).Height / 2.0);
                else
                    setTop = (v, e) => Canvas.SetTop(e, v - GetSize(e).Height / 2.0);

                Action<double, FrameworkElement> setLeftOrRight;
                if (Placement == Placements.Left)
                    setLeftOrRight = (v, e) => Canvas.SetRight(e, v);
                else
                    setLeftOrRight = (v, e) => Canvas.SetLeft(e, v);

                foreach (var x in Ticks)
                {
                    text = new TextBlock
                    {
                        FontFamily = FontFamily,
                        FontSize = FontSize,
                        Foreground = Foreground,
                        Text = x.ToString(),
                        TextAlignment = textAlignment
                    };
                    Root.Children.Add(text);
                    setTop(x, text);
                    setLeftOrRight(Gap, text);
                }

            }

        }

        static Size GetSize(FrameworkElement self)
        {
            self.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            self.Arrange(new Rect(self.DesiredSize));

            return new Size(self.ActualWidth, self.ActualHeight);
        }


    }
}
