namespace LoongEgg.Chart
{
    /// <summary>
    /// 以时间为横向刻度的表
    /// </summary>
    interface ITimerChart
    {
        /// <summary>
        /// 时间表内部有一个<see cref="Chart"/>, 以屏蔽过多的属性设置, 减少设置逻辑
        /// </summary>
        Chart PART_Chart { get; }
        /// <summary>
        /// 每个周期记录的时间长度（单位为s）
        /// </summary>
        Intervals Interval { get; set; } 
        void AddNewValue(double val);
        /// <summary>
        /// 重设横向坐标
        /// </summary>
        void ResetHorizontalTicks();
        /// <summary>
        /// 重设图表数据
        /// </summary>
        void ResetDataSeries();
    }
}
