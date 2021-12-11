namespace CourseWork
{
    public readonly struct Statistic
    {

    }

    public readonly struct ProcessesStatistic
    {
        public uint Id { get; }

        public string Name { get; }

        public byte Priority { get; }

        public ProcessState State { get; }

        public Performance Performance { get; }

        public int ExecutingTime { get; }

        public uint BurstTime { get; }

        public uint Size { get; }
    }
}