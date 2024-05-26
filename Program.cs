global using static System.Console;
using SameProductEstimator;
using SameProductEstimator.Kosik;
using SameProductEstimator.Rohlik;
using SameProductEstimator.Tesco;

// Parsing Kosik products
var json = FileHandler.LoadJsonFromPath(RuntimeConfig.kosikProductDataRelativePath);
KosikAdapter ka = new();
List<NormalizedProduct> kosikProducts = ka.GetNormalizedProducts(json);
ProductParserLogger.Log(kosikProducts, Eshop.Kosik);

// Parsing Rohlik products
json = FileHandler.LoadJsonFromPath(RuntimeConfig.rohlikZipesRelativePath, RuntimeConfig.zipExtractPath);
RohlikAdapter ra = new();
List<NormalizedProduct> rohlikProducts = ra.GetNormalizedProducts(json);
ProductParserLogger.Log(rohlikProducts, Eshop.Rohlik);

// Parsing Tesco products
json = FileHandler.LoadJsonFromPath(RuntimeConfig.tescoProductDataRelativePath);
TescoAdapter ta = new();
List<NormalizedProduct> tescoProducts = ta.GetNormalizedProducts(json);
ProductParserLogger.Log(tescoProducts, Eshop.Tesco);
