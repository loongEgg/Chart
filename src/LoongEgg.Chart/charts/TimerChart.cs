using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using LoongEgg.Data;

namespace LoongEgg.Chart
{
    [TemplatePart(Name = nameof(PART_Chart), Type = typeof(Chart))]
    public class TimerChart : Control, ITimerChart
    {
        private static Clock Clock = Clock.Singleton;

        // TODO: add/remove dynamic as DataSeriesCollection.CollectionChanged or ObservableCollection<Signal>
        public Chart PART_Chart { get; internal set; }

        private List<PolylineFigure> Figures = new List<PolylineFigure>();
        private List<DataSeries> DataSeriesCollection = new List<DataSeries>();

        // TODO: use DataSeriesCollection to replace this
        private DataSeries DataSeries = new DataSeries();

        public RangeFilter Filter { get; private set; } = new RangeFilter(new Range(30, 60), new Range(-50, 50));

        #region ctor and intializing

        public TimerChart()
        {

            SetCurrentValue(HorizontalMajorTicksProperty, new double[] { 0, 25, 50, 75, 100, 125, 150, 175, 200 });

            var ticks = new List<double>();
            for (int i = 0; i < 200; i += 5)
                ticks.Add(i);
            SetCurrentValue(HorizontalMinorTicksProperty, ticks);

            SetCurrentValue(VerticalMajorTicksProperty, new double[] { -30, -20, -10, 0, 10, 20, 30 });
            SetCurrentValue(TimeRangeProperty, new Range(-30, 60));
            SetCurrentValue(ValueRangeProperty, new Range(-50, 50));
            SetCurrentValue(SignalProperty, Signal.SinSignal);

            var signalCollection = new ObservableCollection<Signal>();
            signalCollection.Add(Signal.SinSignal);
            signalCollection.Add(Signal.CosSignal);
            signalCollection.Add(Signal.SquareSignal);
            SetCurrentValue(SignalCollectionProperty, signalCollection);

            int lastMinute = Clock.LastMinute;
            Clock.Tick += (s, e) =>
            {
                if (Clock.LastMinute - lastMinute >= 1)
                {
                    lastMinute = Clock.LastMinute;
                    // ResetDataSeries();
                    foreach (var dataSeries in DataSeriesCollection)
                    {
                        var tmp = Filter.Filtering(dataSeries.ToList());
                        dataSeries.Reset(tmp.Select(p => new Data.Point(p.X - 60, p.Y)));
                    }
                }
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

                var signalCollection = new ObservableCollection<Signal>();
                signalCollection.Add(Signal.SinSignal);
                signalCollection.Add(Signal.CosSignal);
                signalCollection.Add(Signal.SquareSignal);
                this.SignalCollection = signalCollection;
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
                new PropertyMetadata(new Range(-100, 100), OnRangeChanged)
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
                    self.ResetAutoFixTicks();
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

        // TODO: Fix auto fix algorithms
        /// <summary>
        /// 使用自适应算法重置刻度
        /// </summary>
        private void ResetAutoFixTicks()
        {
            List<double> ticks;

            ticks = new List<double>();
            for (double i = TimeRange.Min; i <= TimeRange.Max; i += 5)
                ticks.Add(i);
            HorizontalMajorTicks = ticks;

            ticks = new List<double>();
            for (double i = TimeRange.Min; i <= TimeRange.Max; i += 1)
                ticks.Add(i);
            HorizontalMinorTicks = ticks;

            ticks = new List<double>();
            for (double i = ValueRange.Min; i <= ValueRange.Max; i += 10)
                ticks.Add(i);
            VerticalMajorTicks = ticks;
        }


        [Description("")]
        public ObservableCollection<Signal> SignalCollection
        {
            get { return (ObservableCollection<Signal>)GetValue(SignalCollectionProperty); }
            set { SetValue(SignalCollectionProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="SignalCollection"/>
        /// </summary>
        public static readonly DependencyProperty SignalCollectionProperty = DependencyProperty.Register
            (
                nameof(SignalCollection),
                typeof(ObservableCollection<Signal>),
                typeof(TimerChart),
                new PropertyMetadata(
                    default(ObservableCollection<Signal>),
                    OnSignalCollectionChanged)
            );

        private static void OnSignalCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as TimerChart;
            if (self == null) return;

            var collection = e.OldValue as ObservableCollection<Signal>;
            if (collection != null)
            {
                self.DataSeriesCollection.Clear();
                if (self.PART_Chart != null && self.PART_Chart.Children != null)
                {
                    foreach (var child in self.PART_Chart.Children)
                    {
                        if (child is PolylineFigure)
                            self.PART_Chart.Children.Remove(child);
                    }
                }
            }

            collection = e.NewValue as ObservableCollection<Signal>;
            if (collection != null)
            {
                if (self.PART_Chart != null && self.PART_Chart.Children != null)
                {
                    foreach (var signal in collection)
                    {
                        var dataSeries = new DataSeries();
                        self.PART_Chart.Children.Add(new PolylineFigure() { DataSeries = dataSeries });
                        self.DataSeriesCollection.Add(dataSeries);
                        signal.ValueChanged += (ss, ee) =>
                        {
                            dataSeries.Add(new Data.Point(Clock.TimeStamp, (ss as Signal).Value));
                        };
                    }
                }
            }
        }


        // TODO: use ObservableCollection<Signal> to replace
        [Description("")]
        public Signal Signal
        {
            get { return (Signal)GetValue(SignalProperty); }
            set { SetValue(SignalProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="Signal"/>
        /// </summary>
        public static readonly DependencyProperty SignalProperty = DependencyProperty.Register
            (
                nameof(Signal),
                typeof(Signal),
                typeof(TimerChart),
                new PropertyMetadata(default(Signal), OnSignalChanged)
            );

        private static void OnSignalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as TimerChart;
            if (self == null) return;

            Signal signal;

            signal = e.OldValue as Signal;
            if (signal != null)
                signal.PropertyChanged -= self.Signal_PropertyChanged;

            signal = e.NewValue as Signal;
            if (signal != null)
                signal.PropertyChanged += self.Signal_PropertyChanged;

        }

        private void ResetFilterRange()
        {
            Filter.Xrange = new Range(TimeRange.Max + TimeRange.Min, TimeRange.Max);
            Filter.Yrange = ValueRange;
        }

        private void Signal_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Signal.Value))
                AddNewValue(Signal.Value);
        }

        #endregion

        public void AddNewValue(double val)
        {
            DataSeries.Add(new Data.Point(Clock.TimeStamp, val));
        }

        public void ResetDataSeries()
        {
            if (Filter == null) throw new ArgumentNullException(nameof(Filter));

            Filter.Filtering(DataSeries.ToList());
            var list = Filter.Result.Select(p => new Data.Point(p.X - TimeRange.Max, p.Y));
            DataSeries.Reset(list);
        }

        void OnFilterResultChanged() => DataSeries.Reset(Filter.Result);

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

            //PolylineFigure figure = new PolylineFigure();
            //figure.DataSeries = this.DataSeries;
            //chart.Children.Add(figure);
        }
    }
}
