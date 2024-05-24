using System.Text.Json;

namespace SameProductEstimator.Kosik;

internal class KosikAdapter : Adapter
{
	public override List<NormalizedProduct> GetNormalizedProducts(string json)
	{
		List<KosikJsonProduct> jsonProducts = JsonSerializer.Deserialize<List<KosikJsonProduct>>(json)!;

		List<NormalizedProduct> normalizedProducts = [];

		List<KosikJsonProduct> invalidJsonProducts = [];

		foreach(KosikJsonProduct jsonProduct in jsonProducts)
		{
			if(TryGetNormalized(jsonProduct, out NormalizedProduct normalizedProduct))
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

	private static bool TryGetNormalized(KosikJsonProduct jsonProduct, out NormalizedProduct normalizedProduct)
	{
		string name = jsonProduct.product.name;

		string? producer = SafeRetrieveBrand(jsonProduct.product.detail);
		string? description = jsonProduct.product.detail.description[0].value; // tady bude potreba samostatny vyzkum jak se array chova co se semantiky tyce
		string? storageConditions = GetStorageConditions(jsonProduct.product.detail.supplierInfo);
		string? URL = $"www.kosik.cz{jsonProduct.product.url}";

		UnitType? unitType = SafeRetrieveUnitType(jsonProduct);

		if (producer is null || description is null || storageConditions is null || URL is null ||
			jsonProduct.product.price is null || unitType is null)
		{
			normalizedProduct = null!;
			return false;
		}

		decimal price = (decimal)jsonProduct.product.price;

		int? pieces = 1;
		decimal? weight = null;
		decimal? volume = null;

		NutritionalValues nutritionalValues = ToNormalized(jsonProduct.product.detail.nutritionalValues);

		normalizedProduct = new(
			name, producer, description, storageConditions, URL,
			Eshop.Kosik, price, (UnitType)unitType , pieces, weight, volume,
			nutritionalValues
			);

		return true;
	}

	private static string? SafeRetrieveBrand(Detail detail) => detail.brand is null ? null! : detail.brand.name;

	private static string GetStorageConditions(Supplierinfo[] list)
	{
		foreach(Supplierinfo info in list) 
			if(info.title == "Skladovací podmínky")
				return info.value;

		return "Not found";
	}

	static int counter = 0;
	private static UnitType? SafeRetrieveUnitType(KosikJsonProduct jsonProduct)
	{
		if(jsonProduct.product is null)	
			return null;

		if(jsonProduct.product.unit is null)
			return null;

		string unitDesc = jsonProduct.product.unit;

		if(unitDesc == "ks")
		{
			return UnitType.Pieces;
		}

		return null;
	}

	private static NutritionalValues ToNormalized(KosikNutritionalValues values)
	{

		int energetickaKJ = 0;
		int eneregetickaKCAL = 0;

		decimal tuky = 0.0m;
		decimal mastneKyseliny = 0.0m;
		decimal sacharidy = 0.0m;
		decimal cukry = 0.0m;
		decimal bilkoviny = 0.0m;
		decimal sul = 0.0m;
		decimal vlaknina = 0.0m;

		if(values is not null && values.values is not null)
		{
			foreach (NutritionalValue value in values.values)
			{
				if (value.title == "Energetická hodnota")
				{
					if (value.unit == "kJ")
					{
						if(int.TryParse(value.value, out int val))
						{
							energetickaKJ = val;
						}
						
					} else if (value.unit == "kcal")
					{
						if(int.TryParse(value.value, out int val))
						{
							eneregetickaKCAL = int.Parse(value.value);
						}
					}
				} else if (value.title == "Tuky")
				{
					tuky = decimal.Parse(value.value);
				} else if (value.title == "Z toho nasycené mastné kyseliny")
				{
					mastneKyseliny = decimal.Parse(value.value);
				} else if (value.title == "Sacharidy")
				{
					sacharidy = decimal.Parse(value.value);
				} else if (value.title == "Z toho cukry")
				{
					cukry = decimal.Parse(value.value);
				} else if (value.title == "Bílkoviny")
				{
					bilkoviny = decimal.Parse(value.value);
				} else if (value.title == "Sůl")
				{
					sul = decimal.Parse(value.value);
				} else if (value.title == "Vláknina")
				{
					vlaknina = decimal.Parse(value.value);
				}
			}
		}

		NutritionalValues normalizedValues = new(
			energetickaKJ, eneregetickaKCAL,
			tuky, mastneKyseliny, 
			sacharidy, cukry, 
			bilkoviny, sul, 
			vlaknina
			);

		return normalizedValues;
	}
}

