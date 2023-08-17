using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdaptiveRecordKit;

public sealed class JsonDocument : Document<Car>
{
    private readonly IsoDateTimeConverter _converter;

    private JsonDocument(string path)
        : base(path)
    {
        _converter = new IsoDateTimeConverter
        {
            DateTimeFormat = "dd-MM-yyyy"
        };
    }

    public static JsonDocument Create(string path)
    {
        var document = new JsonDocument(path);
        document.ParseFile();
        return document;
    }

    protected override void ParseFile()
    {
        var fileText = File.ReadAllText(_path);
        var document = JsonConvert.DeserializeObject<DynamicDocument<Car>>(fileText, _converter);

        _listOfT.AddRange(document?.Elements ?? Enumerable.Empty<Car>());

        foreach (var car in _listOfT)
        {
            if (car.Price < 0)
            {
                throw new FormatException($"Price should be positive. Price: {car.Price}");
            }

            if (string.IsNullOrEmpty(car.BrandName))
            {
                throw new FormatException($"BrandName should have valid format. BrandName: {car.BrandName}");
            }
        }
    }

    public override void SaveFile()
    {
        var document = new DynamicDocument<Car>
        {
            Elements = _listOfT
        };

        var carsAsString = JsonConvert.SerializeObject(document, _converter);
        File.WriteAllText(_path, carsAsString);
    }
}