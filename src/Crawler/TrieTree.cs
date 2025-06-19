namespace Crawler;

internal sealed class TrieTree(Uri root)
{
    private Node Root { get; } = new Node(root);

    public bool TryInsert(Uri value)
    {
        throw new NotImplementedException();
    }

    public bool Contains(Uri value)
    {
        throw new NotImplementedException();
    }

    private sealed class Node(Uri value)
    {
        public Uri Value { get; init; } = value;
        
        private Dictionary<Uri, Node> Children { get; } = new();
        
        public bool IsTerminal() => Children.Count == 0;

        public bool Add(Node child)
        {
            return false;
        }

        public bool Remove(Node child)
        {
            return false;
        }
    }
}
