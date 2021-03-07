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

        public DataGroup DataGroup { get; } = new DataGroup();

        public RangeFilter Filter { get; private set; } = new RangeFilter(new Range(30, 60), new Range(-50, 50));

        #region ctor and intializing

        public TimerChart()
        {
            Clock.MinuteTick += (s, e) =>
            {
                foreach (var dataSeries in DataGroup)
                {
                    var tmp = Filter.Filtering(dataSeries.ToList());
                    dataSeries.Reset(tmp.Select(p => new Data.Point(p.X - TimeRange.Max, p.Y)));
                }
            };
            Loaded += (s, e) =>
            {
                ResetTimeAxis();
                ResetValueAxis();
                ResetFilterRange();
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
        }
        #endregion

        #region dependency properties and change handler

        [Description("刻度自适应")]
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
                new PropertyMetadata(true, OnAxisKeyParametersChanged)
            );

        private bool IsSettingAxisParameter = false;

        /// <summary>
        /// 当坐标轴关键参数改变时
        /// </summary>
        /// <param name="d"><see cref="Chart"/></param>
        /// <param name="e">Range, or TicksCount</param>
        private static void OnAxisKeyParametersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as TimerChart;
            if (self == null) return;
            if (self.AutoFixTicks == false) return;

            // 水平刻度改变
            if (e.Property == TimeRangeProperty || e.Property == HorizontalMajorTicksCountProperty || e.Property == HorizontalMinorTicksScalarProperty)
            {
                self.ResetTimeAxis();
                if (e.Property == TimeRangeProperty)
                {
                    self.ResetFilterRange();
                }
            }
            // 垂直刻度改变
            if (e.Property == ValueRangeProperty || e.Property == VerticalMajorTicksCountsProperty || e.Property == VerticalMinorTicksScalarProperty)
            {
                self.ResetValueAxis();

                if (e.Property == ValueRangeProperty)
                {
                    self.ResetFilterRange();
                }
            }
        }

        private void ResetTimeAxis()
        {
            if (this.TimeRange == null || this.HorizontalMajorTicksCount == 0) return;

            if (this.IsSettingAxisParameter == true) return;
            this.IsSettingAxisParameter = true;

            Range range;
            double[] ticks;

            // TODO: Ticks should be outside the range
            AutomaticTick.RangeTicksFix(this.TimeRange, this.HorizontalMajorTicksCount, out range, out ticks);
            this.TimeRange = range;
            this.HorizontalMajorTicks = ticks;

            if (this.HorizontalMinorTicksScalar > 1)
                AutomaticTick.TicksRespacing(ticks, this.HorizontalMinorTicksScalar, out ticks);
            else
                ticks = new double[0];
            this.HorizontalMinorTicks = ticks;

            this.IsSettingAxisParameter = false;
        }

        private void ResetValueAxis()
        {
            if (this.ValueRange == null || this.VerticalMajorTicksCounts < 2) return;

            if (this.IsSettingAxisParameter == true) return;
            this.IsSettingAxisParameter = true;
            Range range;
            double[] ticks;

            AutomaticTick.RangeTicksFix(this.ValueRange, this.VerticalMajorTicksCounts, out range, out ticks);
            this.ValueRange = range;
            this.VerticalMajorTicks = ticks;

            this.IsSettingAxisParameter = false;

            if (this.VerticalMinorTicksScalar > 1)
                AutomaticTick.TicksRespacing(ticks, this.VerticalMinorTicksScalar, out ticks);
            else
                ticks = new double[0];
            this.VerticalMinorTicks = ticks;
        }

        /*-------------------------------- horizontal properties --------------------------------*/
        /// <summary>
        /// 水平刻度的时间范围(s) = -30, 60
        /// </summary>
        /// <remarks>
        ///     protected set
        /// </remarks>
        [Description("水平刻度的时间范围(s)")]
        public Range TimeRange
        {
            get { return (Range)GetValue(TimeRangeProperty); }
            protected set { SetValue(TimeRangeProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="TimeRange"/>
        /// </summary>
        public static readonly DependencyProperty TimeRangeProperty = DependencyProperty.Register
            (
                nameof(TimeRange),
                typeof(Range),
                typeof(TimerChart),
                new PropertyMetadata(new Range(-30, 60), OnAxisKeyParametersChanged)
            );

        [Description("水平主刻度数目")]
        public uint HorizontalMajorTicksCount
        {
            get { return (uint)GetValue(HorizontalMajorTicksCountProperty); }
            set { SetValue(HorizontalMajorTicksCountProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="HorizontalMajorTicksCount"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalMajorTicksCountProperty = DependencyProperty.Register
            (
                nameof(HorizontalMajorTicksCount),
                typeof(uint),
                typeof(TimerChart),
                new PropertyMetadata((uint)18, OnAxisKeyParametersChanged, (s, e) => uint.Parse(e.ToString()) < 2 ? (uint)2 : uint.Parse(e.ToString()))
            );

        [Description("水平主刻度")]
        [EditorBrowsable(EditorBrowsableState.Never)]
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


        [Description("次要刻度相对主要刻度的倍数, ＞1或＝0")]
        public uint HorizontalMinorTicksScalar
        {
            get { return (uint)GetValue(HorizontalMinorTicksScalarProperty); }
            set { SetValue(HorizontalMinorTicksScalarProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="HorizontalMinorTicksScalar"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalMinorTicksScalarProperty = DependencyProperty.Register
            (
                nameof(HorizontalMinorTicksScalar),
                typeof(uint),
                typeof(TimerChart),
                new PropertyMetadata((uint)5, OnAxisKeyParametersChanged, (s, e) => uint.Parse(e.ToString()) <= 1 ? (uint)0 : uint.Parse(e.ToString()))
            );

        [Description("水平次刻度")]
        [EditorBrowsable(EditorBrowsableState.Never)]
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
        /*-------------------------------- horizontal properties --------------------------------*/

        /*--------------------------------- vertical properties ---------------------------------*/
        [Description("垂直数据范围")]
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
                new PropertyMetadata(new Range(-1, 1), OnAxisKeyParametersChanged)
            );


        [Description("垂直主要刻度数")]
        public uint VerticalMajorTicksCounts
        {
            get { return (uint)GetValue(VerticalMajorTicksCountsProperty); }
            set { SetValue(VerticalMajorTicksCountsProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="VerticalMajorTicksCounts"/>
        /// </summary>
        public static readonly DependencyProperty VerticalMajorTicksCountsProperty = DependencyProperty.Register
            (
                nameof(VerticalMajorTicksCounts),
                typeof(uint),
                typeof(TimerChart),
                new PropertyMetadata((uint)8, OnAxisKeyParametersChanged, (s, e) => uint.Parse(e.ToString()) < 2 ? (uint)2 : uint.Parse(e.ToString()))
            );


        [Description("垂直次要刻度相对主要刻度的比例")]
        public uint VerticalMinorTicksScalar
        {
            get { return (uint)GetValue(VerticalMinorTicksScalarProperty); }
            set { SetValue(VerticalMinorTicksScalarProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="VerticalMinorTicksScalar"/>
        /// </summary>
        public static readonly DependencyProperty VerticalMinorTicksScalarProperty = DependencyProperty.Register
            (
                nameof(VerticalMinorTicksScalar),
                typeof(uint),
                typeof(TimerChart),
                new PropertyMetadata((uint)5, OnAxisKeyParametersChanged, (s, e) => uint.Parse(e.ToString()) <= 1 ? (uint)0 : uint.Parse(e.ToString()))
            );


        [Description("垂直主刻度")]
        [EditorBrowsable(EditorBrowsableState.Never)]
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


        [Description("垂直次要刻度")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<double> VerticalMinorTicks
        {
            get { return (IEnumerable<double>)GetValue(VerticalMinorTicksProperty); }
            set { SetValue(VerticalMinorTicksProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="VerticalMinorTicks"/>
        /// </summary>
        public static readonly DependencyProperty VerticalMinorTicksProperty = DependencyProperty.Register
            (
                nameof(VerticalMinorTicks),
                typeof(IEnumerable<double>),
                typeof(TimerChart),
                new PropertyMetadata(default(IEnumerable<double>))
            );

        /*--------------------------------- vertical properties ---------------------------------*/
        [Description("信号集合")]
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
                foreach (var item in newValue.Signals)
                {
                    var dataSeries = new DataSeries();
                    item.ValueChanged += (s, e) =>
                    {
                        OnSignalValueChanged(self, item, dataSeries);
                    };
                    self.DataGroup.Add(dataSeries);
                }
            }
        }

        /// <summary>
        /// 信号变化时的处理方法
        /// </summary>
        /// <param name="chart">信号附属的图表</param>
        /// <param name="signal">信号</param>
        /// <param name="dataSeries">信号附属的数据序列</param>
        private static void OnSignalValueChanged(TimerChart chart, Signal signal, DataSeries dataSeries)
        {
            dataSeries.Add(new Data.Point(Clock.TimeStamp, signal.Value));

            if (signal.Value > chart.ValueRange.Max)
            {
                var min = chart.ValueRange.Min;
                chart.ValueRange = new Range(min, signal.Value);
            }
            else if (signal.Value < chart.ValueRange.Min)
            {
                var max = chart.ValueRange.Max;
                chart.ValueRange = new Range(signal.Value, max);
            }
        }

        private void ResetFilterRange()
        {
            Filter.Xrange = new Range(TimeRange.Max + TimeRange.Min, TimeRange.Max);
            Filter.Yrange = ValueRange;
        }

        #endregion

    }
}
