global using static System.Console;

using SameProductEstimator;

const string kosikProductDataRelativePath = "./../../../ProductParser/ScrapedEshopData/kosikProductData.json";

string json = FileLoader.LoadFileContents(kosikProductDataRelativePath);

KosikAdapter ka = new();

List<NormalizedProduct> kosikProducts = ka.GetNormalizedProducts(json);	

int counter = 0;
foreach(var product in kosikProducts)
{
    WriteLine($"Next product {++counter}:");
    WriteLine($"{product}\n");
}

if(kosikProducts.Count == 0)
    WriteLine("Product list is empty.");
