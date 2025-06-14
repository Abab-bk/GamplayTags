using BenchmarkDotNet.Attributes;
using Flowerfication.GameplayTags;

namespace Benchmark;

[MemoryDiagnoser]
public class TagContainerBenchmarks
{
    private const int TotalTags = 100_000;
    private const int ContainerTags = 50_000;
    private const int BatchSize = 1_000;

    private readonly Tag[] _allTags;
    private readonly TagContainer _container;
    private readonly Random _random = new(42);

    public TagContainerBenchmarks()
    {
        Console.WriteLine($"Registering {TotalTags} tags...");
        TagManager.Clear();

        _allTags = new Tag[TotalTags];
        for (var i = 0; i < TotalTags; i++)
        {
            var tag = TagManager.RegisterTag($"Tag.{i:D7}");
            _allTags[i] = tag;
        }

        _container = new TagContainer();
        Console.WriteLine($"Adding {ContainerTags} tags to container...");

        var selectedIndices = Enumerable.Range(0, TotalTags)
            .OrderBy(_ => _random.Next())
            .Take(ContainerTags)
            .ToArray();

        foreach (var index in selectedIndices) _container.AddTag(_allTags[index], _random.Next(1, 10));
    }

    [Benchmark]
    public void AddBatchTags()
    {
        var batch = Enumerable.Range(0, BatchSize)
            .Select(_ =>
            {
                var index = _random.Next(TotalTags);
                return (tag: _allTags[index], count: _random.Next(1, 5));
            })
            .ToArray();

        foreach (var item in batch) _container.AddTag(item.tag, item.count);
    }

    [Benchmark]
    public int RemoveBatchTags()
    {
        var totalRemoved = 0;
        for (var i = 0; i < BatchSize; i++)
        {
            var index = _random.Next(ContainerTags);
            totalRemoved += _container.RemoveTag(_allTags[index], _random.Next(1, 3));
        }

        return totalRemoved;
    }

    [Benchmark]
    public int CheckBatchExistence()
    {
        var existsCount = 0;
        for (var i = 0; i < BatchSize; i++)
        {
            var index = _random.Next(TotalTags);
            if (_container.HasTag(_allTags[index]))
                existsCount++;
        }

        return existsCount;
    }

    [Benchmark]
    public bool HasAllTags_LargeSet()
    {
        var testContainer = new TagContainer();
        for (var i = 0; i < BatchSize; i++)
        {
            var index = _random.Next(ContainerTags);
            testContainer.AddTag(_allTags[index]);
        }

        return _container.HasAllTags(testContainer);
    }

    [Benchmark]
    public bool HasAnyTag_LargeSet()
    {
        var testContainer = new TagContainer();
        for (var i = 0; i < BatchSize; i++)
        {
            var index = _random.Next(TotalTags);
            testContainer.AddTag(_allTags[index]);
        }

        return _container.HasAnyTag(testContainer);
    }

    [Benchmark]
    public void AddMaxStackTag()
    {
        var tag = _allTags[0];
        _container.AddTag(tag, int.MaxValue - _container.GetTagCount(tag));
    }

    [Benchmark]
    public int RemoveFromMaxStack()
    {
        var tag = _allTags[0];
        return _container.RemoveTag(tag, int.MaxValue);
    }
}