using System.Text.Json;

namespace SameProductEstimator;

internal abstract class Adapter<T>
{
	protected abstract bool TryGetNormalized(T jsonProduct, out NormalizedProduct normalizedProduct);

	public List<NormalizedProduct> GetNormalizedProducts(string json)
	{
		List<T> jsonProducts = JsonSerializer.Deserialize<List<T>>(json)!;

		List<NormalizedProduct> normalizedProducts = [];

		List<T> invalidJsonProducts = [];

		foreach (T jsonProduct in jsonProducts)
		{
			if (TryGetNormalized(jsonProduct, out NormalizedProduct normalizedProduct))
			{
				normalizedProducts.Add(normalizedProduct);
			} else
			{
				invalidJsonProducts.Add(jsonProduct);
			}
		}

		WriteLine(jsonProducts.Count);
		WriteLine($"Normalized products: {normalizedProducts.Count}");
		WriteLine($"Invalid: {invalidJsonProducts.Count}");
		WriteLine($"{normalizedProducts.Count} + {invalidJsonProducts.Count} = {normalizedProducts.Count + invalidJsonProducts.Count}");

		return normalizedProducts;
	}
}
