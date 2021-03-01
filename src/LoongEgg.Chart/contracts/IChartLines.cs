using System.Windows.Media;

namespace LoongEgg.Chart
{
    interface IChartLines
    {
        Brush Stroke { get; set; }
        double StrokeThickness { get; set; }
    }
}
