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

        public event EventHandler ValueToScreenMethodChanged;

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

        public ValueToScreen HorizontalValueToScreen
        {
            get { return _HorizontalValueToScreen; }
            private set
            {
                _HorizontalValueToScreen = value;
                if (value != null)
                    ValueToScreenMethodChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private ValueToScreen _HorizontalValueToScreen;

        public ValueToScreen VerticalValueToScreen
        {
            get { return _VerticalValueToScreen; }
            private set
            {
                _VerticalValueToScreen = value;
                if (value != null)
                    ValueToScreenMethodChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private ValueToScreen _VerticalValueToScreen;

        public void ResetValueToScreenMethod()
        {
            if (HorizontalRange == null || VerticalRange == null) return;
            if (HorizontalRange.Distance <= 0 || VerticalRange.Distance <= 0) return;
            if (PART_Center == null) return;
            var size = PART_Center.RenderSize;
            if (size.Width == 0 || size.Height == 0) return;
            Logger.Dbug("reset", true);
            double kx = size.Width / HorizontalRange.Distance;
            double ky = size.Height / VerticalRange.Distance;
            HorizontalValueToScreen = new ValueToScreen(v => kx * (v - HorizontalRange.Min));
            VerticalValueToScreen = new ValueToScreen(v => ky * (VerticalRange.Max - v));
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
                if (Children != null)
                {
                    foreach (ChartElement item in Children)
                    {
                        if (item != null)
                        {
                            item.Container = this;
                            item.ResetPlacement();
                            item.Update();
                        }
                    }
                }

                PART_Center.SizeChanged += (ss, ee) => ResetValueToScreenMethod();
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
            if (DataGroup != null)
            {
                OnDataGroupReset(this, null, DataGroup);
            }
        }

        #endregion

        #region dependency properties and change handler

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
                        var self = s as Chart;
                        if (self == null) return;
                        var collection = e.NewValue as ObservableCollection<object>;
                        if (collection != null)
                        {
                            foreach (ChartElement item in collection)
                            {
                                if (item != null) item.Container = self;
                                Logger.Dbug($"{item.GetType()} add", true);
                            }
                            collection.CollectionChanged += self.Child_CollectionChanged;
                        }
                        // TODO:FIX
                        //collection = e.OldValue as ObservableCollection<object>;
                        //if (collection != null && self != null)
                        //{
                        //    foreach (var item in collection)
                        //    {
                        //        (item as IChartElement).RemoveContainer();
                        //        Logger.Dbug($"{item.GetType()} removed", true);
                        //    }
                        //    collection.CollectionChanged -= self.Collection_CollectionChanged;
                        //}
                    }));


        [Description("")]
        public ObservableCollection<DataSeries> DataGroup
        {
            get { return (ObservableCollection<DataSeries>)GetValue(DataGroupProperty); }
            set { SetValue(DataGroupProperty, value); }
        }
        /// <summary>
        /// Dependency property of <see cref="DataGroup"/>
        /// </summary>
        public static readonly DependencyProperty DataGroupProperty = DependencyProperty.Register
            (
                nameof(DataGroup),
                typeof(ObservableCollection<DataSeries>),
                typeof(Chart),
                new PropertyMetadata(default(ObservableCollection<DataSeries>), OnDataGroupChanged)
            );

        private static void OnDataGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as Chart;
            if (self == null) return;

            OnDataGroupReset(
                self,
                e.OldValue as ObservableCollection<DataSeries>,
                e.NewValue as ObservableCollection<DataSeries>);
        }

        private static void OnDataGroupReset(Chart self, ObservableCollection<DataSeries> oldCollection, ObservableCollection<DataSeries> newCollection)
        {
            if (self.PART_FigureContainer == null || self.PART_FigureContainer.Children == null) return;
            self.PART_FigureContainer.Children.Clear();

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged += self.DataGroup_CollectionChanged;
            }
            if (newCollection == null) return;
            foreach (var item in newCollection)
            {
                var id = self.PART_FigureContainer.Children.Count;
                var stroke = Brushes.DarkGreen;
                if (id < Chart.DefaultBrushes.Count)
                {
                    stroke = Chart.DefaultBrushes[id];
                }
                var figure = new PolylineFigure() { DataSeries = item, Stroke = stroke };
                figure.Container = self;
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
                    if (item is Figure)
                        PART_FigureContainer?.Children.Add(item as Figure);
                }
            }
        }


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
            => (d as IChart)?.ResetValueToScreenMethod();

        void Child_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = e.NewItems;
            if (collection != null)
            {
                foreach (ChartElement item in collection)
                {
                    if (item != null) item.Container = this;
                }
            }
            // TODO: FIX
            //collection = e.OldItems;
            //if (collection != null)
            //{
            //    foreach (var item in collection)
            //    {
            //        (item as IChartElement)?.RemoveContainer();
            //    }
            //}
        }

        #endregion

    }
}
