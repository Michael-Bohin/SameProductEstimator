global using static System.Console;
using SameProductEstimator;
using SameProductEstimator.Kosik;
using SameProductEstimator.Rohlik;
using SameProductEstimator.Tesco;

const string kosikProductDataRelativePath = "./../../../ProductParser/ScrapedEshopData/kosikProductDataIndented.json";

const string rohlikZipesRelativePath = "./../../../ProductParser/ScrapedEshopData/rohlikProductData.zip";

const string tescoProductDataRelativePath = "./../../../ProductParser/ScrapedEshopData/tescoProductData.json";

const string zipExtractPath = "./out/decompressedFiles/";

// Parsing Kosik products
var json = FileHandler.LoadJsonFromPath(kosikProductDataRelativePath);
KosikAdapter ka = new();
List<NormalizedProduct> kosikProducts = ka.GetNormalizedProducts(json);
ProductParserLogger.Log(kosikProducts, Eshop.Kosik);

// Parsing Rohlik products
json = FileHandler.LoadJsonFromPath(rohlikZipesRelativePath, zipExtractPath);
RohlikAdapter ra = new();
List<NormalizedProduct> rohlikProducts = ra.GetNormalizedProducts(json);
ProductParserLogger.Log(rohlikProducts, Eshop.Rohlik);

// Parsing Tesco products
json = FileHandler.LoadJsonFromPath(tescoProductDataRelativePath);
TescoAdapter ta = new();
List<NormalizedProduct> tescoProducts = ta.GetNormalizedProducts(json);
ProductParserLogger.Log(tescoProducts, Eshop.Tesco);
