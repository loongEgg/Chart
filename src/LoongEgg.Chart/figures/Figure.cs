using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using LoongEgg.Data;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace LoongEgg.Chart
{
    // TODO: make Figure not implement from ChartElement, but Figure itself is the base class.
    //       so when Children property of Chart changed, it will not influence the figures.
    public abstract class Figure : ChartElement, IFigure
    {
        public ValueToScreen HorizontalValueToScreen { get; internal set; } = new ValueToScreen((v) => v);
        public ValueToScreen VerticalValueToScreen { get; internal set; } = new ValueToScreen((v) => v);

        [Description("")]
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="Stroke"/>
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register
            (
                nameof(Stroke),
                typeof(Brush),
                typeof(Figure),
                new PropertyMetadata(Brushes.Red, OnParameterChanged)
            );


        [Description("")]
        public float StrokeThickness
        {
            get { return (float)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="StrokeThickness"/>
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register
            (
                nameof(StrokeThickness),
                typeof(float),
                typeof(Figure),
                new PropertyMetadata(2f, OnParameterChanged)
            );


        [Description("")]
        public DataSeries DataSeries
        {
            get { return (DataSeries)GetValue(DataSeriesProperty); }
            set { SetValue(DataSeriesProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="DataSeries"/>
        /// </summary>
        public static readonly DependencyProperty DataSeriesProperty = DependencyProperty.Register
            (
                nameof(DataSeries),
                typeof(DataSeries),
                typeof(Figure),
                new PropertyMetadata(default(DataSeries), OnDataSeriesChanged)
            );
        

        private static void OnDataSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as Figure;
            if (self == null) return;
            var collection = e.OldValue as DataSeries;
            if (collection != null)
            {
                collection.CollectionChanged -= self.Collection_CollectionChanged;
                self.NormalizedPoints.Clear();
                self.Update();
            }

            collection = e.NewValue as DataSeries;
            if (collection != null)
            {
                self.InternalChange = true;
                foreach (var p in collection)
                {
                    self.NormalizedPoints.Add(self.Normalize(p.X, p.Y));
                }
                self.InternalChange = false;
                self.Update();
                collection.CollectionChanged += self.Collection_CollectionChanged;
            }
        }

        public override void OnInitializing()
        {
            var temp = new DataSeries();
            var random = new Random();

            for (int i = 0; i < 200; i += 10)
            {
                temp.Add(new Data.Point(i, random.Next(-30, 30)));
            }
            SetCurrentValue(DataSeriesProperty, temp);
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                int start = e.OldStartingIndex;
                int length = e.OldItems.Count;
                NormalizedPoints.RemoveRange(start, length);
                Update();
            }
            if(e.Action == NotifyCollectionChangedAction.Reset)
            {
                NormalizedPoints.Clear();
               
                foreach (Data.Point p in DataSeries)
                {
                    NormalizedPoints.Add(Normalize(p.X, p.Y));
                }
                Update();
            }
            if (e.NewItems != null)
            {
                foreach (Data.Point p in e.NewItems)
                {
                    NormalizedPoints.Add(Normalize(p.X, p.Y));
                }
                Update();
            }
        }

        public List<PointF> NormalizedPoints { get; protected set; } = new List<PointF>();

        public PointF Normalize(double x, double y)
            => new PointF(
                (float)HorizontalValueToScreen(x),
                (float)VerticalValueToScreen(y)
            );

        public override void OnContainerSet()
        {
            if(Container != null)
            {
                Container.ValueToScreenMethodChanged += (s, e) => ResetNormalizeMethods();
            }
            ResetPlacement();
            ResetNormalizeMethods();
            Update();
        }

        public void ResetNormalizeMethods()
        {
            if (Container == null) return;
            if (Container.HorizontalValueToScreen != null)
                HorizontalValueToScreen = Container.HorizontalValueToScreen;
            if (Container.VerticalValueToScreen != null)
                VerticalValueToScreen = Container.VerticalValueToScreen;
            ResetNormalizePointsFromDataSeries();
        }

        public void ResetNormalizePointsFromDataSeries()
        {
            NormalizedPoints.Clear();
            foreach (var p in DataSeries)
            {
                NormalizedPoints.Add(Normalize(p.X, p.Y));
            }
        }


        public override void ResetPlacement()
        {
            if (Container == null) return;
            (Parent as Panel)?.Children.Remove(this);
            Container.PART_FigureContainer?.Children.Add(this);
        }

    }
}
