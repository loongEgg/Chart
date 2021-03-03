using System.Collections.Generic;
using System.Windows.Media;
using LoongEgg.Data;

namespace LoongEgg.Chart
{
    
    interface IFigure
    {
        ValueToScreen HorizontalValueToScreen { get; }
        ValueToScreen VerticalValueToScreen { get; }
        DataSeries DataSeries { get; set; }
        List<System.Drawing.PointF> NormalizedPoints { get; }
        System.Drawing.PointF Normalize(double x, double y);

        Brush Stroke { get; set; }
        float StrokeThickness { get; set; }
        void ResetNormalizeMethods();
        void ResetNormalizePointsFromDataSeries();
    }
}
