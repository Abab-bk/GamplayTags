namespace Flowerfication.GameplayTags;

public readonly struct Tag(int hash) : IEquatable<Tag>
{
    public int Hash => hash;
    
    public bool Equals(Tag other) => Hash == other.Hash;

    public override bool Equals(object? obj)
    {
        return obj is Tag other && Equals(other);
    }

    public override int GetHashCode() => Hash;

    public static bool operator ==(Tag left, Tag right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Tag left, Tag right)
    {
        return !(left == right);
    }
}