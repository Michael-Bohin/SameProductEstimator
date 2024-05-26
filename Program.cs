global using static System.Console;
using SameProductEstimator;
using SameProductEstimator.Kosik;
using SameProductEstimator.Rohlik;
using SameProductEstimator.Tesco;

// Parsing Kosik products
KosikAdapter ka = new();
List<NormalizedProduct> kosikProducts = ka.GetNormalizedProducts();

// Parsing Rohlik products 
RohlikAdapter ra = new();
List<NormalizedProduct> rohlikProducts = ra.GetNormalizedProducts(RuntimeConfig.zipExtractPath);

// Parsing Tesco products 
TescoAdapter ta = new();
List<NormalizedProduct> tescoProducts = ta.GetNormalizedProducts();
