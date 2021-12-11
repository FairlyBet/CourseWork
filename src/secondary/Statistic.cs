using System.Collections.Generic;

namespace CourseWork
{
    public readonly struct Statistic
    {
        public ReadOnlyList<ProcessStatistic> ProcessesStatistic { get; }
        public ReadOnlyList<CPUStatistic> CPUStatistics { get; }
        public ReadOnlyList<MemoryBlock> MemoryBlocks { get; }


        public Statistic(IEnumerable<ProcessStatistic> processesStatistic, 
            IEnumerable<CPUStatistic> cpuStatistics, IEnumerable<MemoryBlock> memoryBlocks)
        {
            ProcessesStatistic = new(processesStatistic);
            CPUStatistics = new(cpuStatistics);
            MemoryBlocks = new(memoryBlocks);
        }
    }

    public readonly struct ProcessStatistic
    {
        public uint Id { get; }

        public string Name { get; }

        public byte Priority { get; }

        public ProcessState State { get; }

        public Performance Performance { get; }

        public int ExecutingTime { get; }

        public uint BurstTime { get; }

        public uint Size { get; }


        public ProcessStatistic(Process process)
        {
            Id = process.Id;
            Name = process.Name;
            Priority = process.Priority;
            State = process.State;
            Performance = process.Performance;
            ExecutingTime = process.ExecutingTime;
            BurstTime = process.BurstTime;
            Size = process.Size;
        }

        public static explicit operator ProcessStatistic(Process process)
        {
            return new(process);
        }
    }

    public readonly struct CPUStatistic
    {
        public ProcessStatistic? CurrentProcess { get; }


        public CPUStatistic(CPU cpu)
        {
            CurrentProcess = (ProcessStatistic)cpu.CurrentProcess;
        }

        public static explicit operator CPUStatistic(CPU cpu)
        {
            return new(cpu);
        }
    }
}
