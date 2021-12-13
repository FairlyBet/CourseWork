namespace CourseWork
{
    public class OS
    {
        private readonly Hardware _hardware;
        private readonly TactGenerator _generator;
        private readonly TaskScheduler _scheduler;
        private Statistic _statistic;

        public Statistic Statistic => _statistic;


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
            UpdateStatistic();
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
            Process systemProcess = new(0, "system", 15, Performance.Medium, 1_000_000);
            _scheduler.AddNewProcesses(new Process[] { systemProcess });
        }

        public void UpdateSystemState()
        {
            _generator.Tick();
            UpdateStatistic();
        }

        private void UpdateStatistic()
        {
            var cpus = new CPUStatistic[Config.CPUsCount];
            for (int i = 0; i < cpus.Length; i++)
            {
                cpus[i] = new(_hardware.CPUs[i], i);
            }
            var memory = _hardware.Memory.Blocks;
            var (processes, rejected, terminated) = _scheduler.ProvideStatistic();
            _statistic = new(processes, rejected, terminated, cpus, memory);
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
