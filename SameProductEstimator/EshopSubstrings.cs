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
			SubstringsToProducts[part] = new() { product };
		} else
		{
			SubstringsToProducts[part].Add(product);
		}
	}
}
