using System.Collections.Generic;
using System.Linq;

namespace SqlNotebookScript.Interpreter.Ast;

public sealed class SqliteSyntaxProduction : Node
{
    public string Name { get; set; }
    public string Text { get; set; }
    public int StartToken { get; set; }
    public int NumTokens { get; set; }
    public List<SqliteSyntaxProduction> Items { get; set; } = new List<SqliteSyntaxProduction>();

    protected override IEnumerable<Node> GetChildren() => Items;

    public IEnumerable<SqliteSyntaxProduction> TraverseDottedChildren()
    {
        var stack = new Stack<SqliteSyntaxProduction>();
        stack.Push(this);

        while (stack.Any())
        {
            var n = stack.Pop();
            if (n == null || (n.Name != null && !n.Name.Contains(".")))
            {
                continue;
            }
            foreach (var item in n.Items.AsEnumerable().Reverse())
            {
                stack.Push(item);
            }
            yield return n;
        }
    }
}
