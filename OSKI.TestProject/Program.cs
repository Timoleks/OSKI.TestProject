using AdaptiveRecordKit;

// Examples of using the library

// JSON
var documentJson = JsonDocument.Create("path\\to\\jsonfile.json");

var car = new Car
{
    Date = DateTime.Now,
    BrandName = "somenamejsonUpdated",
    Price = 300
};

var cars = documentJson.GetAll();

var expensiveCar = documentJson.Get(car => car.Price == 4000);

documentJson.Add(car); //adding new car
documentJson.TryUpdate(car => car.BrandName == "somenamejson", car); // updating car where BrandName is somenamejson
documentJson.DeleteAll(car => car.BrandName == "somenamejson"); // deleting all cars where BrandName is somenamejson
documentJson.SaveFile(); // saving changes to a file


// XML
var documentXml = XmlDocument.Create("path\\to\\xmlfile.xml");

documentXml.Add(car);
documentXml.TryUpdate(car => car.BrandName == "somenamejson", car);

documentXml.SaveFile();