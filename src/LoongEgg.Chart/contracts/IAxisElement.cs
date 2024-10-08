﻿namespace LoongEgg.Chart
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
        /// 坐标元素的布局位置
        /// </summary>
        Placements Placement { get; }
        /// <summary>
        /// 投影算法
        /// </summary>
        ValueToScreen ValueToScreen { get; }
         
    }
}
