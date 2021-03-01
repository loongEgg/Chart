using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LoongEgg.Chart
{
    public abstract class AxisElement : ChartElement, IAxisElement
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public double Gap
        {
            get { return (double)GetValue(GapProperty); }
            set { SetValue(GapProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="Gap"/>
        /// </summary>
        public static readonly DependencyProperty GapProperty =
            DependencyProperty.Register(
                nameof(Gap),
                typeof(double),
                typeof(AxisElement),
                new PropertyMetadata(default(double), (s, e) => (s as AxisElement)?.ResetMargin()));
               
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Placements Placement
        {
            get { return (Placements)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="Placement"/>
        /// </summary>
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register(
                nameof(Placement),
                typeof(Placements),
                typeof(AxisElement),
                new PropertyMetadata(
                    default(Placements), 
                    (s,e) =>
                    {
                        var self = s as AxisElement;
                        if (self == null) return;
                        self.ResetPlacement();
                        self.ResetMargin();
                        self.Update();
                    }));

        public ValueToScreen ValueToScreen
        {
            get
            {
                if (Container == null) return null;
                if (Placement  == Placements.Top || Placement == Placements.Bottom)
                    return Container.HorizontalValueToScreen;
                else
                    return Container.VerticalValueToScreen;
            }
        }

        public override void ResetPlacement()
        {
            if (Container == null) return;
            (Parent as Panel)?.Children.Remove(this);
            if (Placement == Placements.Left && Parent != Container.PART_Left)
            {
                Container.PART_Left?.Children.Add(this);
            }
            else if (Placement == Placements.Top && Parent != Container.PART_Top)
            {
                Container.PART_Top?.Children.Add(this);
            }
            else if (Placement == Placements.Right && Parent != Container.PART_Right)
            {
                Container.PART_Right?.Children.Add(this);
            }
            else if (Placement == Placements.Bottom && Parent != Container.PART_Bottom)
            {
                Container.PART_Bottom?.Children.Add(this);
            }
        }

        public void ResetMargin()
        {
            if (Placement == Placements.Left)
            {
                HorizontalAlignment = HorizontalAlignment.Right;
                Margin = new Thickness(0, 0, Gap, 0);
            }
            else if (Placement == Placements.Top)
            {
                VerticalAlignment = VerticalAlignment.Bottom;
                Margin = new Thickness(0, 0, 0, Gap);
            }
            else if (Placement == Placements.Right)
            {
                HorizontalAlignment = HorizontalAlignment.Left;
                Margin = new Thickness(Gap, 0, 0, 0);
            }
            else if (Placement == Placements.Bottom)
            {
                VerticalAlignment = VerticalAlignment.Top;
                Margin = new Thickness( 0, Gap, 0, 0);
            }
        }
    }
}
