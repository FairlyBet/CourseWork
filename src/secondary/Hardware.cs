namespace CourseWork
{
    public readonly struct Hardware
    {
        public readonly ReadOnlyList<CPU> CPUs;
        public readonly Memory Memory;

        public Hardware(in ReadOnlyList<CPU> cpus, in Memory memory)
        {
            CPUs = cpus;
            Memory = memory;
        }
    }
}
