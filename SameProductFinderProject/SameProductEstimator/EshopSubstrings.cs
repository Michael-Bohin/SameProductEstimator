using Serilog;

namespace SameProductEstimator;

internal class EshopSubstrings
{
	public List<NormalizedProduct> Products;
	public readonly Dictionary<string, List<NormalizedProduct>> SubstringsToProducts = [];

	public EshopSubstrings(List<NormalizedProduct> products)  
	{
		Products = products;
		foreach(var product in products) 
			AddSubstringsToDictionary(product);

		ConsoleLogDictionarySizeStats();
	}

	private void AddSubstringsToDictionary(NormalizedProduct product)
	{
		foreach(string part in product.InferredData.lowerCaseNameParts)
			if(part.Length > 2)
				AddPartToDictionary(part, product);
	}

	private void AddPartToDictionary(string part, NormalizedProduct product)
	{
		if(SubstringsToProducts.ContainsKey(part))
		{
			SubstringsToProducts[part].Add(product);
		} else
		{
			SubstringsToProducts[part] = new() { product }; 
		}
	}

	private void ConsoleLogDictionarySizeStats()
	{
		Log.Information("Constructed dictionary of product names substrings to list of product references of eshop {productsCount}", Products[0].Eshop);
		Log.Information("Dictionary contains {substringCount} keys.", SubstringsToProducts.Count);
		
		int counter = 0;
        foreach (List<NormalizedProduct> productsWithSameSubstrings in SubstringsToProducts.Values)
		{
			counter += productsWithSameSubstrings.Count;    
        }

		Log.Information("Sum of all product references {Counter}", counter);
		Log.Information("Average references per one substring {avgRefsPerSubstring}", $"{(double)counter / SubstringsToProducts.Count:f2}");
		Log.Information("Average number of ws split substrings per product {avgSplitSubstringsPerProduct}\n", $"{(double)SubstringsToProducts.Count / Products.Count:f2}");
    }
}
