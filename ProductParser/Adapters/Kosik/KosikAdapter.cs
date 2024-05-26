namespace SameProductEstimator.Kosik;

internal class KosikAdapter : Adapter<KosikJsonProduct>
{
	protected override bool TryGetNormalized(KosikJsonProduct jsonProduct, out NormalizedProduct normalizedProduct)
	{		
		string? name = jsonProduct?.product?.name;
		string? URL = SafeRetrieveURL(jsonProduct!);

		if (name is null || URL is null || jsonProduct is null || jsonProduct?.product?.price is null)
		{
			normalizedProduct = null!;
			return false;
		}

		decimal price = (decimal)jsonProduct.product.price;


		normalizedProduct = new(name, URL, price, Eshop.Kosik) {
			Producer = jsonProduct?.product?.detail?.brand?.name,
			Description = jsonProduct?.product?.detail?.description?[0]?.value, // tady bude potreba samostatny vyzkum jak se array chova co se semantiky tyce
			StorageConditions = GetStorageConditions(jsonProduct!),
			UnitType = SafeRetrieveUnitType(jsonProduct!),
			Pieces = 1,
			Weight = null,
			Volume = null,
			NutritionalValues = ToNormalized(jsonProduct?.product?.detail?.nutritionalValues)
		};
		
		return true;
	}

	private static string? SafeRetrieveURL(KosikJsonProduct product)
	{
		string? url = product?.product?.url;

		return url is null ? null : $"www.kosik.cz{url}";
	}

	private static string? GetStorageConditions(KosikJsonProduct product)
	{
		Supplierinfo[]? list = product?.product?.detail?.supplierInfo;
		if (list is null)
			return null;

		foreach (Supplierinfo info in list) 
			if(info?.title == "Skladovací podmínky")
				return info?.value;

		return null;
	}

	private static UnitType? SafeRetrieveUnitType(KosikJsonProduct jsonProduct)
	{
		string? unitDesc = jsonProduct?.product?.unit;

		if (unitDesc is null)	
			return null;

		if(unitDesc == "ks")
			return UnitType.Pieces;

		// further study of the entire range of unit types in Kosik is needed!
		return null;
	}

	private static NutritionalValues? ToNormalized(KosikNutritionalValues? values)
	{
		if(values is null)
			return null;

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

