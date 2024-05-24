namespace SameProductEstimator;

internal class ProductParserLogger
{
	const string logsPath = "./out/devLogs/";
	const string kosikParserLogName = "parsedKosikProducts.txt";
	public static void Log(List<NormalizedProduct> products)
	{
		Directory.CreateDirectory(logsPath);
		using StreamWriter sw = new($"{logsPath}{kosikParserLogName}");
		foreach (NormalizedProduct product in products)
			sw.WriteLine(product + "\n");
	}
}

