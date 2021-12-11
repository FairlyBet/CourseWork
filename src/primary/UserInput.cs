using System;
using System.Threading;
using System.Collections.Generic;

namespace CourseWork
{
    public class InputSystem
    {
        private readonly Action<IEnumerable<Process>, IEnumerable<uint>> _systemInterface;
        private readonly Thread _systemThread;
        private readonly List<Process> _newProcesses;
        private readonly List<uint> _terminatingProcesses;
        private readonly object _locker;


        public InputSystem(in Action<IEnumerable<Process>, IEnumerable<uint>> systemInterface,
            in Thread thread)
        {
            _systemInterface = systemInterface;
            _systemThread = thread;
            _newProcesses = new();
            _terminatingProcesses = new();
            _locker = new();
        }

        public void StartNewProcess(in Process process)
        {
            InterThreadingInterface(process, null, null);
        }

        public void TerminateProcess(in uint id)
        {
            InterThreadingInterface(null, id, null);
        }

        public void UpdateUserInput(in Thread caller)
        {
            InterThreadingInterface(null, null, caller);
        }

        private void InterThreadingInterface(in Process process, in uint? id, in Thread caller)
        {
            lock (_locker)
            {
                if (caller == _systemThread)
                {
                    _systemInterface.Invoke(_newProcesses, _terminatingProcesses);
                    _newProcesses.Clear();
                    _terminatingProcesses.Clear();
                    return;
                }
                if (process != null)
                {
                    _newProcesses.Add(process);
                    return;
                }
                if (id.HasValue)
                {
                    _terminatingProcesses.Add(id.Value);
                }
            }
        }
    }
}