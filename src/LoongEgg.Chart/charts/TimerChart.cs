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

                PART_Chart.HorizontalRange = new Range(-20, 60);
                List<double> ticks = new List<double>();
                for (int i = -20; i <= 60; i += 5)
                {
                    ticks.Add(i);
                }
                PART_Chart.HorizontalMajorTicks = ticks;

                ticks = new List<double>();
                for (int i = -20; i <= 60; i += 1)
                {
                    ticks.Add(i);
                }
                PART_Chart.HorizontalMinorTicks = ticks;
            }
        }
        
        private static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as TimerChart;
            if (self == null) return;

        }

        private DataSeries DataSeries = new DataSeries();
        public void AddNewValue(double val)
        {
            DataSeries.Add(new Data.Point(Clock.TimeStamp, val));
        }

        public void ResetDataSeries()
        {

        }
    }
}
