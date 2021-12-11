namespace CourseWork
{
    public static class Config
    {
        public static readonly uint MemorySize = 4_000_000_000;
        public static readonly byte MaxPriority = 31;
        public static readonly uint MinProccesSize = 500;
        public static readonly uint MaxProccesSize = MemorySize / 2;
        public static readonly int TickRate = 500;
        public static readonly int CPUsCount = 4;
    }
}
