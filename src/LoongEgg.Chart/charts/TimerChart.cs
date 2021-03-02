using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using LoongEgg.Data;
using LoongEgg.Log;

namespace LoongEgg.Chart
{
    [TemplatePart(Name = nameof(PART_Chart), Type = typeof(Chart))]
    public class TimerChart : Control, ITimerChart
    {
        private static Clock Clock = Clock.Singleton;
        public Chart PART_Chart { get; internal set; }

        static TimerChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimerChart), new FrameworkPropertyMetadata(typeof(TimerChart)));
        }

        public TimerChart()
        {
            SetCurrentValue(IntervalProperty, Intervals.Sec120);
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


        [Description("")]
        public Intervals Interval
        {
            get { return (Intervals)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="Interval"/>
        /// </summary>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            (
                nameof(Interval),
                typeof(Intervals),
                typeof(TimerChart),
                new PropertyMetadata(Intervals.Sec60, OnParameterChanged)
            );

        [Description("")]
        public Range TimeRange
        {
            get { return (Range)GetValue(TimeRangeProperty); }
            private set { SetValue(TimeRangeProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="TimeRange"/>
        /// </summary>
        public static readonly DependencyProperty TimeRangeProperty = DependencyProperty.Register
            (
                nameof(TimeRange),
                typeof(Range),
                typeof(TimerChart),
                new PropertyMetadata(new Range(-10, 20))
            );


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


        private static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as TimerChart;
            if (self == null) return;
            if (e.Property == IntervalProperty)
            {
                List<double> majorTicks = new List<double>();
                List<double> minorTicks = new List<double>();
                if (self.Interval == Intervals.Sec10)
                {
                    int min = -5;
                    self.TimeRange = new Range(min, (int)self.Interval);

                    for (int i = min; i < (int)self.Interval; i += 1)
                        majorTicks.Add(i);

                    for (double i = min; i < (int)self.Interval; i += 0.5)
                        minorTicks.Add(i);
                }
                else if (self.Interval == Intervals.Sec20
                   || self.Interval == Intervals.Sec30)
                {
                    int min = -10;
                    self.TimeRange = new Range(min, (int)self.Interval);

                    for (int i = min; i < (int)self.Interval; i += 5)
                        majorTicks.Add(i);

                    for (double i = min; i < (int)self.Interval; i += 1)
                        minorTicks.Add(i);
                }
                else if (self.Interval == Intervals.Sec60 || self.Interval == Intervals.Sec120)
                {
                    int min = -10;
                    self.TimeRange = new Range(min, (int)self.Interval);

                    for (int i = min; i < (int)self.Interval; i += 10)
                        majorTicks.Add(i);

                    for (double i = min; i < (int)self.Interval; i += 5)
                        minorTicks.Add(i);

                }
                self.HorizontalMajorTicks = majorTicks;
                self.HorizontalMinorTicks = minorTicks;
            }
        }

        private DataSeries DataSeries = new DataSeries();
        public void AddNewValue(double val)
        {

        }

        public void ResetHorizontalTicks()
        {

        }

        public void ResetDataSeries()
        {

        }
    }
}
