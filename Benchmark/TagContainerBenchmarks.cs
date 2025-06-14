using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Flowerfication.GameplayTags;

namespace Benchmark;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
public class TagContainerBenchmarks
{
    private const int TotalTags = 100_000;
    private const int ContainerTags = 50_000;
    private const int BatchSize = 1_000;

    private readonly Random _random = new(42);

    private Tag[] _allTags = default!;
    private (Tag tag, int count)[] _batchToAdd = default!;
    private (Tag tag, int count)[] _batchToRemove = default!;
    private Tag[] _batchToCheck = default!;
    private TagContainer _testContainerForHasAll = default!;
    private TagContainer _testContainerForHasAny = default!;

    private TagContainer _pristineContainer = default!;
    private TagContainer _containerForMutation = default!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Console.WriteLine($"Registering {TotalTags} tags...");
        TagManager.Clear();
        _allTags = new Tag[TotalTags];
        for (var i = 0; i < TotalTags; i++) _allTags[i] = TagManager.RegisterTag($"Tag.{i:D7}");

        Console.WriteLine($"Creating pristine container with {ContainerTags} tags...");
        var selectedIndices = Enumerable.Range(0, TotalTags)
            .OrderBy(_ => _random.Next())
            .Take(ContainerTags)
            .ToArray();
        _pristineContainer = new TagContainer();
        foreach (var index in selectedIndices) _pristineContainer.AddTag(_allTags[index], _random.Next(1, 10));

        Console.WriteLine("Pre-generating batch data for benchmarks...");

        _batchToAdd = Enumerable.Range(0, BatchSize)
            .Select(_ => (_allTags[_random.Next(TotalTags)], _random.Next(1, 5)))
            .ToArray();

        _batchToRemove = Enumerable.Range(0, BatchSize)
            .Select(_ => (_allTags[_random.Next(TotalTags)], _random.Next(1, 3)))
            .ToArray();

        _batchToCheck = Enumerable.Range(0, BatchSize)
            .Select(_ => _allTags[_random.Next(TotalTags)])
            .ToArray();

        var tagsForHasAll = selectedIndices.OrderBy(_ => _random.Next()).Take(BatchSize).Select(i => _allTags[i]);
        _testContainerForHasAll = new TagContainer(tagsForHasAll);
        
        var tagsForHasAny = Enumerable.Range(0, BatchSize).Select(_ => _allTags[_random.Next(TotalTags)]);
        _testContainerForHasAny = new TagContainer(tagsForHasAny);
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _containerForMutation = new TagContainer(_pristineContainer);
    }

    [Benchmark]
    public void AddBatchTags()
    {
        foreach (var item in _batchToAdd)
            _containerForMutation.AddTag(item.tag, item.count);
    }

    [Benchmark]
    public int RemoveBatchTags()
    {
        var totalRemoved = 0;
        foreach (var item in _batchToRemove) totalRemoved +=
            _containerForMutation.RemoveTag(item.tag, item.count);
        return totalRemoved;
    }

    [Benchmark]
    public int CheckBatchExistence()
    {
        var existsCount = 0;
        foreach (var tag in _batchToCheck)
            if (_pristineContainer.HasTag(tag))
                existsCount++;
        return existsCount;
    }

    [Benchmark]
    public bool HasAllTags_LargeSet() =>
        _pristineContainer.HasAllTags(_testContainerForHasAll);

    [Benchmark]
    public bool HasAnyTag_LargeSet() =>
        _pristineContainer.HasAnyTag(_testContainerForHasAny);

    [Benchmark]
    public void AddMaxStackTag()
    {
        var tag = _allTags[0];
        _containerForMutation
            .AddTag(tag, int.MaxValue - _containerForMutation.GetTagCount(tag));
    }

    [Benchmark]
    public int RemoveFromMaxStack()
    {
        var tag = _allTags[0];
        return _containerForMutation.RemoveTag(tag, int.MaxValue);
    }
}