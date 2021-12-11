#undef DEBUG
using System.Threading;
using System.Collections.Generic;

namespace CourseWork
{
    public class OS
    {
        private readonly Thread _mainThread;
        private readonly Hardware _hardware;
        private readonly TactGenerator _generator;
        private readonly TaskScheduler _scheduler;
        public readonly InputSystem Input;


        public OS()
        {
            _generator = new(Config.TickRate);
            _mainThread = new(_generator.Run);
            CPU[] cpus = new CPU[Config.CPUsCount];
            for (int i = 0; i < cpus.Length; i++)
            {
                cpus[i] = new CPU();
            }
            _hardware = new(new(cpus), new(Config.MemorySize));
            _scheduler = new(_hardware, _generator);
            Input = new(SystemInterface, _mainThread);
            SetupWorkflow();
        }

        private void SetupWorkflow()
        {
            foreach (var item in _hardware.CPUs)
            {
                _generator.OnTick += item.Tick;
            }
            _generator.OnTick += UpdateUserInput;
            _generator.OnTick += _scheduler.Update;

#if DEBUG
            _generator.OnTick += PrintSystemInfo;
#endif
            RaiseSystemProcess();
        }

        private void UpdateUserInput()
        {
            Input.UpdateUserInput(_mainThread);
        }

        private void RaiseSystemProcess()
        {
            Process systemProcess = new(0, "system", 15, ProcessState.Ready, Performance.Medium, 1_000_000);
            _scheduler.AddNewProcesses(new Process[] { systemProcess });
        }

        private void SystemInterface(IEnumerable<Process> newProcesses,
            IEnumerable<uint> terminatingProcesses)
        {
            _scheduler.AddNewProcesses(newProcesses);
            _scheduler.AddTerminatingProcesses(terminatingProcesses);
        }

#if DEBUG
        public void PrintSystemInfo()
        {
            System.Console.Clear();
            System.Console.WriteLine(this);
        }
#endif

        public void SetToAutomaticMode()
        {
            if (_mainThread.ThreadState == ThreadState.Unstarted)
            {
                _mainThread.Start();
            }
            _generator.Pause = false;
        }

        public void KillAutomaticMode()
        {
            _generator.Exit = true;
        }

        public void SetToManualMode()
        {
            _generator.Pause = true;
        }

        public void UpdateSystemState()
        {
            _generator.Tick();
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
