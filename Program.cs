global using static System.Console;
using SameProductEstimator;
using SameProductEstimator.Kosik;
using SameProductEstimator.Rohlik;
using SameProductEstimator.Tesco;
using System.Diagnostics;
using Serilog;
using Serilog.Settings.Configuration;
using Serilog.Formatting.Json;
using Serilog.Core;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SerilogTimings;
using SerilogTimings.Extensions;

/*/var configuration = new ConfigurationBuilder()
	.SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Optional based on setup
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();/**/

Serilog.ILogger logger = new LoggerConfiguration()
	// .ReadFrom.Configuration(configuration)
	.MinimumLevel.Debug()
	.Destructure.ByTransforming<NormalizedProduct>(x => new {
		x.Name,	x.Price, x.Eshop
	})
	.WriteTo.Console(theme: AnsiConsoleTheme.Code)
	.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
	
	.CreateLogger();

Log.Logger = logger;


var timedOp = logger.BeginOperation("Initiating intro to serilog section");
Log.Error("Hello, Serilog!");
Log.Debug("Debug log reporting");

int i = 5;
string hello = "hi";
Log.Information("Hello world! {i} {hello}", i, hello);

var name = "Michael";
var age = 33;

Log.Information("{Name} just turned: {Age}", name, age);

NormalizedProduct sampleProduct = new("Jogobela jahoda 150g", "/hezkeUrlJahody", 25.9m, Eshop.Kosik);
Log.Information("Uzivatel si vyzadal produkt:\n{@Product}", sampleProduct );

await Task.Delay(138);

Random rand = new();
if(rand.Next(2) == 0)
{
	timedOp.Complete();
} else
{
	timedOp.Abandon();
}


Log.CloseAndFlush();

return;

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
