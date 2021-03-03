using System.ComponentModel;

namespace LoongEgg.Chart
{
    public interface ISignal
    {
        /// <summary>
        /// 当前的数值
        /// </summary>
        double Value { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [DefaultValue("LABEL")]
        string Label { get; set; } 
        /// <summary>
        /// 单位
        /// </summary>
        [DefaultValue("-")]
        string Unit { get; set; }
    }
}
