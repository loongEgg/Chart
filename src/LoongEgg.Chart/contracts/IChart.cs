using LoongEgg.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace LoongEgg.Chart
{
    public interface IChart
    {
        /// <summary>
        /// 内部改动标志, 抑制重复刷新
        /// </summary>
        bool InternalChange { get; set; }
        /// <summary>
        /// 左侧元素的容器
        /// </summary>
        Panel PART_Left { get; }
        /// <summary>
        /// 右侧元素的容器
        /// </summary>
        Panel PART_Right { get; }
        /// <summary>
        /// 顶部元素的容器
        /// </summary>
        Panel PART_Top { get; }
        /// <summary>
        /// 底部元素的容器
        /// </summary>
        Panel PART_Bottom { get; }
        /// <summary>
        /// 中间元素的容器
        /// </summary>
        Panel PART_Center { get; }
        /// <summary>
        /// 横向主刻度
        /// </summary>
        IEnumerable<double> HorizontalMajorTicks { get; set; }
        /// <summary>
        /// 横向次要刻度
        /// </summary>
        IEnumerable<double> HorizontalMinorTicks { get; set; }
        /// <summary>
        /// 垂直方向主刻度
        /// </summary>
        IEnumerable<double> VerticalMajorTicks { get; set; }
        /// <summary>
        /// 垂直方向次要刻度
        /// </summary>
        IEnumerable<double> VerticalMinorTicks { get; set; }
        /// <summary>
        /// 水平数据范围
        /// </summary>
        Range HorizontalRange { get; set; }
        /// <summary>
        /// 垂直数据范围
        /// </summary>
        Range VerticalRange { get; set; }
        /// <summary>
        /// 水平投影算法
        /// </summary>
        ValueToScreen HorizontalValueToScreen { get; }
        /// <summary>
        /// 垂直投影算法
        /// </summary>
        ValueToScreen VerticalValueToScreen { get; }
        /// <summary>
        /// 所有子元素(类型应该为IChartElement)集合, 子元素在被添加时应把<see cref="IChartElement.Container"/>设置为this
        /// </summary>
        ObservableCollection<object> Children { get; }
        /// <summary>
        /// 重置<see cref="HorizontalValueToScreen"/>和<see cref="VerticalValueToScreen"/>
        /// </summary>
        void ResetValueToScreenMethod();

    }

   
}
