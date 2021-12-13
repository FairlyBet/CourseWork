using System;
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

    public readonly struct ProcessStatistic : IEquatable<ProcessStatistic>
    {
        public uint Id { get; }

        public string Name { get; }

        public byte Priority { get; }

        public ProcessState State { get; }

        public Performance Performance { get; }

        public int ExecutingTime { get; }

        public uint BurstTime { get; }

        public string Size { get; }


        public ProcessStatistic(Process process)
        {
            Id = process.Id;
            Name = process.Name;
            Priority = process.Priority;
            State = process.State;
            Performance = process.Performance;
            ExecutingTime = process.ExecutingTime;
            BurstTime = process.BurstTime;
            Size = (process.Size / 1_000_000).ToString() + " MB";
        }

        public static explicit operator ProcessStatistic(Process process)
        {
            return new(process);
        }

        public bool Equals(ProcessStatistic other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }

        public override bool Equals(object obj)
        {
            return obj is ProcessStatistic && Equals((ProcessStatistic)obj);
        }
    }

    public readonly struct CPUStatistic
    {
        public readonly string State { get; }


        public CPUStatistic(CPU cpu, int id)
        {
            State = $"CPU {id}: ";
            if (cpu.CurrentProcess == null)
            {
                State += "free";
            }
            else
            {
                State += "busy";
            }
        }
    }
}
