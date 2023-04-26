using System.Text.Json;
using MyGrater.Example.rows;

namespace MyGrater.Example.datasources;

public class JsonDataDest
{
    private readonly string _fileName;

    public JsonDataDest(string fileName)
    {
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
    }

    public async Task WriteExampleRows(IReadOnlyCollection<ExampleRow> rows)
    {
        var result = new List<ExampleRow>();
        if (File.Exists(_fileName))
        {
            await using (var streamReader = File.OpenRead(_fileName))
            {
                await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<ExampleRow>(streamReader))
                {
                    if (item != null)
                    {
                        result.Add(item);
                    }
                }


            }
        }
        
        result.AddRange(rows);

        await File.WriteAllTextAsync(_fileName, JsonSerializer.Serialize(result, new JsonSerializerOptions{ WriteIndented = true}));
    }
}