using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using SerilogTimings.Extensions;
using Serilog.Formatting.Json;

namespace SameProductEstimator;

internal class RuntimeConfig
{
	public const string kosikProductDataRelativePath = "./../../../ProductParser/ScrapedEshopData/kosikProductDataIndented.json",

	rohlikZipesRelativePath = "./../../../ProductParser/ScrapedEshopData/rohlikProductData.zip",

	tescoProductDataRelativePath = "./../../../ProductParser/ScrapedEshopData/tescoProductData.json",

	zipExtractPath = "./out/decompressedFiles/";

	public const int LimitProcessedProducts = 500;

	public static ILogger Logger()
	{
		Serilog.ILogger logger = new LoggerConfiguration()      // .ReadFrom.Configuration(configuration)
			.MinimumLevel.Debug()
			.Enrich.FromLogContext()
			.Enrich.WithThreadId()
			.Enrich.WithProperty("Stop watch", "Default")
			.Destructure.ByTransforming<NormalizedProduct>(x => new {
				x.Name, x.Price, x.Eshop
			})
			.WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
			.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
			.WriteTo.File(new JsonFormatter(), "./logs/debug-logs.json", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
			.CreateLogger();

		Log.Logger = logger;

		Log.Fatal("Serilog usage examples:");

		int i = 5;
		string hello = "hi";
		Log.Error("Hello world! {i} {hello}", i, hello);

		var name = "Michael";
		var age = 1138;
		Log.Warning("{Name} just turned: {Age}", name, age);

		NormalizedProduct sampleProduct = new("Jogobela jahoda 150g", "/hezkeUrlJahody", 25.9m, Eshop.Kosik);
		Log.Information("Uzivatel si vyzadal produkt: {@Product}\n", sampleProduct);

		Log.CloseAndFlush();

		return logger;
	}

	/*/var configuration = new ConfigurationBuilder()
	.SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Optional based on setup
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();/**/
}
