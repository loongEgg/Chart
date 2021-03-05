using System;
using System.Diagnostics;
using System.Windows.Threading;
using LoongEgg.Log;

namespace LoongEgg.Chart
{
    // TODO:简化时钟
    public sealed class Clock
    {
        public static Clock Singleton => _Singleton ?? (_Singleton = new Clock());
        private static Clock _Singleton;

        public int LastHour { get; private set; }
        public int LastSecond { get; private set; }
        public int LastMinute { get; private set; }
        public int LastMilliSecond { get; private set; }
        /// <summary>
        /// 当前小时: 分钟
        /// </summary>
        public string TimeNow => $"{LastHour}:{LastMinute}";
        /// <summary>
        /// 当前秒+毫秒
        /// </summary>
        public double TimeStamp => LastSecond + LastMilliSecond / 1000.0;
        public DispatcherTimer Timer { get; } = new DispatcherTimer();
        /// <summary>
        /// 每个时钟周期, 约15.6ms间隔, 在VS中会很慢约25Hz, 但是独立运行可以到64Hz
        /// </summary>
        public event EventHandler Tick;
        /// <summary>
        /// 分钟事件
        /// </summary>
        public event EventHandler MinuteTick;
        public int FPS { get; private set; }

        Clock()
        {
            Timer.Interval = TimeSpan.FromMilliseconds(10);
            DateTime now;
            Timer.Tick += (s, e) =>
            {
                now = DateTime.Now;
                LastHour = now.Hour;

                if (LastMinute != now.Minute)
                {
                    LastMinute = now.Minute;
                    MinuteTick?.Invoke(this, EventArgs.Empty);
                }

                LastMinute = now.Minute;

                LastMilliSecond = now.Millisecond;

                FPS += 1;

                if (LastSecond != now.Second)
                {
                    Logger.Dbug($"Clock FPF: {FPS}");
                    FPS = 0;
                }
                LastSecond = now.Second;
                Tick?.Invoke(this, EventArgs.Empty);
            };
            Timer.Start();
        }

    }
}
