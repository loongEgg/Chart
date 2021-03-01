using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LoongEgg.Chart
{
    public class HorizontalTicks : HorizontalLineElements
    {

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public HorizontalElementPlacement Placement
        {
            get { return (HorizontalElementPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        /// <summary>
        /// dependency property of <see ref="Placement"/>
        /// </summary>
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register(
                nameof(Placement),
                typeof(HorizontalElementPlacement),
                typeof(HorizontalTicks),
                new PropertyMetadata(default(HorizontalElementPlacement), (s, e) => (s as HorizontalTicks)?.ResetMargin()));


        public override void ResetMargin()
        {
            if(Placement == HorizontalElementPlacement.Top)
            {

            }
        }

        public override void ResetPlacement()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
    public class HorizontalGridLines : HorizontalLineElements
    {
        public override void ResetMargin() { }

        public override void ResetPlacement()
        {
            if (Container == null || Container.PART_Center == null) return;
            (Parent as Panel)?.Children.Remove(this);
            Container.PART_Center.Children.Add(this);
        }

        public override void Update()
        {
            if (Root == null || Ticks == null || Container == null || Container.PART_Center == null) return;
            double height = Container.PART_Center.ActualHeight;
            GeometryGroup group = new GeometryGroup();
            if (ValueToScreen == null)
            {
                foreach (var x in Ticks)
                {
                    LineGeometry line = new LineGeometry(new Point(x, 0), new Point(x, height));
                    group.Children.Add(line);
                }
            }
            else  // if (ValueToScreen != null)
            {
                double x;
                foreach (var t in Ticks)
                {
                    x = ValueToScreen(t);
                    LineGeometry line = new LineGeometry(new Point(x, 0), new Point(x, height));
                    group.Children.Add(line);
                }
            }
            Lines.Data = group;
            Root.Children.Add(Lines);
        }
    }
}
