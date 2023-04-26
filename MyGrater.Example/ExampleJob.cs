using MyGrater.Example.datasources;

namespace MyGrater.Example;

public class ExampleJob : Job<CsvDataSource, JsonDataDest>
{
    public ExampleJob() : 
        base(new MigrationProcessor<CsvDataSource,JsonDataDest>(new CsvDataSource("./data/src-data.csv"), new JsonDataDest("./data/dest-data.json")))
    {
    }

    public Task ProcessRecords() 
        => Process(
            "default", 
            "blah", 
            0, 
            40, 
            10, 
            new Batcher(),
            ExampleDelegates.GetExampleRow, 
            ExampleDelegates.WriteExampleRows);
}

public class Batcher : IBatchRetriever<int, int>
{

    private int _counter;
    
    public Task<IReadOnlyCollection<int>> GetNextBatch(int startingId, int endingId, int batchSize)
    {
        var currentBatch = _counter++;
        var start = startingId + currentBatch * batchSize;
        var count = Math.Min(endingId - start, batchSize);

        var batch = (count >= 0
            ? Enumerable.Range(start, count)
            : Enumerable.Empty<int>()).ToList();
        
        return Task.FromResult<IReadOnlyCollection<int>>(batch);
    }
}