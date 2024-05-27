using System.Text.Json;

namespace SameProductEstimator;

internal abstract class Adapter<T>
{
	protected abstract bool AnyCriticalPropertyIsNull(T product);

	protected abstract NormalizedProduct UnsafeParseNormalizedProduct(T product);

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

	/// <summary>
	/// Invalid products are not yet treated, since the product data are now 
	/// static three files, which have been proofed to produce only valid products now
	/// however, as we will be moving to production and run this worker automatically
	/// we will need to implement object that will take care of invalid results 
	/// </summary>
	private List<NormalizedProduct> ParseNormalizedProducts(string json)
	{
		List<T> jsonProducts = DeserializeProducts(json);
		var (normalizedProducts, invalidProducts) = ProcessProducts(jsonProducts);
		
		LogProductCounts(normalizedProducts, jsonProducts.Count, invalidProducts.Count);
		return normalizedProducts;
	}

	private static List<T> DeserializeProducts(string json) => JsonSerializer.Deserialize<List<T>>(json)!;

	private (List<NormalizedProduct>, List<T>) ProcessProducts(List<T> products)
	{
		List<NormalizedProduct> normalizedProducts = [];
		List<T> invalidProducts = [];

		foreach (T product in products)
		{
			if (TryGetNormalized(product, out NormalizedProduct? normalizedProduct))
			{
				normalizedProducts.Add(normalizedProduct!);
			} else
			{
				invalidProducts.Add(product);
			}
		}

		return (normalizedProducts, invalidProducts);
	}

	private void LogProductCounts(List<NormalizedProduct> normalizedProducts, int total, int invalidCount)
	{
		ProductParserLogger.Log(normalizedProducts, GetEshopType());

		WriteLine(GetNameOf());
		WriteLine(total);
		WriteLine($"Normalized products: {normalizedProducts.Count}");
		WriteLine($"Invalid products: {invalidCount}");
		WriteLine($"{normalizedProducts.Count} + {invalidCount} = {total}\n");
	}

	protected bool TryGetNormalized(T product, out NormalizedProduct normalizedProduct)
	{
		if (AnyCriticalPropertyIsNull(product))
		{
			normalizedProduct = null!;
			return false;
		}

		normalizedProduct = UnsafeParseNormalizedProduct(product);

		return true;
	}
}
