using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LoongEgg.Log;

namespace LoongEgg.Chart
{
    public class PolylineFigure : Figure
    {
        public override void Update()
        {
            if (Root == null) return;
            Root.Children.Clear();
            if (ActualHeight == 0 || ActualWidth == 0) return;
            if (NormalizedPoints == null || NormalizedPoints.Count <= 1) return;

            //Logger.Dbug($"PolylineFigure[{GetHashCode()}] update x {(++UpdateCount)}");

            // 创建一个新的图形
            WriteableBitmap bitmap = new WriteableBitmap
            (
                (int)this.ActualWidth,
                (int)this.ActualHeight,
                96, 96,
                PixelFormats.Bgra32,
                null
            );
            var figure = new System.Windows.Controls.Image { Source = bitmap };
            bitmap.Lock(); /* 别忘了锁定 */

            // 绘制图形
            using (System.Drawing.Bitmap buff = new System.Drawing.Bitmap(
                    (int)bitmap.Width,
                    (int)bitmap.Height,
                    bitmap.BackBufferStride,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                    bitmap.BackBuffer))
            {
                // 使用GDI+绘制
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(buff))
                {
                    var color = (Stroke as SolidColorBrush).Color;
                    var brush = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                    var pen = new System.Drawing.Pen(brush, StrokeThickness); /* 颜色和线条宽度 */
                    graphics.DrawLines(pen, NormalizedPoints.ToArray());
                    graphics.Flush();
                }
            }

            bitmap.AddDirtyRect(new Int32Rect(0, 0, (int)bitmap.Width, (int)bitmap.Height));
            bitmap.Unlock();
            Root.Children.Add(figure);
        }
    }
}
