using LoongEgg.Data;

namespace LoongEgg.Chart
{
    /// <summary>
    /// 以时间为横向刻度的表
    /// </summary>
    interface ITimerChart
    {
        bool InternalChanging { get; set; }
        /// <summary>
        /// 时间表内部有一个<see cref="Chart"/>, 以屏蔽过多的属性设置, 减少设置逻辑
        /// </summary>
        Chart PART_Chart { get; }       
        SignalGroup SignalGroup { get; set; }
    }
}
