using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoongEgg.Data;

namespace LoongEgg.Chart
{
    public static class AutomaticTick
    {
        #region default resources 

        readonly static double[] Standard_Steps = { 0.1, 0.125, 0.15, 0.2, 0.25, 0.4, 0.5, 1 };

        #endregion

        #region protected methods

        private static void AutomaticSpacing(Range range, double count, out double step)
        {
            AutomaticSpacing(range.Min, range.Max, count, out step);
        }

        public static void AutomaticSpacing(double min, double max, double count, out double step)
        {
            step = (max - min) / count;
            double power = Math.Ceiling(Math.Log(step) / Math.Log(10)); /* 计算当前space相对10的幂(即数量级) */
            double multiplier = Math.Pow(10, power);
            var steps = Standard_Steps.Select(s => s * multiplier);
            foreach (var s in steps)
            {
                if (s >= step)
                {
                    step = s;
                    break;
                }
            }
        }

        private static void EndPointFix(double minOld, double maxOld, double step, out double minNew, out double maxNew)
        {
            minNew = Math.Floor(minOld / step) * step;
            maxNew = Math.Ceiling(maxOld / step) * step;
        }

        private static void TicksSpacing(Range range, double step, out double[] ticks)
        {
            var result = new List<double>();
            double tmp = range.Min;
            result.Add(tmp);
            do
            {
                tmp += step;
                result.Add(tmp);
            } while (tmp < range.Max);

            ticks = result.ToArray();
        }

        private static void RangeFix(Range oldRange, double step, out Range newRange)
        {
            double min, max;
            EndPointFix(oldRange.Min, oldRange.Max, step, out min, out max);
            newRange = new Range(min, max);
        }

        #endregion

        #region public methods

        /// <summary>
        /// 计算规范化的数据范围和刻度集合
        /// </summary>
        /// <param name="oldRange">老的范围</param>
        /// <param name="count">划分的分数</param>
        /// <param name="newRange">计算出来新的范围</param>
        /// <param name="ticks">计算出来的刻度集合</param>
        public static void RangeTicksFix(Range oldRange, double count, out Range newRange, out double[] ticks)
        {
            double step;
            AutomaticSpacing(oldRange, count, out step);
            RangeFix(oldRange, step, out newRange);
            TicksSpacing(newRange, step, out ticks);
        }

        /// <summary>
        /// 重新划分刻度, 用于从MajorTicks转换到MinorTicks
        /// </summary>
        /// <param name="ticksOld">老的刻度集合</param>
        /// <param name="newCount">划分的新的分数</param>
        /// <param name="ticksNew">计算出的新的刻度集合</param>
        public static void TicksRespacing(double[] ticksOld, double newCount, out double[] ticksNew)
        {
            var min = ticksOld.Min();
            var max = ticksOld.Max();
            double step = (max - min) / newCount;
            ticksNew = new double[(int)newCount];
            for (int i = 0; i < newCount; i++)
            {
                ticksNew[i] = min + step * i;
            }
        }

        #endregion

    }
}
