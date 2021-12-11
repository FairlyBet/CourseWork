using System;

namespace CourseWork
{
    public class Process
    {
        private static int s_seed = 0;

        public uint Id { get; }

        public string Name { get; }

        public byte Priority { get; set; }

        public ProcessState State { get; private set; }

        public Performance Performance { get; }

        public int ExecutingTime { get; private set; }

        public uint BurstTime { get; private set; }

        public uint Size { get; }

        public MemoryBlock Location { get; set; }


        public Process(in uint id)
        {
            Id = id;
            Name = ProccesNameGenerator.GenerateName();
            State = ProcessState.Ready;
            BurstTime = 0;

            Random random = new(s_seed++);

            Priority = (byte)random.Next(Config.MaxPriority + 1);
            Size = random.NextUint(Config.MinProccesSize, Config.MaxProccesSize + 1);
            Performance = (Performance)(random.Next((int)Performance.High + 1));
            ExecutingTime = random.Next((int)Performance) + 1;

            Location = null;
        }

        public Process(uint id, string name, byte priority, ProcessState state,
            Performance performance, uint size)
        {
            Id = id;
            Name = name;
            Priority = (byte)(priority % Config.MaxPriority);
            State = state;
            Performance = performance;
            Size = size % Config.MemorySize + (uint)(size == 0 ? 1 : 0);
            Random random = new(s_seed++);
            ExecutingTime = random.Next((int)Performance) + 1;
            Location = null;
        }

        public void Execute()
        {
            if (BurstTime < ExecutingTime)
            {
                BurstTime++;
                if (BurstTime == ExecutingTime)
                {
                    State = ProcessState.Waiting;
                }
            }
        }

        public void StartExecuting()
        {
            State = ProcessState.Running;
        }

        public void FinishProcess()
        {
            State = ProcessState.Finished;
        }

        public void UpdateState()
        {
            if (State == ProcessState.Waiting)
            {
                Random random = new(s_seed++);
                int chance = 5 * (int)Performance;
                if (random.Next(100) + 1 <= chance)
                {
                    ExecutingTime = random.Next((int)Performance) + 1;
                    BurstTime = 0;
                    State = ProcessState.Ready;
                }
            }
        }

        public override string ToString()
        {
            return $"[{Name}, {Id}, {Size / 1_000_000}MB, {State}, {BurstTime}/{ExecutingTime}]";
        }


        private static class ProccesNameGenerator
        {
            public static readonly string[] Nouns =
            {
                "process",
                "procedure",
                "work",
                "operation",
                "act",
                "event"
            };
            public static readonly string[] Adjectives =
            {
                "cool",
                "important",
                "lite",
                "insignificant",
                "hard",
                "common"
            };


            public static string GenerateName()
            {
                Random random = new(s_seed++);
                string name = string.Empty;
                name += Adjectives[random.Next() % Adjectives.Length];
                name += " " + Nouns[random.Next() % Nouns.Length];
                return name;
            }
        }
    }

    public enum Performance
    {
        Low = 2,
        Medium = 5,
        High = 10
    }

    public enum ProcessState
    {
        Ready,
        Waiting,
        Running,
        Finished
    }
}
