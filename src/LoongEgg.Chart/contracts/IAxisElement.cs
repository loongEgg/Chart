using System.Windows.Controls;

namespace LoongEgg.Chart
{
    /// <summary>
    /// 坐标轴元素接口
    /// </summary>
    interface IAxisElement
    {
        /// <summary>
        /// 位置偏移量, 用来简化Margin设置
        /// </summary>
        double Gap { get; set; }
        /// <summary>
        /// 坐标轴元素的方向
        /// </summary>
        Orientation Orientation { get; }
        /// <summary>
        /// 投影算法
        /// </summary>
        ValueToScreen ValueToScreen { get; }

        /// <summary>
        /// 根据<see cref="Gap"/>设置Margin
        /// </summary>
        void ResetMargin();
    }
}
