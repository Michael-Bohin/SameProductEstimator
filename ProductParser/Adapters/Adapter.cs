using System.Text.Json;

namespace SameProductEstimator;

internal abstract class Adapter<T>
{
	protected abstract bool TryGetNormalized(T jsonProduct, out NormalizedProduct normalizedProduct);

	protected abstract string GetNameOf();

	protected abstract string GetRelativeDataPath();

	protected abstract Eshop GetEshopType();

	public List<NormalizedProduct> GetNormalizedProducts()
	{
		string json = FileHandler.LoadJsonFromPath(GetRelativeDataPath());
		return ParseNormalizedProducts(json);
	}

	public List<NormalizedProduct> GetNormalizedProducts(string zipExtractPath)
	{
		string json = FileHandler.LoadJsonFromPath(GetRelativeDataPath(), zipExtractPath);
		return ParseNormalizedProducts(json);
	}

	private List<NormalizedProduct> ParseNormalizedProducts(string json)
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

		WriteLine(GetNameOf());
		WriteLine(jsonProducts.Count);
		WriteLine($"Normalized products: {normalizedProducts.Count}");
		WriteLine($"Invalid: {invalidJsonProducts.Count}");
		WriteLine($"{normalizedProducts.Count} + {invalidJsonProducts.Count} = {normalizedProducts.Count + invalidJsonProducts.Count}\n");

		ProductParserLogger.Log(normalizedProducts, GetEshopType());

		return normalizedProducts;
	}
}
