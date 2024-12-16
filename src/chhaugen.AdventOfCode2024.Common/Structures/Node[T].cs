namespace chhaugen.AdventOfCode2024.Common.Structures;
public class Node<T> : ICloneable
{
    public Node(T value)
    {
        Value = value;
    }

    public Node(T value, Node<T>? parent)
        : this(value)
    {
        Parent = parent;
    }

    public Node(T value, Node<T>? parent, params List<Node<T>> children)
        : this(value, parent)
    {
        Children = children;
    }

    public T Value { get; set; }

    public Node<T>? Parent { get; set; }

    public List<Node<T>> Children { get; set; } = [];

    public bool IsLeaf => Children.Count == 0;

    public IEnumerable<Node<T>> GetAncestors()
    {
        yield return this;
        if (Parent != null)
            foreach(var node in Parent.GetAncestors())
                yield return node;
    }

    public IEnumerable<Node<T>> GetLeafs()
    {
        if (IsLeaf)
        {
            yield return this;
        }
        else
        {
            foreach (var child in Children)
            {
                var leafs = child.GetLeafs();
                foreach (var leaf in leafs)
                    yield return leaf;
            }
        }
    }

    public Node<T> Clone()
        => new(Value, Parent, [.. Children]);

    object ICloneable.Clone()
        => Clone();
}
