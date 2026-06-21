namespace UrlShortener.DataStructures;

public sealed class CustomHashTable<TKey, TValue> where TKey : notnull
{
    private const int DefaultCapacity = 16;
    private const double LoadFactor = 0.75;

    private Bucket[] _buckets;
    private int _count;
    private readonly IEqualityComparer<TKey> _comparer;

    public CustomHashTable(int capacity = DefaultCapacity, IEqualityComparer<TKey>? comparer = null)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _buckets = CreateBuckets(capacity);
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
    }

    public int Count => _count;

    public bool TryGetValue(TKey key, out TValue value)
    {
        var bucketIndex = GetBucketIndex(key);
        var node = _buckets[bucketIndex].Head;

        while (node is not null)
        {
            if (_comparer.Equals(node.Key, key))
            {
                value = node.Value;
                return true;
            }

            node = node.Next;
        }

        value = default!;
        return false;
    }

    public void AddOrUpdate(TKey key, TValue value)
    {
        var bucketIndex = GetBucketIndex(key);
        var bucket = _buckets[bucketIndex];
        var node = bucket.Head;

        while (node is not null)
        {
            if (_comparer.Equals(node.Key, key))
            {
                node.Value = value;
                return;
            }

            node = node.Next;
        }

        bucket.Head = new HashNode<TKey, TValue>(key, value, bucket.Head);
        _count++;

        if ((double)_count / _buckets.Length >= LoadFactor)
        {
            Resize(_buckets.Length * 2);
        }
    }

    private int GetBucketIndex(TKey key)
    {
        var hashCode = _comparer.GetHashCode(key);
        return Math.Abs(hashCode) % _buckets.Length;
    }

    private void Resize(int newCapacity)
    {
        var oldBuckets = _buckets;
        _buckets = CreateBuckets(newCapacity);
        _count = 0;

        foreach (var bucket in oldBuckets)
        {
            var node = bucket.Head;
            while (node is not null)
            {
                AddOrUpdate(node.Key, node.Value);
                node = node.Next;
            }
        }
    }

    private static Bucket[] CreateBuckets(int capacity)
    {
        var buckets = new Bucket[capacity];
        for (var i = 0; i < capacity; i++)
        {
            buckets[i] = new Bucket();
        }

        return buckets;
    }

    private sealed class Bucket
    {
        public HashNode<TKey, TValue>? Head { get; set; }
    }
}

public sealed class HashNode<TKey, TValue> where TKey : notnull
{
    public TKey Key { get; }
    public TValue Value { get; set; }
    public HashNode<TKey, TValue>? Next { get; set; }

    public HashNode(TKey key, TValue value, HashNode<TKey, TValue>? next = null)
    {
        Key = key;
        Value = value;
        Next = next;
    }
}
