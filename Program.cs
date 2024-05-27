global using static System.Console;
using SameProductEstimator;
using SameProductEstimator.Kosik;
using SameProductEstimator.Rohlik;
using SameProductEstimator.Tesco;
using System.Diagnostics;

Stopwatch sw = Stopwatch.StartNew();

// Parsing Kosik products
KosikAdapter ka = new();
List<NormalizedProduct> kosikProducts = ka.GetNormalizedProducts();

// Parsing Rohlik products 
RohlikAdapter ra = new();
List<NormalizedProduct> rohlikProducts = ra.GetNormalizedProducts(RuntimeConfig.zipExtractPath);

// Parsing Tesco products
TescoAdapter ta = new();
List<NormalizedProduct> tescoProducts = ta.GetNormalizedProducts();

EqualProductsFinder epf = new(kosikProducts, rohlikProducts, tescoProducts);
await epf.SortProbableEqualProductsAsync();

sw.Stop();

WriteLine($"Program ran for {sw.ElapsedMilliseconds / 1_000} seconds and {sw.ElapsedMilliseconds % 1_000} ms.");
