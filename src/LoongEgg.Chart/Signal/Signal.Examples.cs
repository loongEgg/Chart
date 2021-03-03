using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace LoongEgg.Chart
{
    public partial class Signal
    {

        private static Clock Clock = Clock.Singleton;

        private static DispatcherTimer Timer = new DispatcherTimer();

        #region standard signal

        static Signal()
        {
            Timer.Interval = TimeSpan.FromMilliseconds(15);
            Stopwatch watch = new Stopwatch();
            Timer.Start();
            watch.Start();
        }

        private readonly static double Deg2Rad = Math.PI / 180.0;

        public static Signal SinSignal => _SinSignal ?? (_SinSignal = CreatSinSignal());
        private static Signal _SinSignal;

        private static Signal CreatSinSignal()
        {
            var signal = new Signal() { Label = "Sin(t)" };
            Timer.Tick += (s, e) =>
            {
                signal.Value = Math.Sin((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Deg2Rad * 6) * 30;
            };
            return signal;
        }

        public static Signal CosSignal => _CosSignal ?? (_CosSignal = CreatCosSignal());
        private static Signal _CosSignal;

        private static Signal CreatCosSignal()
        {
            var signal = new Signal() { Label = "Cos(t)" };
            Timer.Tick += (s, e) =>
            {
                signal.Value = Math.Cos((DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0) * Deg2Rad * 6) * 30;
            };
            return signal;
        }

        public static Signal SquareSignal => _SquareSignal ?? (_SquareSignal = CreatSquareSignal());
        private static Signal _SquareSignal;

        private static Signal CreatSquareSignal()
        {
            int T = 10 * 1000;
            double Thalf = T / 2.0;
            var signal = new Signal() { Label = "Square(t)" };
            Timer.Tick += (s, e) =>
            {
                double t = (int)((DateTime.Now.Second * 1000 + DateTime.Now.Millisecond)) % T;
                if (t <= Thalf)
                    signal.Value = 25;
                else
                    signal.Value = -25;
            };
            return signal;
        }

        public static Signal TriangleSignal => _TriangleSignal ?? (_TriangleSignal = CreatTriangleSignalSignal());

        private static Signal CreatTriangleSignalSignal()
        {
            int T = 20 * 1000;
            double T1 = 5 * 1000;
            double T2 = 10 * 1000;
            double T3 = 15 * 1000;
            var signal = new Signal() { Label = "Triangle(t)" };
            Clock.Tick += (s, e) =>
            {

                double t = (DateTime.Now.Second * 1000 + DateTime.Now.Millisecond) % T;
                double time = t / 1000.0;
                if (t < T1)
                    signal.Value = time * 2.0;
                else if (t <= T2)
                    signal.Value = -(time - 5) * 2.0 + 10;
                else if (t <= T3)
                    signal.Value = (time - 10) * 2.0 - 10;
                else
                    signal.Value = -(time - 15) * 2.0 + 10;
            };
            return signal;
        }

        private static Signal _TriangleSignal;

        #endregion

    }
}
