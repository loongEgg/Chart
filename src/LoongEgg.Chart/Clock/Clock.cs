using System;
using System.Diagnostics;
using System.Windows.Threading;
using LoongEgg.Log;

namespace LoongEgg.Chart
{

    public sealed class Clock
    {
        static int ClockCount = 0;
        public int ClockId { get; }

        public static Clock Singleton { get; } = new Clock();

        public int LastHour { get; private set; }
        public int LastMinute { get; private set; }
        public int LastSecond { get; private set; }
        public int LastMilliSecond { get; private set; }
        public double LastElapsedSecond { get; private set; }
        public double TimeStamp => LastSecond + LastMilliSecond / 1000.0;
        public DispatcherTimer Timer { get; } = new DispatcherTimer();
        /// <summary>
        /// 每个时钟周期, 约15.6ms间隔, 在VS中会很慢约25Hz, 但是独立运行可以到64Hz
        /// </summary>
        public event EventHandler Tick;
        public int FPS { get; private set; }

        Clock()
        {
            ClockCount += 1;
            ClockId = ClockCount;

            Timer.Interval = TimeSpan.FromMilliseconds(1);
            DateTime now;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            double lastTime = 0;
            Timer.Tick += (s, e) =>
            {
                now = DateTime.Now;
                LastHour = now.Hour;
                LastMinute = now.Minute;
                LastMilliSecond = now.Millisecond;
                if (watch.ElapsedMilliseconds - lastTime > 2)
                {
                    lastTime = watch.ElapsedMilliseconds;
                    FPS += 1;
                    Tick?.Invoke(this, EventArgs.Empty);
                }
                if (lastTime >= 1000)
                {
                    lastTime = 0;
                    Logger.Dbug($"FPF: {FPS}");
                    FPS = 0;
                    watch.Restart();
                }
                LastSecond = now.Second;

            };
            Timer.Start();
        }

    }
}
