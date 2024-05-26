﻿namespace SameProductEstimator.Kosik;

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
		if (values?.values is null)
			return null;

		int energetickaKJ = 0, energetickaKCAL = 0;
		decimal tuky = 0.0m, mastneKyseliny = 0.0m, sacharidy = 0.0m, cukry = 0.0m, bilkoviny = 0.0m, sul = 0.0m, vlaknina = 0.0m;

		foreach (NutritionalValue value in values.values)
		{
			switch (value.title)
			{
				case "Energetická hodnota":
					if (value.unit == "kJ" && int.TryParse(value.value, out int kJ))
						energetickaKJ = kJ;
					else if (value.unit == "kcal" && int.TryParse(value.value, out int kcal))
						energetickaKCAL = kcal;
					break;
				case "Tuky":
					tuky = decimal.TryParse(value.value, out decimal tmp) ? tmp : tuky;
					break;
				case "Z toho nasycené mastné kyseliny":
					mastneKyseliny = decimal.TryParse(value.value, out tmp) ? tmp : mastneKyseliny;
					break;
				case "Sacharidy":
					sacharidy = decimal.TryParse(value.value, out tmp) ? tmp : sacharidy;
					break;
				case "Z toho cukry":
					cukry = decimal.TryParse(value.value, out tmp) ? tmp : cukry;
					break;
				case "Bílkoviny":
					bilkoviny = decimal.TryParse(value.value, out tmp) ? tmp : bilkoviny;
					break;
				case "Sůl":
					sul = decimal.TryParse(value.value, out tmp) ? tmp : sul;
					break;
				case "Vláknina":
					vlaknina = decimal.TryParse(value.value, out tmp) ? tmp : vlaknina;
					break;
			}
		}

		return new NutritionalValues(energetickaKJ, energetickaKCAL,
									 tuky, mastneKyseliny, sacharidy, cukry,
									 bilkoviny, sul, vlaknina);
	}
}
