using System.Globalization;
using CsvHelper;
using MyGrater.Example.rows;

namespace MyGrater.Example.datasources;

public class CsvDataSource : IDataSource
{
    private List<ExampleRow>? _records;

    public CsvDataSource(string fileName)
    {
        using var streamReader = new StreamReader(fileName);
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        _records = csvReader.GetRecords<ExampleRow>()?.ToList();
    }

    public async Task<IReadOnlyCollection<ExampleRow>> GetExampleRows(IEnumerable<int> ids)
    {
        var list = new List<ExampleRow>();

        if (_records is null)
        {
            return list;
        }
        
        foreach (var id in ids)
        {
            var row = _records.ElementAtOrDefault(id);
            if (row is not null)
            {
                list.Add(row);
            }
        }

        return list;
    }
}