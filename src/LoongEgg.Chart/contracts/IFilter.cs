using System;
using System.Collections.Generic;
using Point = LoongEgg.Data.Point;

namespace LoongEgg.Chart
{
    /// <summary>
    /// 滤波器的基类
    /// </summary>
    public interface IFilter
    { 
        /// <summary>
        /// 滤波结果
        /// </summary>
        List<Point> Result { get;  }

        /// <summary>
        /// 滤波的具体实现方法
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        List<Point> Filtering(List<Point> points);
        /// <summary>
        /// raised when <see cref="Result"/> changed 
        /// </summary>
        event EventHandler ResultChanged;
    }
}
