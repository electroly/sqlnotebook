namespace SqlNotebookScript;

public sealed class Token
{
    public TokenType Type;
    public string Text;

    public override string ToString() => $"{Type}: \"{Text}\"";

    public ulong Utf8Start;
    public ulong Utf8Length;
}
