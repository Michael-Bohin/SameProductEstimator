namespace SameProductEstimator;

internal class ProductParserLogger
{
	const string logsPath = "./out/devLogs/";
	static string ParserLogName(Eshop eshop) => $"parsed{eshop}Products.txt";
	public static void Log(List<NormalizedProduct> products, Eshop eshop)
	{
		Directory.CreateDirectory(logsPath);
		using StreamWriter sw = new($"{logsPath}{ParserLogName(eshop)}");
		foreach (NormalizedProduct product in products)
			sw.WriteLine(product + "\n");
	}
}

