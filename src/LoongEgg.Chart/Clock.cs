using System;
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
        public DispatcherTimer Timer { get; } = new DispatcherTimer();

        Clock()
        {
            ClockCount += 1;
            ClockId = ClockCount;

            Timer.Interval = TimeSpan.FromMilliseconds(10);
            DateTime now;
            int fps = 0;
            Timer.Tick += (s, e) =>
            {
                now = DateTime.Now;
                LastHour = now.Hour;
                LastMinute = now.Minute;

#if DEBUG
                if (now.Second != LastSecond)
                {
                    Logger.Dbug($"Clock:{ClockId}, FPS: {fps}, LastElapsedSecond: {LastElapsedSecond}");
                    LastSecond = now.Second;
                    fps = 0;
                }
                fps += 1;
#endif
                if (now.Millisecond > LastMilliSecond)
                    LastElapsedSecond = (now.Millisecond - LastMilliSecond) / 1000.0;
                else
                    LastElapsedSecond = (1000.0 - LastMilliSecond) / 1000.0;
                LastMilliSecond = now.Millisecond;

                LastSecond = now.Second;
            };
            Timer.Start();
        }
    }
}
