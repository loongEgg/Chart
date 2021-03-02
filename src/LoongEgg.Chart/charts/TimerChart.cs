using System;
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
                new PropertyMetadata(default(Intervals), OnParameterChanged)
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
                new PropertyMetadata(new Range(-10, 20))
            );



        private static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as TimerChart;
            if (self == null) return;
            if (e.Property == IntervalProperty)
            {
                if (self.Interval == Intervals.Sec10
                    || self.Interval == Intervals.Sec20
                    || self.Interval == Intervals.Sec30)
                {
                    self.TimeRange = new Range(-10, (int)self.Interval);
                }
                else if (self.Interval == Intervals.Sec60)
                {
                    self.TimeRange = new Range(-20, 60);

                }
                else if (self.Interval == Intervals.Sec120)
                {
                    self.TimeRange = new Range(-20, 120);
                }
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
