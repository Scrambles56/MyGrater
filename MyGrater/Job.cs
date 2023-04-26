using JetBrains.Annotations;

namespace MyGrater;

[PublicAPI]
public abstract class Job<TDataSource, TDataDest>
{
    private readonly IBatchProcessor<TDataSource, TDataDest> _processor;

    public Job(IBatchProcessor<TDataSource, TDataDest> processor)
    {
        _processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    protected async Task Process<TRow, TId, TBatchSize>(
        string taskName,
        string indexKey,
        TId startingId,
        TId endingId,
        TBatchSize batchSize,
        IBatchRetriever<TId, TBatchSize> batchRetriever,
        Delegates.Getter<TId, TRow, TDataSource> getter,
        Delegates.Inserter<TRow, TDataDest> inserter
    )
    {
        IReadOnlyCollection<TId> batch;
        do
        {
            batch = await batchRetriever.GetNextBatch(startingId, endingId, batchSize);
            await _processor.ProcessBatch(batch, getter, inserter);

        } while (batch.Any());
    }
}

public interface IBatchRetriever<TId, TBatchSize>
{
    Task<IReadOnlyCollection<TId>> GetNextBatch(TId startingId, TId endingId, TBatchSize batchSize);
}