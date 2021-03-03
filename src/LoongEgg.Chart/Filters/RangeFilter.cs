using System;
using System.Collections.Generic;
using System.Linq;
using LoongEgg.Data;
using Point = LoongEgg.Data.Point;

namespace LoongEgg.Chart
{
    public class RangeFilter : Filter
    {
        public Range Xrange { get; set; } = new Range(50, 60);
        public Range Yrange { get; set; } = new Range(-100, 100);

        public RangeFilter(Range xrange, Range yrange)
        {
            if (xrange == null) throw new ArgumentNullException($"{nameof(xrange)}");
            if (yrange == null) throw new ArgumentNullException($"{nameof(yrange)}");
            Xrange = xrange;
            Yrange = yrange;
        }

        public override List<Point> Filtering(List<Point> points)
        {
            if (points == null || points.Count == 0)
            {
                if (points.Count >= 1)
                {
                    Result.Clear();
                    RaiseChanged();
                }else
                {
                    Result.Clear();
                }

                return Result;
            }
            else
            {
                Result = points.Where(p => Xrange.Min <= p.X && p.X <= Xrange.Max && Yrange.Min <= p.Y && p.Y <= Yrange.Max).ToList();
                return Result;
            }
        }
    }
}
