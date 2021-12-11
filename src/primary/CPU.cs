namespace CourseWork
{
    public class CPU
    {
        public Process CurrentProcess { get; set; }


        public void Tick()
        {
            CurrentProcess?.Execute();
        }
    
        public override string ToString()
        {
            return CurrentProcess == null ? "[free]" : "[busy]";
        }
    }
}
