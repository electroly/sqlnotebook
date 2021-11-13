namespace SqlNotebook.Pages;

public sealed class AddBlockEventArgs {
    public DividerBlockControl Divider { get; }
    public BlockType Type { get; }

    public AddBlockEventArgs(DividerBlockControl divider, BlockType type) {
        Divider = divider;
        Type = type;
    }
}
