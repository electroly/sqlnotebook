using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlNotebook {
    public partial class ImportConnectionForm : Form {
        public ImportConnectionForm(string title, DbConnectionStringBuilder builder) {
            InitializeComponent();
            _propertyGrid.SelectedObject = builder;
            Text = title;
        }
    }
}
