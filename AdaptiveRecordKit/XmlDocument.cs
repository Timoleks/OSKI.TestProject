using System.Globalization;
using System.Xml;

namespace AdaptiveRecordKit;

public sealed class XmlDocument : Document<Car>
{
    private XmlDocument(string path)
        : base(path)
    { }

    public static XmlDocument Create(string path)
    {
        var document = new XmlDocument(path);
        document.ParseFile();
        return document;
    }

    protected override void ParseFile()
    {
        var xmlDocument = new System.Xml.XmlDocument();
        xmlDocument.Load(_path);

        XmlNodeList carNodes = xmlDocument.SelectNodes("/Document/Car");

        foreach (XmlNode carNode in carNodes)
        {
            var dateString = carNode.SelectSingleNode("Date")?.InnerText;
            var brandName = carNode.SelectSingleNode("BrandName")?.InnerText;
            var priceString = carNode.SelectSingleNode("Price")?.InnerText;

            Console.WriteLine($"Date: {dateString}, BrandName: {brandName}, Price: {priceString}");

            var car = CreateCar(priceString, brandName, dateString);

            _listOfT.Add(car);
        }
    }

    private static Car CreateCar(string? priceString, string? brandName, string? dateString)
    {
        if (decimal.TryParse(priceString, out var price) is false || price < 0)
        {
            throw new FormatException($"Price should have valid format and should be positive. Price: {priceString}");
        }

        if (string.IsNullOrEmpty(brandName))
        {
            throw new FormatException($"BrandName should have valid format. BrandName: {brandName}");
        }

        var car = new Car
        {
            Date = DateTime.ParseExact(dateString!, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            BrandName = brandName,
            Price = price
        };

        return car;
    }

    public override void SaveFile()
    {
        var xmlDocument = new System.Xml.XmlDocument();
        var documentNode = xmlDocument.CreateElement("Document");
        xmlDocument.AppendChild(documentNode);

        foreach (var car in _listOfT)
        {
            var carNode = xmlDocument.CreateElement("Car");

            var dateNode = xmlDocument.CreateElement("Date");
            dateNode.InnerText = car.Date.ToString("dd-MM-yyyy");
            carNode.AppendChild(dateNode);

            var brandNode = xmlDocument.CreateElement("BrandName");
            brandNode.InnerText = car.BrandName;
            carNode.AppendChild(brandNode);

            var priceNode = xmlDocument.CreateElement("Price");
            priceNode.InnerText = car.Price.ToString();
            carNode.AppendChild(priceNode);

            documentNode.AppendChild(carNode);
        }

        xmlDocument.Save(_path);
    }
}