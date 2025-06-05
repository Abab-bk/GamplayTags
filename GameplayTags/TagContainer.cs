namespace Flowerfication.GameplayTags;

public class TagContainer
{
    private HashSet<int> TagHashes { get; } = [];

    public void AddTag(Tag tag) => TagHashes.Add(tag.Hash);
    public void RemoveTag(Tag tag) => TagHashes.Remove(tag.Hash);
    
    public bool HasTag(Tag tag) => TagHashes.Contains(tag.Hash);
    public bool HasAllTags(TagContainer other) => TagHashes.IsSupersetOf(other.TagHashes);
    public bool HasAnyTag(TagContainer other) => TagHashes.Overlaps(other.TagHashes);
    
    public override string ToString() => string.Join(", ",
        TagHashes
            .Where(tagHash => TagManager.GetTag(tagHash, out _))
            .Select(TagManager.GetTagPathByHash)
    );

    public TagContainer()
    {
    }

    public TagContainer(IEnumerable<Tag> tags)
    {
        foreach (var tag in tags)
        {
            AddTag(tag);
        }
    }
}