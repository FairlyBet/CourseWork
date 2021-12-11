using System;

namespace CourseWork
{
    public class TactGenerator
    {
        public event Action OnTick;

        public int TickRate { get; set; }



        public TactGenerator(in int tickRate)
        {
            TickRate = tickRate;
        }

        public void Tick()
        {
            OnTick?.Invoke();
        }

    }
}
