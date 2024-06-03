global using static System.Console;
using SameProductEstimator;
using SameProductEstimator.Kosik;
using SameProductEstimator.Rohlik;
using SameProductEstimator.Tesco;
using Serilog;
using SerilogTimings.Extensions;

ILogger logger = RuntimeConfig.Logger();
Log.Logger = logger;

var timedOpProgram = logger.BeginOperation("Equal products finder stopwatch.");
var timedAdapters = logger.BeginOperation("Adapters stopwatch.");

// Parsing Kosik products
KosikAdapter ka = new();
List<NormalizedProduct> kosikProducts = ka.GetNormalizedProducts();

// Parsing Rohlik products 
RohlikAdapter ra = new();
List<NormalizedProduct> rohlikProducts = ra.GetNormalizedProducts(RuntimeConfig.zipExtractPath);

// Parsing Tesco products
TescoAdapter ta = new();
List<NormalizedProduct> tescoProducts = ta.GetNormalizedProducts();

timedAdapters.Abandon();
var timedEPF = logger.BeginOperation("Equal products finder stopwatch.");

EqualProductsFinder epf = new(kosikProducts, rohlikProducts, tescoProducts, logger);
await epf.SortProbableEqualProductsAsync();

timedEPF.Abandon();
timedOpProgram.Abandon();

