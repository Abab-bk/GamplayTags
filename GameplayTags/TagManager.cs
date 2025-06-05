namespace Flowerfication.GameplayTags;

public static class TagManager
{
    private static readonly Dictionary<string, Tag> TagsByString =
        new(StringComparer.OrdinalIgnoreCase);
    private static readonly Dictionary<int, string> StringsByHash = new();
    private static readonly Dictionary<int, HashSet<int>> Hierarchy = new();

    public static void Clear()
    {
        TagsByString.Clear();
        StringsByHash.Clear();
        Hierarchy.Clear();
    }
    
    public static string GetTagPathByHash(int hash)
    {
        if (StringsByHash.TryGetValue(hash, out var tagPath))
        {
            return tagPath;
        }
        throw new ArgumentException($"Tag hash '{hash}' not found", nameof(hash));
    }
    
    public static bool GetTag(string tagPath, out Tag tag)
    {
        if (TagsByString.TryGetValue(tagPath, out var tagValue))
        {
            tag = tagValue;
            return true;
        }
        
        tag = default;
        return false;
    }

    public static Tag GetTag(string tagPath)
    {
        if (TagsByString.TryGetValue(tagPath, out var tagValue))
        {
            return tagValue;
        }
        throw new ArgumentException($"Tag '{tagPath}' not found", nameof(tagPath));
    }

    public static bool GetTag(int hash, out Tag tag)
    {
        if (StringsByHash.TryGetValue(hash, out var tagPath))
        {
            tag = GetTag(tagPath);
            return true;
        }
        
        tag = default;
        return false;
    }
    
    public static bool HasTag(int hash) => StringsByHash.ContainsKey(hash);
    public static bool HasTag(string tagPath) => TagsByString.ContainsKey(tagPath);
    
    public static Tag RegisterTag(string tagPath)
    {
        if (TagsByString.TryGetValue(tagPath, out var existingTag)) return existingTag;
        
        var hash = tagPath.GetHashCode();
        var newTag = new Tag(hash);
        
        TagsByString[tagPath] = newTag;
        StringsByHash[hash] = tagPath;
        
        BuildHierarchy(tagPath, hash);
        return newTag;
    }
    
    
    public static IEnumerable<Tag> GetParentTags(Tag tag)
    {
        return Hierarchy.TryGetValue(tag.Hash, out var parentHashes) ?
            parentHashes.Select(h => new Tag(h)) : [];
    }
    
    
    private static void BuildHierarchy(string fullPath, int currentHash)
    {
        var parts = fullPath.Split('.');
    
        var currentPath = "";
        var parentHash = 0;
    
        foreach (var part in parts)
        {
            currentPath = string.IsNullOrEmpty(currentPath) ? part : $"{currentPath}.{part}";
        
            var hash = currentPath.GetHashCode();
            if (!StringsByHash.ContainsKey(hash))
            {
                TagsByString[currentPath] = new Tag(hash);
                StringsByHash[hash] = currentPath;
            }
        
            if (parentHash != 0)
            {
                if (!Hierarchy.TryGetValue(hash, out var parents))
                    Hierarchy[hash] = parents = [];
                parents.Add(parentHash);
            }
        
            parentHash = hash;
        }
    }
}