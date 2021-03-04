using System;
using System.Collections.Generic;
using Point = LoongEgg.Data.Point;

namespace LoongEgg.Chart
{
    // TODO: remove to LoongEgg.Filter
    public abstract class Filter : IFilter
    {
        public List<Point> Result { get; protected set; } = new List<Point>();

        public event EventHandler ResultChanged;

        public abstract List<Point> Filtering(List<Point> points);

        protected void RaiseChanged() => ResultChanged?.Invoke(this, EventArgs.Empty);

    }
}
