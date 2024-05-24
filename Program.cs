global using static System.Console;
using SameProductEstimator;
using SameProductEstimator.Kosik;

const string kosikProductDataRelativePath = "./../../../ProductParser/ScrapedEshopData/kosikProductDataIndented.json";

string json = FileLoader.LoadFileContents(kosikProductDataRelativePath);

KosikAdapter ka = new();

List<NormalizedProduct> kosikProducts = ka.GetNormalizedProducts(json);	

ProductParserLogger.Log(kosikProducts);	
