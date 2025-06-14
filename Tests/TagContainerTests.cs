using Flowerfication.GameplayTags;

namespace Tests;

public class TagContainerTests
{
    private Tag _fireBall;
    private Tag _fireBolt;
    
    [SetUp]
    public void Setup()
    {
        _fireBall = TagManager.RegisterTag("Ability.Fire.Fireball");
        _fireBolt = TagManager.RegisterTag("Ability.Fire.Firebolt");
    }

    [Test]
    public void HasAllTags_ReturnTrue()
    {
        var container = new TagContainer([_fireBall, _fireBolt]);
        var otherContainer = new TagContainer([_fireBall]);
        
        Assert.That(container.HasAllTags(otherContainer), Is.True);
    }

    [Test]
    public void HasAnyTag_ReturnTrue()
    {
        var container = new TagContainer([_fireBall, _fireBolt]);
        var otherContainer = new TagContainer([_fireBall]);
        
        Assert.That(container.HasAnyTag(otherContainer), Is.True);
    }

    [Test]
    public void ToString_ReturnsCorrectString()
    {
        var container = new TagContainer([_fireBall, _fireBolt]);
        
        Assert.That(container.ToString(),
            Is.EqualTo("Ability.Fire.Fireball, Ability.Fire.Firebolt"));
    }
    
    [Test]
    public void AddTag_WithPositiveCount()
    {
        var container = new TagContainer([_fireBall, _fireBolt]);
        container.AddTag(_fireBall, 2);
        
        Assert.That(container.GetTagCount(_fireBall), Is.EqualTo(3));
    }

    [Test]
    public void RemoveTag_NonExistingTag_ReturnsZeroAndNoChanges()
    {
        var container = new TagContainer();
        var removed = container.RemoveTag(_fireBolt, 2);
        Assert.That(removed, Is.EqualTo(0));
    }

    [Test]
    public void RemoveTag_WithPositiveCount()
    {
        var container = new TagContainer([_fireBall]);
        container.RemoveTag(_fireBall, 2);
        
        Assert.That(container.GetTagCount(_fireBall), Is.EqualTo(0));
    }
}