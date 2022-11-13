using System.Collections.Generic;
using System.Linq;

namespace SqlNotebookScript.Interpreter.Ast;

public abstract class Node
{
    public Token SourceToken { get; set; }

    // each node will implement only one of the following three:
    protected virtual bool IsLeaf { get; } = false; // if the node has no children

    protected virtual Node GetChild()
    {
        return null;
    } // if the node only has one child

    protected virtual IEnumerable<Node> GetChildren()
    {
        return _empty;
    } // if the node has multiple children

    private static Node[] _empty = new Node[0];
    public IEnumerable<Node> Children
    {
        get
        {
            if (IsLeaf)
            {
                return _empty;
            }
            var child = GetChild();
            if (child != null)
            {
                return new Node[] { child };
            }
            else
            {
                return GetChildren();
            }
        }
    }

    public IEnumerable<Node> Traverse()
    {
        var stack = new Stack<Node>();
        stack.Push(this);

        while (stack.Any())
        {
            var n = stack.Pop();
            if (!n.IsLeaf)
            {
                var onlyChild = n.GetChild();
                if (onlyChild != null)
                {
                    stack.Push(onlyChild);
                }
                else
                {
                    foreach (var child in n.GetChildren().Reverse())
                    {
                        if (child != null)
                        {
                            stack.Push(child);
                        }
                    }
                }
            }
            yield return n;
        }
    }
}
