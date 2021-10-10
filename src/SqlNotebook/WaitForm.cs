using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class WaitForm : Form {
        public Task WaitTask;
        public Exception ResultException { get; private set; }

        public WaitForm(string title, string text, Action action) {
            InitializeComponent();
            Text = title;
            _infoTxt.Text = text;

            Ui ui = new(this, 65, 8);
            ui.Init(_table);
            ui.Init(_infoTxt);
            ui.Init(_progressBar);

            WaitTask = Task.Run(() => {
                try {
                    action();
                } catch (Exception ex) {
                    ResultException = ex;
                }
                while (!IsHandleCreated) {
                    Thread.Sleep(1);
                }
                BeginInvoke(new MethodInvoker(() => {
                    DialogResult = ResultException == null ? DialogResult.OK : DialogResult.Abort;
                    Close();
                }));
            });
        }
    }
}
