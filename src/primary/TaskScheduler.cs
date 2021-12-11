using System.Collections.Generic;
using System.Linq;

namespace CourseWork
{
    public class TaskScheduler
    {
        private readonly Hardware _hardware;
        private readonly TactGenerator _generator;
        private readonly List<Process> _processes;
        private readonly List<Process> _newProcesses;
        private readonly List<uint> _terminatingProcesses;
        private uint _idCount;
        private int _tacts;


        public TaskScheduler(in Hardware hardware, in TactGenerator generator)
        {
            _generator = generator;
            _hardware = hardware;
            _processes = new();
            _newProcesses = new();
            _terminatingProcesses = new();
        }

        public void AddNewProcesses(params Process[] newProcesses)
        {
            foreach (var item in newProcesses)
            {
                Process process = new(_idCount++, item.Name, item.Priority,
                    item.State, item.Performance, item.Size);
                _newProcesses.Add(process);
            }
        }

        public void AddTerminatingProcesses(params uint[] terminatingProcesses)
        {
            foreach (var item in terminatingProcesses)
            {
                if (!_terminatingProcesses.Contains(item))
                {
                    _terminatingProcesses.Add(item);
                }
            }
        }

        public void Update()
        {
            ReleaseCPUs();
            RaiseNewProcesses();
            TerminateProcesses();
            ScheduleProcesses();
            OrderList();
        }

        private void ReleaseCPUs()
        {
            foreach (var item in _hardware.CPUs.Where(x => x.CurrentProcess != null
                && x.CurrentProcess.State == ProcessState.Waiting))
            {
                item.CurrentProcess = null;
            }
        }

        private void RaiseNewProcesses()
        {
            var raisedProcesses = _newProcesses.Where(x => _hardware.Memory.TryAddProcces(x));
            foreach (var item in raisedProcesses)
            {
                _processes.Add(item);
                _generator.OnTick += item.UpdateState;
            }
            _newProcesses.Clear();
        }

        private void TerminateProcesses()
        {
            foreach (var item in _terminatingProcesses)
            {
                var process = _processes.Where(x => x.Id == item).First();
                if (process != null && process.Id != 0)
                {
                    _hardware.Memory.DeleteProcess(process);
                    process.FinishProcess();
                    _generator.OnTick -= process.UpdateState;   
                    _processes.Remove(process);
                }
            }
        }

        private void ScheduleProcesses()
        {
            foreach (var item in _hardware.CPUs.Where(x => x.CurrentProcess == null))
            {
                var p = _processes.Where(x => x.State == ProcessState.Ready).MaxBy(x => x.Priority);
                p?.StartExecuting();
                item.CurrentProcess = p;
            }
        }

        private void OrderList()
        {
            _tacts++;
            if (_tacts == Config.OrderRate)
            {
                _processes.Sort(Process.Comparer);
            }
            _tacts %= Config.OrderRate;
        }

        public ProcessStatistic[] ProvideStatistic()
        {
            var result = new ProcessStatistic[_processes.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (ProcessStatistic)_processes[i];
            }
            return result;
        }

        public override string ToString()
        {
            string result = string.Empty;
            foreach (var item in _processes)
            {
                result += item + "\n";
            }
            return result;
        }
    }
}
