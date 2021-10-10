using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public static class Bind {
        public static void OnChange(IEnumerable<Slot> slots, EventHandler handler) {
            foreach (var slot in slots) {
                slot.ChangeNoData += () => handler?.Invoke(slot, EventArgs.Empty);
            }
        }

        public static void BindAny(IReadOnlyList<Slot<bool>> slots, Action<bool> handler) {
            Action wrapper = () => handler(slots.Any(x => x.Value));
            foreach (var slot in slots) {
                slot.ChangeNoData += wrapper;
            }
        }
        
        public static void BindValue(this NumericUpDown self, Slot<int> slot) {
            slot.Value = (int)self.Value;
            slot.Change += (old, val) => {
                if (self.Value != val) {
                    self.Value = val;
                }
            };
            self.ValueChanged += (sender, e) => {
                slot.Value = (int)self.Value;
            };
        }

        public static void BindChecked(this CheckBox self, Slot<bool> slot) {
            slot.Value = self.Checked;
            slot.Change += (old, val) => {
                if (self.Checked != val) {
                    self.Checked = val;
                }
            };
            self.CheckedChanged += (sender, e) => {
                slot.Value = self.Checked;
            };
        }

        public static void BindSelectedValue<T>(this ComboBox self, Slot<T> slot) {
            slot.Value = (T)self.SelectedValue;
            slot.Change += (old, val) => {
                var ctlVal = self.SelectedValue;
                if ((ctlVal == null && val != null) || (ctlVal != null && !ctlVal.Equals(val))) {
                    self.SelectedValue = val;
                }
            };
            self.SelectedValueChanged += (sender, e) => {
                slot.Value = (T)self.SelectedValue;
            };
        }

        public static void BindText(this ComboBox self, Slot<string> slot) {
            slot.Value = self.Text;
            slot.Change += (old, val) => {
                if (self.Text != val) {
                    self.Text = val;
                }
            };
            self.TextChanged += (sender, e) => {
                slot.Value = self.Text;
            };
        }
    }
}
