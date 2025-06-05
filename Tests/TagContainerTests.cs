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
}