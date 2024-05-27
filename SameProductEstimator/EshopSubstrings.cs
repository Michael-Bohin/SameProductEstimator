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
		string[] nameParts = product.Name.Split(' ');

		foreach(string part in nameParts)
			if(part.Length > 1)
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
		WriteLine($"Constructed dictionary of product names substrings to list of product references of eshop {Products[0].Eshop}");
		WriteLine($"Dictionary contains {SubstringsToProducts.Count} keys.");
		
		int counter = 0;
        foreach (List<NormalizedProduct> productsWithSameSubstrings in SubstringsToProducts.Values)
		{
			counter += productsWithSameSubstrings.Count;    
        }

		WriteLine($"Sum of all product references {counter}");
		WriteLine($"Average references per one substring {(double)counter/SubstringsToProducts.Count:f2}");
		WriteLine($"Average number of ws split substrings per product {(double)counter/Products.Count:f2}\n");
    }
}
