﻿using SameProductEstimator.Kosik;

namespace SameProductEstimator.Rohlik;

internal class RohlikAdapter : Adapter<RohlikJsonProduct>
{
	protected override string GetNameOf() => nameof(RohlikAdapter);

	protected override string GetRelativeDataPath() => RuntimeConfig.rohlikZipesRelativePath;

	protected override Eshop GetEshopType() => Eshop.Rohlik;

	protected override bool AnyCriticalPropertyIsNull(RohlikJsonProduct product)
	{
		if (product is null || product?.name is null || product?.url is null || product?.price is null)
			return true;

		Price p = product.price;

		return p?.amount is null || p?.currency is null;
	}

	protected override NormalizedProduct UnsafeParseNormalizedProduct(RohlikJsonProduct rohlikProduct)
	{
		string name = rohlikProduct.name;
		string url = rohlikProduct.url;
		decimal price = rohlikProduct.price.amount;

		NormalizedProduct normalizedProduct = new(name, url, price, Eshop.Rohlik) {
			Producer = rohlikProduct?.brand,
			Description = rohlikProduct?.htmlDescription, // zde bude potreba vyzkum jakym regexpem prevest z html na porovnatelny text
			StorageConditions = null, // zde bude potreba vyzkum jakym regexpem vytahnout skladovaci podminky z htmlDescription, rohlik tuhle informaci nema v samostatnem fieldu 
			UnitType = ParseUnitType(rohlikProduct!),
			Pieces = 1,
			Weight = null,
			Volume = null,
			NutritionalValues = null // nutricni hodnoty je treba doimplementovat
		};

		return normalizedProduct;
	}

	private static int kusy, vaha, objem, other;

	private static UnitType? ParseUnitType(RohlikJsonProduct product)
	{
		if(product?.unit is null)
			return null!;

		if(product.unit == "kg")
			vaha++;

		else if(product.unit == "ks")
			kusy++;

		else if(product.unit == "l")
			objem++;

		else 
			other++;

		// WriteLine($"{product.unit} {kusy}	{vaha}	{objem}	{other}");

		if(product.unit == "kg") 
			return UnitType.Weight;

		if (product.unit == "ks")
			return UnitType.Pieces;

		if (product.unit == "kg")
			return UnitType.Volume;

		if (product.unit == "krabička")
			return UnitType.Krabicka;

		// unknown other unit type => default to pieces
		return UnitType.Ostatni;
	}
}