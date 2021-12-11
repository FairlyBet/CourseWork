namespace CourseWork
{
    public class MemoryBlock
    {
        public readonly BlockState State;
        public readonly uint Address;
        public readonly uint Size;


        public MemoryBlock(in BlockState state, in uint address, in uint size)
        {
            State = state;
            Address = address;
            Size = size;
        }
    }

    public enum BlockState
    {
        Empty,
        Occupied
    }
}
