using JetBrains.Annotations;

namespace MyGrater;

public interface IBatchProcessor<TDataSource, TDataDest>
{
    Task ProcessBatch<TId, TRow>(
        IReadOnlyCollection<TId> idBatch,
        Delegates.Getter<TId, TRow, TDataSource> getter,
        Delegates.Inserter<TRow, TDataDest> inserter) ;
}

[PublicAPI]
public class MigrationProcessor<TDataSource, TDataDest> : IBatchProcessor<TDataSource, TDataDest>
{
    private readonly TDataSource _source;
    private readonly TDataDest _dest;

    public MigrationProcessor(TDataSource source, TDataDest dest)
    {
        _source = source;
        _dest = dest;
    }

    public async Task ProcessBatch<TId, TRow>(
        IReadOnlyCollection<TId> idBatch,
        Delegates.Getter<TId, TRow, TDataSource> getter,
        Delegates.Inserter<TRow, TDataDest> inserter)
    {
        var rows = await getter(_source, idBatch);
        await inserter(_dest, rows);
    }
}