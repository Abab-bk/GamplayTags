namespace Flowerfication.GameplayTags;

public class TagContainer
{
    private Dictionary<int, int> Tags { get; } = new();

    public TagContainer(TagContainer other)
    {
        Tags = new Dictionary<int, int>(other.Tags);
    }
    
    public void AddTag(Tag tag, int count = 1)
    {
        if (count <= 0) return;

        if (Tags.TryGetValue(tag.Hash, out var currentCount))
        {
            Tags[tag.Hash] = currentCount + count;
            return;
        }
        Tags.Add(tag.Hash, count);
    }
    
    public int RemoveTag(Tag tag, int count = 1)
    {
        if (count <= 0) return 0;
        if (!Tags.TryGetValue(tag.Hash, out var currentCount)) return 0;
        
        var actualRemove = Math.Min(count, currentCount);
        var newCount = currentCount - actualRemove;
    
        if (newCount > 0)
        {
            Tags[tag.Hash] = newCount;
        }
        else
        {
            Tags.Remove(tag.Hash);
        }
    
        return actualRemove;
    }
    
    public int RemoveTagCompletely(Tag tag) =>
        Tags.Remove(tag.Hash, out var currentCount) ? currentCount : 0;
    
    public bool HasTag(Tag tag) => Tags.ContainsKey(tag.Hash);
    
    public int GetTagCount(Tag tag) => Tags.GetValueOrDefault(tag.Hash, 0);
    
    public bool HasAllTags(TagContainer other) => 
        other.Tags.Keys.All(Tags.ContainsKey);
    
    public bool HasAnyTag(TagContainer other) => 
        other.Tags.Keys.Any(Tags.ContainsKey);
    
    public override string ToString() => string.Join(", ",
        Tags.Select(pair => 
            $"{TagManager.GetTagPathByHash(pair.Key)}" + 
            (pair.Value > 1 ? $"({pair.Value})" : "")
        )
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