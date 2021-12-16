#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8618
#pragma warning disable CS8625
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
