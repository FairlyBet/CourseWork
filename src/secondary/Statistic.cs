using System.Collections.Generic;

namespace CourseWork
{
    public readonly struct Statistic
    {
        public ReadOnlyList<ProcessStatistic> Processes { get; }

        public ReadOnlyList<ProcessStatistic> RejectedProcesses { get; }

        public ReadOnlyList<ProcessStatistic> TerminatedProcesses { get; }

        public ReadOnlyList<CPUStatistic> CPUStatistics { get; }

        public ReadOnlyList<MemoryBlock> MemoryBlocks { get; }


        public Statistic(IEnumerable<ProcessStatistic> processesStatistic, 
            IEnumerable<ProcessStatistic> rejectedProcessesStatistic, 
            IEnumerable<ProcessStatistic> terminatedProcesses, 
            IEnumerable<CPUStatistic> cpuStatistics, IEnumerable<MemoryBlock> memoryBlocks)
        {
            Processes = new(processesStatistic);
            RejectedProcesses = new(rejectedProcessesStatistic);
            TerminatedProcesses = new(terminatedProcesses);
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
            if (cpu.CurrentProcess == null)
            {
                CurrentProcess = null;
            }
            else
            {
                CurrentProcess = new(cpu.CurrentProcess);
            }
        }

        public static explicit operator CPUStatistic(CPU cpu)
        {
            return new(cpu);
        }
    }
}
