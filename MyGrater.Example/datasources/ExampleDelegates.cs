using MyGrater.Example.rows;

namespace MyGrater.Example.datasources;

public static class ExampleDelegates
{
    public static readonly Delegates.Getter<int, ExampleRow, CsvDataSource> GetExampleRow =
        (database, ids) => database.GetExampleRows(ids);

    public static readonly Delegates.Inserter<ExampleRow, JsonDataDest> WriteExampleRows =
        (destination, rows) => destination.WriteExampleRows(rows);
}