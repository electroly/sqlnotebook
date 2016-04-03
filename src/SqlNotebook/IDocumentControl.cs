using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlNotebook {
    public interface IDocumentControl {
        string ItemName { get; set; }
        string DocumentText { get; }
    }
}
