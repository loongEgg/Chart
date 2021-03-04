using System;
using System.Collections.Generic;
using LoongEgg.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoongEgg.Chart.Test
{
    [TestClass]
    public class AutomaticTick_Test
    {
        [TestMethod]
        public void AutomaticSpacing_Check()
        {
            var data = new List<object[]>
            {
                /*                        oldMin oldMax  count  step              newMin       newMax              ticks                     */
                //new object[] { new Range( 10,       20),  10,     1,   new Range (  10,         20    ), new double[] {10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20} },
                new object[] { new Range( 10,       20),   5,     2,   new Range (  10,         20    ), new double[] {10, 12, 14, 16, 18, 20} },
                new object[] { new Range( -0.23, 10.11),   4,     4,   new Range (  -4,         12    ), new double[] {-4,  0,  4,  8, 12} },
                new object[] { new Range(-10,    90.34),  10,    12.5, new Range ( -12.5,      100    ), new double[] {-12.5,  0,  12.5,  25, 37.5, 50, 62.5, 75, 87.5, 100} },
                new object[] { new Range(-0.034, -0.023), 10, 0.00125, new Range (  -0.035,    -0.0225), new double[] { -0.035, -0.03375, -0.0325, -0.03125, -0.03, -0.02875, -0.0275, -0.02625, -0.025, -0.0225} },
                new object[] { new Range( 3.21,    2021),  5,     500, new Range (   0,      2500     ), new double[] { 0, 500, 1000, 1500, 2000, 2500} },
            };
            Range oldRange;
            double count;
            Func<object[], double> stepExpected = (a) => double.Parse(a[2].ToString());

            double stepActual;
            foreach (var a in data)
            {
                oldRange = a[0] as Range;
                count = double.Parse(a[1].ToString());
                AutomaticTick.AutomaticSpacing(oldRange.Min, oldRange.Max, count, out stepActual);
                Assert.AreEqual(stepExpected(a), stepActual);
            }
        }

        [TestMethod]
        public void RangeTicksFix_Check()
        {
            var data = new List<object[]>
            {
                /*                        oldMin oldMax  count  step              newMin       newMax              ticks                     */
                //new object[] { new Range( 10,       20),  10,     1,   new Range (  10,         20    ), new double[] {10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20} },
                new object[] { new Range( 10,       20),   5,     2,   new Range (  10,         20    ), new double[] {10, 12, 14, 16, 18, 20} },
                new object[] { new Range( -0.23, 10.11),   4,     4,   new Range (  -4,         12    ), new double[] {-4,  0,  4,  8, 12} },
                new object[] { new Range(-10,    90.34),  10,    12.5, new Range ( -12.5,      100    ), new double[] {-12.5,  0,  12.5,  25, 37.5, 50, 62.5, 75, 87.5, 100} },
                new object[] { new Range(-0.034, -0.023), 10, 0.00125, new Range (  -0.035,    -0.0225), new double[] { -0.035, -0.03375, -0.0325, -0.03125, -0.03, -0.02875, -0.0275, -0.02625, -0.025, -0.02375, -0.0225} },
                new object[] { new Range( 3.21,    2021),  5,     500, new Range (   0,      2500     ), new double[] { 0, 500, 1000, 1500, 2000, 2500} },
            };
            Range oldRange;
            Range newRangeExpected;
            double count;
            Func<object[], double> stepExpected = (a) => (double)a[2];
            double[] ticksExpected;

            //double stepActual;
            Range newRangeActual;
            double[] ticksActual;
            foreach (var a in data)
            {
                oldRange = a[0] as Range;
                count = double.Parse(a[1].ToString());
                newRangeExpected = a[3] as Range;
                ticksExpected = a[4] as double[];

                AutomaticTick.RangeTicksFix(oldRange, count, out newRangeActual, out ticksActual);

                Assert.AreEqual(newRangeExpected.Min, newRangeActual.Min);
                Assert.AreEqual(newRangeExpected.Max, newRangeActual.Max);

                Assert.AreEqual(ticksExpected.Length, ticksActual.Length);
            }
        }
    }
}
