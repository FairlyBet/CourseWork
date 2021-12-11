using System;
using System.Threading;

namespace CourseWork
{
    public class TactGenerator
    {
        private readonly AutoResetEvent _waitHandler;
        private bool _pause;
        public event Action OnTick;

        public bool Exit { get; set; }

        public int TickRate { get; set; }

        public bool Pause
        {
            get => _pause;

            set
            {
                _pause = value;
                if (!_pause)
                {
                    _waitHandler.Set();
                }
            }
        }


        public TactGenerator(in int tickRate)
        {
            _waitHandler = new(true);
            TickRate = tickRate;
        }

        public void Tick()
        {
            OnTick?.Invoke();
        }

        public void Run()
        {
            while (!Exit)
            {
                if (_pause)
                {
                    _waitHandler.Reset();
                }
                _waitHandler.WaitOne();
                OnTick?.Invoke();
                _waitHandler.Set();
                Thread.Sleep(TickRate);
            }
        }
    }
}
