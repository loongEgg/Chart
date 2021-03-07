using LoongEgg.Data;
using LoongEgg.Log;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System;
using System.Windows.Media;
using System.Windows.Data;

namespace LoongEgg.Chart
{
    [TemplatePart(Name = nameof(PART_Left), Type = typeof(Panel))]
    [TemplatePart(Name = nameof(PART_Right), Type = typeof(Panel))]
    [TemplatePart(Name = nameof(PART_Top), Type = typeof(Panel))]
    [TemplatePart(Name = nameof(PART_Bottom), Type = typeof(Panel))]
    [TemplatePart(Name = nameof(PART_Center), Type = typeof(Panel))]
    [TemplatePart(Name = nameof(PART_FigureBorder), Type = typeof(Border))]
    [TemplatePart(Name = nameof(PART_FigureContainer), Type = typeof(Panel))]
    [ContentProperty(nameof(Children))]

    public class Chart : Control, IChart
    {
        #region default resources

        private readonly static List<SolidColorBrush> DefaultBrushes = new List<SolidColorBrush>()
        {
            Brushes.Red,
            Brushes.Green,
            Brushes.Blue
        };

        #endregion

        #region events

        public event EventHandler ValueToScreenAlgorithmsChanged;

        #endregion

        #region ui parts

        public Panel PART_Left { get; private set; }
        public Panel PART_Right { get; private set; }
        public Panel PART_Top { get; private set; }
        public Panel PART_Bottom { get; private set; }
        public Panel PART_Center { get; private set; }
        public Border PART_FigureBorder { get; private set; }
        public Panel PART_FigureContainer { get; private set; }
        #endregion

        #region properties

        public bool InternalChange { get; set; }

        #endregion

        #region value to screen algorithms

        public ValueToScreen HorizontalValueToScreen { get; set; }

        public ValueToScreen VerticalValueToScreen { get; set; }

        public void ResetValueToScreenAlgorithms()
        {
            if (HorizontalRange == null || VerticalRange == null) return;
            if (HorizontalRange.Distance <= 0 || VerticalRange.Distance <= 0) return;
            if (PART_Center == null) return;
            var size = PART_Center.RenderSize;
            if (size.Width == 0 || size.Height == 0) return;
            Logger.Dbug("value to screen reset", true);
            double kx = size.Width / HorizontalRange.Distance;
            double ky = size.Height / VerticalRange.Distance;
            HorizontalValueToScreen = new ValueToScreen(v => kx * (v - HorizontalRange.Min));
            VerticalValueToScreen = new ValueToScreen(v => ky * (VerticalRange.Max - v));
            ValueToScreenAlgorithmsChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region ctor and initializing

        static Chart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
        }

        public Chart()
        {
            Children = new ObservableCollection<object>();
            SetCurrentValue(HorizontalRangeProperty, new Range(0, 200));
            SetCurrentValue(VerticalRangeProperty, new Range(-30, 30));
            SetCurrentValue(HorizontalMajorTicksProperty, new double[] { 0, 25, 50, 75, 100, 125, 150, 175, 200 });
            SetCurrentValue(VerticalMajorTicksProperty, new double[] { -30, -20, -10, 0, 10, 20, 30 });

            Loaded += (s, e) =>
            {
                ResetOnChildrenChanged(this, Children, Children);
                ResetOnDataGroupChanged(this, DataGroup, DataGroup);
                PART_Center.SizeChanged += (ss, ee) => ResetValueToScreenAlgorithms();
            };
        }

        /// <summary>
        /// apply the style defined in Generic.xaml
        /// </summary>
        /// <remarks>
        ///     raised after the ctor, but before Loaded event.
        ///     so if you want to add sth to the ui parts. add
        ///     in the <code>Loaded += (s,e) => {}</code>
        /// </remarks>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Left = GetTemplateChild(nameof(PART_Left)) as Panel;
            PART_Right = GetTemplateChild(nameof(PART_Right)) as Panel;
            PART_Top = GetTemplateChild(nameof(PART_Top)) as Panel;
            PART_Bottom = GetTemplateChild(nameof(PART_Bottom)) as Panel;
            PART_Center = GetTemplateChild(nameof(PART_Center)) as Panel;
            PART_FigureBorder = GetTemplateChild(nameof(PART_FigureBorder)) as Border;
            PART_FigureContainer = GetTemplateChild(nameof(PART_FigureContainer)) as Panel;


            var parent = Helper.TemplateHelper.GetParent<TimerChart>(this);
            if (parent != null)
            {
                Binding binding;

                binding = new Binding(nameof(TimerChart.TimeRange)) { Source = parent };
                SetBinding(HorizontalRangeProperty, binding);

                binding = new Binding(nameof(TimerChart.HorizontalMajorTicks)) { Source = parent };
                SetBinding(HorizontalMajorTicksProperty, binding);

                binding = new Binding(nameof(TimerChart.HorizontalMinorTicks)) { Source = parent };
                SetBinding(HorizontalMinorTicksProperty, binding);

                binding = new Binding(nameof(TimerChart.ValueRange)) { Source = parent };
                SetBinding(VerticalRangeProperty, binding);

                binding = new Binding(nameof(TimerChart.VerticalMajorTicks)) { Source = parent };
                SetBinding(VerticalMajorTicksProperty, binding);

                binding = new Binding(nameof(TimerChart.VerticalMinorTicks)) { Source = parent };
                SetBinding(VerticalMinorTicksProperty, binding);

                binding = new Binding(nameof(TimerChart.DataGroup)) { Source = parent };
                SetBinding(DataGroupProperty, binding);
            }

#if DEBUG
            if (PART_Left == null
                || PART_Right == null
                || PART_Top == null
                || PART_Bottom == null
                || PART_Center == null
                || PART_FigureBorder == null
                || PART_FigureContainer == null)
                Debugger.Break();
#endif           
        }

        #endregion

        #region dependency properties and change handler

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public ObservableCollection<object> Children
        {
            get { return (ObservableCollection<object>)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="Children"/>
        /// </summary>
        public static readonly DependencyProperty ChildrenProperty =
            DependencyProperty.Register(
                nameof(Children),
                typeof(ObservableCollection<object>),
                typeof(Chart),
                new PropertyMetadata(
                    null,
                    (s, e) =>
                    {
                        ResetOnChildrenChanged(s as Chart, e.OldValue as ObservableCollection<object>, e.NewValue as ObservableCollection<object>);
                    }));

        private static void ResetOnChildrenChanged(Chart self, ObservableCollection<object> oldValue, ObservableCollection<object> newValue)
        {
            if (self == null) return;
            if (oldValue != null)
            {
                foreach (ChartElement item in oldValue)
                {
                    if (item != null)
                    {
                        (item.Parent as Panel)?.Children.Remove(item);
                        item.Container = null;
                    }
                }
                oldValue.CollectionChanged -= self.Children_CollectionChanged;
            }
            if (newValue != null)
            {
                foreach (ChartElement item in newValue)
                {
                    if (item != null) item.Container = self;
                    item.ResetPlacement();
                    item.Update();
                    Logger.Dbug($"{item.GetType()} add", true);
                }
                newValue.CollectionChanged += self.Children_CollectionChanged;
            }
        }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var oldItems = e.OldItems;
            var newItems = e.NewItems;
            if (oldItems != null)
            {
                foreach (ChartElement item in oldItems)
                {
                    if (item != null)
                    {
                        (item.Parent as Panel)?.Children.Remove(item);
                        item.Container = null;
                    }
                }
            }
            if (newItems != null)
            {
                foreach (ChartElement item in newItems)
                {
                    AddChild(this, item);
                }
            }
        }

        /// <summary>
        /// add <see cref="ChartElement"/> to a <see cref="Chart"/>
        /// </summary>
        /// <param name="self">the chart to contain the element</param>
        /// <param name="element">the element should be added the chart</param>
        private static void AddChild(Chart self, ChartElement element)
        {
            if (element == null || self == null) return;
            element.Container = self;
            element.ResetPlacement();
            element.Update();
            Logger.Dbug($"{element.GetType()}[{element.GetHashCode()}] added in Chart[{self.GetHashCode()}]");
        }

        /// <summary>
        /// remove <see cref="ChartElement"/> from a <see cref="Chart"/>
        /// </summary>
        /// <param name="self">the chart contain the element</param>
        /// <param name="element">the element to be removed</param>
        private static void RemoveChild(Chart self, ChartElement element)
        {
            if (element == null || self == null) return;
            (element.Parent as Panel)?.Children.Remove(element);
            element.Container = null;
            Logger.Dbug($"{element.GetType()}[{element.GetHashCode()}] removed from Chart[{self.GetHashCode()}]");
        }

        /// <summary>
        /// the DataSeries collection should be paint to Figures
        /// </summary>
        [Description("the DataSeries collection should be paint to Figures")]
        public DataGroup DataGroup
        {
            get { return (DataGroup)GetValue(DataGroupProperty); }
            set { SetValue(DataGroupProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="DataGroup"/>
        /// </summary>
        public static readonly DependencyProperty DataGroupProperty = DependencyProperty.Register
            (
                nameof(DataGroup),
                typeof(DataGroup),
                typeof(Chart),
                new PropertyMetadata(default(DataGroup), OnDataGroupChanged)
            );

        private static void OnDataGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as Chart;
            if (self == null) return;

            ResetOnDataGroupChanged(
                self,
                e.OldValue as DataGroup,
                e.NewValue as DataGroup);
        }

        private static void ResetOnDataGroupChanged(Chart self, DataGroup oldValue, DataGroup newValue)
        {
            if (self.PART_FigureContainer == null || self.PART_FigureContainer.Children == null) return;
            self.PART_FigureContainer.Children.Clear();

            if (oldValue != null)
            {
                oldValue.CollectionChanged -= self.DataGroup_CollectionChanged;
            }
            if (newValue != null)
            {
                foreach (var item in newValue)
                {
                    var id = self.PART_FigureContainer.Children.Count;
                    var stroke = self.Foreground; /* assign to foreground the stroke */

                    /* if has default brushes use it as the item add */
                    if (id < Chart.DefaultBrushes.Count)
                    {
                        stroke = Chart.DefaultBrushes[id];
                    }

                    var figure = new PolylineFigure() { DataSeries = item, Stroke = stroke };
                    figure.Container = self;
                }
                newValue.CollectionChanged += self.DataGroup_CollectionChanged;
            }
        }

        private void DataGroup_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                int start = e.OldStartingIndex;
                int length = e.OldItems.Count;
                PART_FigureContainer?.Children.RemoveRange(start, length);
            }
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                PART_FigureContainer?.Children.Clear();
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var series = item as DataSeries;
                    if (series != null)
                    {
                        int id = PART_FigureContainer.Children.Count;
                        var stroke = Foreground; /* assign to foreground the stroke */

                        /* if has default brushes use it as the item add */
                        if (id < Chart.DefaultBrushes.Count)
                        {
                            stroke = Chart.DefaultBrushes[id];
                        }

                        var figure = new PolylineFigure() { DataSeries = series, Stroke = stroke };
                        figure.Container = this;
                    }
                }
            }
        }



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
                typeof(Chart),
                new PropertyMetadata(default(IEnumerable<double>)));

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
                typeof(Chart),
                new PropertyMetadata(default(IEnumerable<double>)));

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
                typeof(Chart),
                new PropertyMetadata(default(IEnumerable<double>)));

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
                typeof(Chart),
                new PropertyMetadata(default(IEnumerable<double>)));

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Range HorizontalRange
        {
            get { return (Range)GetValue(HorizontalRangeProperty); }
            set { SetValue(HorizontalRangeProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="HorizontalRange"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalRangeProperty =
            DependencyProperty.Register(
                nameof(HorizontalRange),
                typeof(Range),
                typeof(Chart),
                new PropertyMetadata(default(Range), OnRangeChanged));


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Range VerticalRange
        {
            get { return (Range)GetValue(VerticalRangeProperty); }
            set { SetValue(VerticalRangeProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="VerticalRange"/>
        /// </summary>
        public static readonly DependencyProperty VerticalRangeProperty =
            DependencyProperty.Register(
                nameof(VerticalRange),
                typeof(Range),
                typeof(Chart),
                new PropertyMetadata(default(Range), OnRangeChanged));


        private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as IChart)?.ResetValueToScreenAlgorithms();

        #endregion

    }
}
