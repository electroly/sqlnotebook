using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNotebookScript.Utils;

namespace SqlNotebook {
    public partial class LoadingContainerControl : UserControl {
        private Control _control;
        private int _loadCount = 0;
        private string _errorMessage = null;

        public string ErrorMessage => _errorMessage;

        public Slot<bool> IsLoading { get; } = new Slot<bool>();
        public Slot<bool> IsError { get; } = new Slot<bool>();
        public Slot<bool> IsOverlayVisible { get; } = new Slot<bool>();

        public Control ContainedControl {
            get {
                return _control;
            }
            set {
                _control = value;
                _control.Dock = DockStyle.Fill;
                Controls.Add(_control);
                _loadingLbl.Visible = false;
            }
        }

        public void PushLoad() {
            _loadCount++;
            ShowHideLoadingLbl();
        }

        public void PopLoad() {
            _loadCount--;
            ShowHideLoadingLbl();
        }

        public async Task DoLoad(Func<Task> action) {
            PushLoad();
            try {
                await action();
            } finally {
                PopLoad();
            }
        }

        public void SetError(string message) {
            _errorMessage = message;
            ShowHideLoadingLbl();
        }

        public void ClearError() {
            _errorMessage = null;
            ShowHideLoadingLbl();
        }

        private void ShowHideLoadingLbl() {
            bool loading = _loadCount > 0;
            bool error = _errorMessage != null;

            _loadingLbl.Text = error ? _errorMessage : "Please wait...";
            _loadingLbl.Visible = error || loading;
            _loadingLbl.BringToFront(); // harmless if the label is invisible

            IsLoading.Value = loading;
            IsError.Value = error;
            IsOverlayVisible.Value = error || loading;
        }

        public LoadingContainerControl() {
            InitializeComponent();
        }
    }
}
