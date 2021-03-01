using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LoongEgg.Chart
{
    public class GridLines : ChartLines, IGridLines
    {
        #region dependency properties

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public IEnumerable<double> HorizontalMajorTicks
        {
            get { return (IEnumerable<double>)GetValue(HorizontalMajorTicksProperty); }
            set { SetValue(HorizontalMajorTicksProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="HorizontalMajorTicks"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalMajorTicksProperty =
            DependencyProperty.Register(
                nameof(HorizontalMajorTicks),
                typeof(IEnumerable<double>),
                typeof(GridLines),
                new PropertyMetadata(default(IEnumerable<double>), OnParameterChanged));

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public IEnumerable<double> HorizontalMinorTicks
        {
            get { return (IEnumerable<double>)GetValue(HorizontalMinorTicksProperty); }
            set { SetValue(HorizontalMinorTicksProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="HorizontalMinorTicks"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalMinorTicksProperty =
            DependencyProperty.Register(
                nameof(HorizontalMinorTicks),
                typeof(IEnumerable<double>),
                typeof(GridLines),
                new PropertyMetadata(default(IEnumerable<double>), OnParameterChanged));


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public IEnumerable<double> VerticalMajorTicks
        {
            get { return (IEnumerable<double>)GetValue(VerticalMajorTicksProperty); }
            set { SetValue(VerticalMajorTicksProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="VerticalMajorTicks"/>
        /// </summary>
        public static readonly DependencyProperty VerticalMajorTicksProperty =
            DependencyProperty.Register(
                nameof(VerticalMajorTicks),
                typeof(IEnumerable<double>),
                typeof(GridLines),
                new PropertyMetadata(default(IEnumerable<double>), OnParameterChanged));


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public IEnumerable<double> VerticalMinorTicks
        {
            get { return (IEnumerable<double>)GetValue(VerticalMinorTicksProperty); }
            set { SetValue(VerticalMinorTicksProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="VerticalMinorTicks"/>
        /// </summary>
        public static readonly DependencyProperty VerticalMinorTicksProperty =
            DependencyProperty.Register(
                nameof(VerticalMinorTicks),
                typeof(IEnumerable<double>),
                typeof(GridLines),
                new PropertyMetadata(default(IEnumerable<double>), OnParameterChanged));

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public bool IsHorizontalMajorTickVisible
        {
            get { return (bool)GetValue(IsHorizontalMajorTickVisibleProperty); }
            set { SetValue(IsHorizontalMajorTickVisibleProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="IsHorizontalMajorTickVisible"/>
        /// </summary>
        public static readonly DependencyProperty IsHorizontalMajorTickVisibleProperty =
            DependencyProperty.Register(
                nameof(IsHorizontalMajorTickVisible),
                typeof(bool),
                typeof(GridLines),
                new PropertyMetadata(default(bool), OnParameterChanged));


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public bool IsHorizontalMinorTickVisible
        {
            get { return (bool)GetValue(IsHorizontalMinorTickVisibleProperty); }
            set { SetValue(IsHorizontalMinorTickVisibleProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="IsHorizontalMinorTickVisible"/>
        /// </summary>
        public static readonly DependencyProperty IsHorizontalMinorTickVisibleProperty =
            DependencyProperty.Register(
                nameof(IsHorizontalMinorTickVisible),
                typeof(bool),
                typeof(GridLines),
                new PropertyMetadata(default(bool), OnParameterChanged));

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public bool IsVerticalMajorTickVisible
        {
            get { return (bool)GetValue(IsVerticalMajorTickVisibleProperty); }
            set { SetValue(IsVerticalMajorTickVisibleProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="IsVerticalMajorTickVisible"/>
        /// </summary>
        public static readonly DependencyProperty IsVerticalMajorTickVisibleProperty =
            DependencyProperty.Register(
                nameof(IsVerticalMajorTickVisible),
                typeof(bool),
                typeof(GridLines),
                new PropertyMetadata(default(bool), OnParameterChanged));

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public bool IsVerticalMinorTickVisible
        {
            get { return (bool)GetValue(IsVerticalMinorTickVisibleProperty); }
            set { SetValue(IsVerticalMinorTickVisibleProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="IsVerticalMinorTickVisible"/>
        /// </summary>
        public static readonly DependencyProperty IsVerticalMinorTickVisibleProperty =
            DependencyProperty.Register(
                nameof(IsVerticalMinorTickVisible),
                typeof(bool),
                typeof(GridLines),
                new PropertyMetadata(default(bool), OnParameterChanged));

        #endregion

        #region value to screen method

        public ValueToScreen HorizontalValueToScreen => Container?.HorizontalValueToScreen;

        public ValueToScreen VerticalValueToScreen => Container?.VerticalValueToScreen;

        #endregion

        public override void OnContainerSet()
        {
            if (Container == null) return;
            Binding binding;

            binding = new Binding(nameof(Container.HorizontalMajorTicks)) { Source = Container };
            SetBinding(HorizontalMajorTicksProperty, binding);

            binding = new Binding(nameof(Container.HorizontalMinorTicks)) { Source = Container };
            SetBinding(HorizontalMinorTicksProperty, binding);

            binding = new Binding(nameof(Container.VerticalMajorTicks)) { Source = Container };
            SetBinding(VerticalMajorTicksProperty, binding);

            binding = new Binding(nameof(Container.VerticalMinorTicks)) { Source = Container };
            SetBinding(VerticalMinorTicksProperty, binding);

            ResetPlacement();
        }
         
        public override void ResetPlacement()
        {
            if (Container == null || Container.PART_Center == null) return;
            if (Parent == Container.PART_Center) return;
            (Parent as Panel)?.Children.Remove(this);
            Container.PART_Center.Children.Add(this);
        }

        private Path HorizontalMajorLines = new Path();
        private Path HorizontalMinorLines = new Path();
        private Path VerticalMajorLines = new Path();
        private Path VerticalMinorLines = new Path();
        public override void OnInitializing() {
            Binding binding;

            binding = new Binding(nameof(Stroke)) { Source = this };
            HorizontalMajorLines.SetBinding(Path.StrokeProperty, binding);
            HorizontalMinorLines.SetBinding(Path.StrokeProperty, binding);
            VerticalMajorLines.SetBinding(Path.StrokeProperty, binding);
            VerticalMinorLines.SetBinding(Path.StrokeProperty, binding);

            binding = new Binding(nameof(StrokeThickness)) { Source = this };
            HorizontalMajorLines.SetBinding(Path.StrokeThicknessProperty, binding);
            HorizontalMinorLines.SetBinding(Path.StrokeThicknessProperty, binding);
            VerticalMajorLines.SetBinding(Path.StrokeThicknessProperty, binding);
            VerticalMinorLines.SetBinding(Path.StrokeThicknessProperty, binding);
        }



        public override void Update()
        {
            if (Root == null)
            {
                Debugger.Break();
                return;
            }
            Root.Children.Clear();
            if (RenderSize.Height == 0 || RenderSize.Width == 0) return;

            if (HorizontalMajorTicks != null && IsHorizontalMajorTickVisible == true)
                UpdateHorizontalTicks(HorizontalMajorTicks, HorizontalMajorLines);

            if (HorizontalMinorTicks != null && IsHorizontalMinorTickVisible == true)
                UpdateHorizontalTicks(HorizontalMinorTicks, HorizontalMinorLines);

            if (VerticalMajorTicks != null && IsVerticalMajorTickVisible == true)
                UpdateVerticalTicks(VerticalMajorTicks, VerticalMajorLines);

            if (VerticalMinorTicks != null && IsVerticalMinorTickVisible == true)
                UpdateVerticalTicks(VerticalMinorTicks, VerticalMinorLines);
        }

        private void UpdateHorizontalTicks(IEnumerable<double> ticks, Path lines)
        {
            GeometryGroup group = new GeometryGroup();
            double height = RenderSize.Height;
            ValueToScreen valueToScreen = HorizontalValueToScreen;

            if (valueToScreen != null)
            {
                double x;
                foreach (var t in ticks)
                {
                    x = valueToScreen(t);
                    LineGeometry line = new LineGeometry(new Point(x, 0), new Point(x, height));
                    group.Children.Add(line);
                }

            }
            else  // if (valueToScreen == null)
            {
                foreach (var x in ticks)
                {
                    LineGeometry line = new LineGeometry(new Point(x, 0), new Point(x, height));
                    group.Children.Add(line);
                }
            }
            lines.Data = group;
            Root.Children.Add(lines);
        }

        private void UpdateVerticalTicks(IEnumerable<double> ticks, Path lines)
        {
            GeometryGroup group = new GeometryGroup();
            double width = RenderSize.Width;
            ValueToScreen valueToScreen = VerticalValueToScreen;

            if (valueToScreen != null)
            {
                double y;
                foreach (var t in ticks)
                {
                    y = valueToScreen(t);
                    LineGeometry line = new LineGeometry(new Point(0, y), new Point(width, y));
                    group.Children.Add(line);
                }

            }
            else  // if (valueToScreen == null)
            {
                foreach (var y in ticks)
                {
                    LineGeometry line = new LineGeometry(new Point(0, y), new Point(width, y));
                    group.Children.Add(line);
                }
            }
            lines.Data = group;
            Root.Children.Add(lines);
        }

    }
}
