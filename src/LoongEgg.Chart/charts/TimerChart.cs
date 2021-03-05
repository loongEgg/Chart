using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using LoongEgg.Data;
using LoongEgg.Filter;

namespace LoongEgg.Chart
{
    [TemplatePart(Name = nameof(PART_Chart), Type = typeof(Chart))]
    [ContentProperty(nameof(SignalGroup))]
    public class TimerChart : Control, ITimerChart
    {
        private static Clock Clock = Clock.Singleton;

        public Chart PART_Chart { get; internal set; }

        public bool InternalChanging { get; set; } = false;

        private DataGroup DataGroup = new DataGroup();

        public RangeFilter Filter { get; private set; } = new RangeFilter(new Range(30, 60), new Range(-50, 50));

        #region ctor and intializing

        public TimerChart()
        {

            SetCurrentValue(HorizontalMajorTicksProperty, new double[] { 0, 25, 50, 75, 100, 125, 150, 175, 200 });

            var ticks = new List<double>();
            for (int i = 0; i < 200; i += 5)
                ticks.Add(i);
            SetCurrentValue(HorizontalMinorTicksProperty, ticks);

            SetCurrentValue(VerticalMajorTicksProperty, new double[] { -10, -5, 0, 5, 10 });
            SetCurrentValue(TimeRangeProperty, new Range(-30, 60));
            SetCurrentValue(ValueRangeProperty, new Range(-10, 10));

            int lastMinute = Clock.LastMinute;
            Clock.Tick += (s, e) =>
            {
                if (Clock.LastMinute - lastMinute >= 1)
                {
                    lastMinute = Clock.LastMinute;

                    foreach (var dataSeries in DataGroup)
                    {
                        var tmp = Filter.Filtering(dataSeries.ToList());
                        dataSeries.Reset(tmp.Select(p => new Data.Point(p.X - TimeRange.Max, p.Y)));
                    }
                }
            };
            Loaded += (s, e) =>
            {
                ResetDataGroup(this, null, SignalGroup);
            };
        }

        static TimerChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimerChart), new FrameworkPropertyMetadata(typeof(TimerChart)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Chart = GetTemplateChild(nameof(PART_Chart)) as Chart;
            if (PART_Chart == null)
            {
                Debugger.Break();
            }
            else
            {
                OnAddChart(PART_Chart);
            }
        }
        #endregion

        #region dependency properties and change handler
        [Description("")]
        public bool AutoFixTicks
        {
            get { return (bool)GetValue(AutoFixTicksProperty); }
            set { SetValue(AutoFixTicksProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="AutoFixTicks"/>
        /// </summary>
        public static readonly DependencyProperty AutoFixTicksProperty = DependencyProperty.Register
            (
                nameof(AutoFixTicks),
                typeof(bool),
                typeof(TimerChart),
                new PropertyMetadata(true, OnRangeChanged)
            );

        [Description("")]
        public Range ValueRange
        {
            get { return (Range)GetValue(ValueRangeProperty); }
            set { SetValue(ValueRangeProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="ValueRange"/>
        /// </summary>
        public static readonly DependencyProperty ValueRangeProperty = DependencyProperty.Register
            (
                nameof(ValueRange),
                typeof(Range),
                typeof(TimerChart),
                new PropertyMetadata(null, OnRangeChanged)
            );

        [Description("")]
        public Range TimeRange
        {
            get { return (Range)GetValue(TimeRangeProperty); }
            set { SetValue(TimeRangeProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="TimeRange"/>
        /// </summary>
        public static readonly DependencyProperty TimeRangeProperty = DependencyProperty.Register
            (
                nameof(TimeRange),
                typeof(Range),
                typeof(TimerChart),
                new PropertyMetadata(new Range(-30, 60), OnRangeChanged)
            );

        private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as TimerChart;
            if (self == null) return;
            if (self.TimeRange != null && self.ValueRange != null)
            {
                self.ResetFilterRange();
                if (self.AutoFixTicks)
                    self.ResetAutoFixValueRangeTicks();
            }
        }

        [Description("")]
        public IEnumerable<double> HorizontalMajorTicks
        {
            get { return (IEnumerable<double>)GetValue(HorizontalMajorTicksProperty); }
            set { SetValue(HorizontalMajorTicksProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="HorizontalMajorTicks"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalMajorTicksProperty = DependencyProperty.Register
            (
                nameof(HorizontalMajorTicks),
                typeof(IEnumerable<double>),
                typeof(TimerChart),
                new PropertyMetadata(default(IEnumerable<double>))
            );

        [Description("")]
        public IEnumerable<double> HorizontalMinorTicks
        {
            get { return (IEnumerable<double>)GetValue(HorizontalMinorTicksProperty); }
            set { SetValue(HorizontalMinorTicksProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="HorizontalMinorTicks"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalMinorTicksProperty = DependencyProperty.Register
            (
                nameof(HorizontalMinorTicks),
                typeof(IEnumerable<double>),
                typeof(TimerChart),
                new PropertyMetadata(default(IEnumerable<double>))
            );

        [Description("")]
        public IEnumerable<double> VerticalMajorTicks
        {
            get { return (IEnumerable<double>)GetValue(VerticalMajorTicksProperty); }
            set { SetValue(VerticalMajorTicksProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="VerticalMajorTicks"/>
        /// </summary>
        public static readonly DependencyProperty VerticalMajorTicksProperty = DependencyProperty.Register
            (
                nameof(VerticalMajorTicks),
                typeof(IEnumerable<double>),
                typeof(TimerChart),
                new PropertyMetadata(default(IEnumerable<double>))
            );

        private bool IsSettingValueRangeTicks = false;
        /// <summary>
        /// 使用自适应算法重置刻度
        /// </summary>
        private void ResetAutoFixValueRangeTicks()
        {

            #region horizontal axis not set now

            List<double> ticks;

            ticks = new List<double>();
            for (double i = TimeRange.Min; i <= TimeRange.Max; i += 5)
                ticks.Add(i);
            HorizontalMajorTicks = ticks;

            ticks = new List<double>();
            for (double i = TimeRange.Min; i <= TimeRange.Max; i += 1)
                ticks.Add(i);
            HorizontalMinorTicks = ticks;

            #endregion

            if (IsSettingValueRangeTicks == true) return;
            IsSettingValueRangeTicks = true;

            if (PART_Chart != null && PART_Chart.PART_FigureContainer != null)
            {
                foreach (Figure child in PART_Chart.PART_FigureContainer.Children)
                {
                    if (child != null)
                        child.InternalChange = true;
                }
            }

            Range verticalRange;
            double[] verticalMajorTicks;

            AutomaticTick.RangeTicksFix(ValueRange, 8, out verticalRange, out verticalMajorTicks);
            ValueRange = verticalRange;
            VerticalMajorTicks = verticalMajorTicks;

            if (PART_Chart != null && PART_Chart.PART_FigureContainer.Children != null)
            {
                foreach (Figure child in PART_Chart.PART_FigureContainer.Children)
                {
                    if (child != null)
                    {
                        child.ResetNormalizeAlgorithms();
                        child.InternalChange = false;
                        child.Update();
                    }
                }
            }

            IsSettingValueRangeTicks = false;
        }


        [Description("")]
        public SignalGroup SignalGroup
        {
            get { return (SignalGroup)GetValue(SignalGroupProperty); }
            set { SetValue(SignalGroupProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="SignalGroup"/>
        /// </summary>
        public static readonly DependencyProperty SignalGroupProperty = DependencyProperty.Register
            (
                nameof(SignalGroup),
                typeof(SignalGroup),
                typeof(TimerChart),
                new PropertyMetadata(
                    default(SignalGroup),
                    OnSignalGroupChanged)
            );

        private static void OnSignalGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ResetDataGroup(d as TimerChart, e.OldValue as SignalGroup, e.NewValue as SignalGroup);

        private static void ResetDataGroup(TimerChart self, SignalGroup oldValue, SignalGroup newValue)
        {
            if (self == null) return;
            self.DataGroup.Clear();

            if (newValue != null)
            {
                foreach (var item in newValue)
                {
                    var dataSeries = new DataSeries();
                    item.ValueChanged += (s, e) =>
                    {
                        dataSeries.Add(new Data.Point(Clock.TimeStamp, item.Value));

                        if (item.Value > self.ValueRange.Max)
                        {
                            var min = self.ValueRange.Min;
                            self.ValueRange = new Range(min, item.Value);
                        }
                        else if (item.Value < self.ValueRange.Min)
                        {
                            var max = self.ValueRange.Max;
                            self.ValueRange = new Range(item.Value, max);
                        }
                    };
                    self.DataGroup.Add(dataSeries);
                }
            }
        }

        private void ResetFilterRange()
        {
            Filter.Xrange = new Range(TimeRange.Max + TimeRange.Min, TimeRange.Max);
            Filter.Yrange = ValueRange;
        }

        #endregion

        void OnAddChart(Chart chart)
        {
            Binding binding;
            binding = new Binding(nameof(TimeRange)) { Source = this };
            chart.SetBinding(Chart.HorizontalRangeProperty, binding);

            binding = new Binding(nameof(ValueRange)) { Source = this };
            chart.SetBinding(Chart.VerticalRangeProperty, binding);

            binding = new Binding(nameof(HorizontalMajorTicks)) { Source = this };
            chart.SetBinding(Chart.HorizontalMajorTicksProperty, binding);
            binding = new Binding(nameof(HorizontalMinorTicks)) { Source = this };
            chart.SetBinding(Chart.HorizontalMinorTicksProperty, binding);

            binding = new Binding(nameof(VerticalMajorTicks)) { Source = this };
            chart.SetBinding(Chart.VerticalMajorTicksProperty, binding);

            chart.DataGroup = this.DataGroup;
        }
    }
}
