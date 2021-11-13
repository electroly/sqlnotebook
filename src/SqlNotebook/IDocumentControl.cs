namespace SqlNotebook;

public interface IDocumentControl {
    string ItemName { get; set; }
    void Save();
}
