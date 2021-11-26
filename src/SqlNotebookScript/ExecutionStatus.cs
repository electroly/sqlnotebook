using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SqlNotebookScript;

/// <summary>
/// Used by WaitForm to get insight into the currently executing statement.
/// </summary>
public static class ExecutionStatus {
    private static readonly List<InFlight> _stack = new(); // locked

    public static string Status => _stack.Count == 0 ? null : _stack[^1].Status;

    public static InFlight Start(string target) {
        lock (_stack) {
            InFlight f = new(target);
            _stack.Add(f);
            return f;
        }
    }

    public sealed class InFlight : IDisposable {
        private string _target;
        private string _status;
        
        public string Status { get; private set; }

        public InFlight(string target) {
            _target = target;
        }

        public void Dispose() {
            lock (_stack) {
                var success = _stack.Remove(this);
                Debug.Assert(success);
            }
        }

        public void SetTarget(string target) {
            _target = target;
            Set();
        }

        public void SetStatus(string status) {
            _status = status;
            Set();
        }

        private void Set() {
            Status =
                _target == null
                ? _status ?? ""
                : _target + Environment.NewLine + _status ?? "";
        }
    }
}
