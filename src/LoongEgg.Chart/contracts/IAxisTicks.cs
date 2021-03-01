using System.Collections.Generic;

namespace LoongEgg.Chart
{
    interface IAxisTicks
    {
        /// <summary>
        /// 刻度集合
        /// </summary>
        IEnumerable<double> Ticks { get; set; }
        /// <summary>
        /// <see cref="TickLevels"/>
        /// </summary>
        TickLevels TickLevel { get; set; }
        /// <summary>
        /// 重设刻度源
        /// </summary>
        void ResetTickSource();
    }
}
