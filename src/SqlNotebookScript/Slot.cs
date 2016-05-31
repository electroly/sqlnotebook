// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

namespace SqlNotebookScript {
    public abstract class Slot {
        public event Action ChangeNoData;

        protected void SendChangeNoDataEvent() {
            ChangeNoData?.Invoke();
        }

        public static void Bind(Action set, params Slot[] deps) {
            foreach (var dep in deps) {
                dep.ChangeNoData += set;
            }
            set();
        }
    }

    public sealed class Slot<T> : Slot {
        private T _value;

        private readonly object _lock = new object();

        public T Value {
            get {
                lock (_lock) {
                    return _value;
                }
            }
            set {
                lock (_lock) {
                    bool didChange =
                        (value == null && _value != null) ||
                        (value != null && !value.Equals(_value));
                    if (didChange) {
                        var oldValue = _value;
                        _value = value;
                        Change?.Invoke(oldValue, value);
                        SendChangeNoDataEvent();
                    }
                }
            }
        }

        public delegate void ChangeHandler(T oldValue, T newValue);
        public event ChangeHandler Change;

        public static implicit operator T(Slot<T> self) {
            return self._value;
        }
    }

    // a slot that has no data; it is only used to trigger events
    public sealed class NotifySlot : Slot {
        public void Notify() {
            SendChangeNoDataEvent();
        }
    }
}
