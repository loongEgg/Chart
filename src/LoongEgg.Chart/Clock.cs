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
        /// 每个时钟周期, 约32ms
        /// </summary>
        public event EventHandler Tick;
        private double Count;
        public int FPS { get; private set; }

        Clock()
        {
            ClockCount += 1;
            ClockId = ClockCount;

            Timer.Interval = TimeSpan.FromMilliseconds(4);
            DateTime now;
            
            Timer.Tick += (s, e) =>
            {
                now = DateTime.Now;
                LastHour = now.Hour;
                LastMinute = now.Minute;
                LastMilliSecond = now.Millisecond; 
                if (LastMilliSecond - Count > 28)
                {
                    Count = LastMilliSecond;
                    FPS += 1;
                    Tick?.Invoke(this, EventArgs.Empty);
                }
                if (LastSecond != now.Second)
                {
                    Debug.WriteLine($"FPF: {FPS}");
                    FPS = 0;
                    Count = 0; 
                }
                LastSecond = now.Second;

            };
            Timer.Start();
        }
       
    }
}
