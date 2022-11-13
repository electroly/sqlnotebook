namespace SqlNotebook;

public interface IDocumentControl
{
    string ItemName { get; set; }
    void Save();
}

public interface IDocumentControlOpenNotification
{
    void OnOpen();
}
