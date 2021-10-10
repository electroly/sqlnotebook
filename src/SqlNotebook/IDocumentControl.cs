using System.Threading.Tasks;

namespace SqlNotebook {
    public interface IDocumentControl {
        string ItemName { get; set; }
        void Save();
    }
}
