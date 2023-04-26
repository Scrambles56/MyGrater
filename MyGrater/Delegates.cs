namespace MyGrater;

public class Delegates
{
    public delegate Task<IReadOnlyCollection<TRow>> Getter<in TId, TRow, in TDataSource>(TDataSource database, IEnumerable<TId> ids);

    public delegate Task Inserter<in TRow, in TDataDest>(TDataDest destination, IReadOnlyCollection<TRow> rows);
}