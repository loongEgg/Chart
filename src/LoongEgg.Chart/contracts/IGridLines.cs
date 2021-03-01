using System.Collections.Generic;

namespace LoongEgg.Chart
{
    interface IGridLines
    {
        /// <summary>
        /// 水平主刻度线集合
        /// </summary>
        IEnumerable<double> HorizontalMajorTicks { get; set; }

        /// <summary>
        /// 水平次刻度线集合
        /// </summary>
        IEnumerable<double> HorizontalMinorTicks { get; set; }

        /// <summary>
        /// 垂直主刻度线集合
        /// </summary>
        IEnumerable<double> VerticalMajorTicks { get; set; }

        /// <summary>
        /// 垂直次刻度线集合
        /// </summary>
        IEnumerable<double> VerticalMinorTicks { get; set; }

        /// <summary>
        /// 水平主刻度可见?
        /// </summary>
        bool IsHorizontalMajorTickVisible { get; set; }
        /// <summary>
        /// 水平次刻度可见?
        /// </summary>
        bool IsHorizontalMinorTickVisible { get; set; }
        /// <summary>
        /// 垂直主刻度可见?
        /// </summary>
        bool IsVerticalMajorTickVisible { get; set; }
        /// <summary>
        /// 垂直次刻度可见?
        /// </summary>
        bool IsVerticalMinorTickVisible { get; set; }

        ValueToScreen HorizontalValueToScreen { get; }
        ValueToScreen VerticalValueToScreen { get; }
    }
}
