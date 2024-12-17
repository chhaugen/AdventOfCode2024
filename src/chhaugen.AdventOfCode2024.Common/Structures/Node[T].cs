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

    public IEnumerable<Node<T>> GetAllChildren()
    {
        if (!IsLeaf)
            foreach (var child in Children)
            {
                var innerChildren = child.GetAllChildren();
                foreach (var innerChild in innerChildren)
                    yield return innerChild;
            }
        yield return this;
    }

    public Node<T> Clone(Node<T>? newParent = null)
    {
        Node<T> newNode = new(Value, newParent);
        foreach (var child in Children)
        {
            newNode.Children.Add(child.Clone(newNode));
        }
        return newNode;
    }

    object ICloneable.Clone()
        => Clone();
}
