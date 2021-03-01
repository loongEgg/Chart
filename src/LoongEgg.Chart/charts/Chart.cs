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

namespace LoongEgg.Chart
{
    [TemplatePart(Name = nameof(PART_Left), Type = typeof(Panel))]
    [TemplatePart(Name = nameof(PART_Right), Type = typeof(Panel))]
    [TemplatePart(Name = nameof(PART_Top), Type = typeof(Panel))]
    [TemplatePart(Name = nameof(PART_Bottom), Type = typeof(Panel))]
    [TemplatePart(Name = nameof(PART_Center), Type = typeof(Panel))]
    [ContentProperty(nameof(Children))]

    public class Chart : Control, IChart
    {
        public Panel PART_Left { get; private set; }
        public Panel PART_Right { get; private set; }
        public Panel PART_Top { get; private set; }
        public Panel PART_Bottom { get; private set; }
        public Panel PART_Center { get; private set; }

        public bool InternalChange { get; set; }
         
        public ValueToScreen HorizontalValueToScreen { get; private set; }
        public ValueToScreen VerticalValueToScreen { get; private set; }

        static Chart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
        }

        public Chart()
        {
            Children = new ObservableCollection<object>();
            SetCurrentValue(HorizontalRangeProperty, new Range(0, 200));
            SetCurrentValue(VerticalRangeProperty, new Range(-30, 30));
            SetCurrentValue(HorizontalMajorTicksProperty, new double[] { 25, 50, 75, 100, 125, 150, 175 });
            SetCurrentValue(VerticalMajorTicksProperty, new double[] { -20, -10, 0, 10, 20 });

            Loaded += (s, e) =>
            {
                if(Children != null)
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
            };
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Left = GetTemplateChild(nameof(PART_Left)) as Panel;
            PART_Right = GetTemplateChild(nameof(PART_Right)) as Panel;
            PART_Top = GetTemplateChild(nameof(PART_Top)) as Panel;
            PART_Bottom = GetTemplateChild(nameof(PART_Bottom)) as Panel;
            PART_Center = GetTemplateChild(nameof(PART_Center)) as Panel;

            if (PART_Center != null)
                PART_Center.SizeChanged += (s, e) => ResetValueToScreenMethod();

            if (PART_Left == null
                || PART_Right == null
                || PART_Top == null
                || PART_Bottom == null
                || PART_Center == null)
                Debugger.Break();
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

                        var collection = e.NewValue as ObservableCollection<object>;
                        if (collection != null || self != null)
                        {
                            foreach (ChartElement item in collection)
                            {
                                if(item != null) item.Container = self;
                                Logger.Dbug($"{item.GetType()} add", true);
                            }
                            collection.CollectionChanged += self.Collection_CollectionChanged;
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


        void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = e.NewItems;
            if (collection != null)
            {
                foreach (ChartElement item in collection)
                {
                    if(item != null ) item.Container = this; 
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
    }
}
