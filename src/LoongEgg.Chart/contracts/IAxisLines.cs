using System.Windows.Media;

namespace LoongEgg.Chart
{
    interface IAxisLines
    {
        Brush Stroke { get; set; }
        double StrokeThickness { get; set; }
        double Length { get; set; }
        void OnLengthSet();
    }
}
