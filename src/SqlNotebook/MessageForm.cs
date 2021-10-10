using System.Windows.Forms;

namespace SqlNotebook {
    public static class MessageForm {
        public static void ShowError(IWin32Window owner, string title, string message, string details = null) {
            if (details != null) {
                message += "\r\n\r\n" + details;
            }
            MessageBox.Show(owner, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
