namespace CourseWork
{
    public class OS
    {
        private readonly Hardware _hardware;
        private readonly TactGenerator _generator;
        private readonly TaskScheduler _scheduler;


        public OS()
        {
            _generator = new(Config.TickRate);
            CPU[] cpus = new CPU[Config.CPUsCount];
            for (int i = 0; i < cpus.Length; i++)
            {
                cpus[i] = new CPU();
            }
            _hardware = new(new(cpus), new(Config.MemorySize));
            _scheduler = new(_hardware, _generator);
            SetupWorkflow();
        }

        private void SetupWorkflow()
        {
            foreach (var item in _hardware.CPUs)
            {
                _generator.OnTick += item.Tick;
            }
            _generator.OnTick += _scheduler.Update;
            RaiseSystemProcess();
        }

        private void RaiseSystemProcess()
        {
            Process systemProcess = new(0, "system", 15, ProcessState.Ready, Performance.Medium, 1_000_000);
            _scheduler.AddNewProcesses(new Process[] { systemProcess });
        }

        public void UpdateSystemState()
        {
            _generator.Tick();
        }

        public void RaiseNewProcess(Process process)
        {
            _scheduler.AddNewProcesses(process);
        }

        public void KillProcess(uint id)
        {
            _scheduler.AddTerminatingProcesses(id);
        }

        public override string ToString()
        {
            string result = "System info:\n";
            int i = 0;
            foreach (var item in _hardware.CPUs)
            {
                result += $"CPU {i++}:" + item + "\n";
            }
            result += "\nMemory: " + _hardware.Memory + "\n";
            result += "Processes:\n" + _scheduler;
            return result;
        }
    }
}
