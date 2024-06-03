namespace SameProductEstimator.Tesco;

internal class TescoAdapter : Adapter<TescoJsonProduct>
{
	protected override string GetNameOf() => nameof(TescoAdapter);

	protected override string GetRelativeDataPath() => RuntimeConfig.tescoProductDataRelativePath;

	protected override Eshop GetEshopType() => Eshop.Tesco;

	protected override bool AnyCriticalPropertyIsNull(TescoJsonProduct tescoProduct)
	{
		if (tescoProduct is null || tescoProduct?.product is null)
			return true;

		Product p = tescoProduct.product;

		return p?.title is null || p?.id is null || p?.price is null;
	}

	protected override NormalizedProduct UnsafeParseNormalizedProduct(TescoJsonProduct tescoProduct) 
	{
		Product p = tescoProduct.product;

		string name = p.title;
		string url = $"https://nakup.itesco.cz/groceries/cs-CZ/products/{p.id}";
		decimal price = p.price;

		NormalizedProduct normalizedProduct = new(name, url, price, Eshop.Tesco) {
			Producer = null,  // information is absent in the webscraped data
			Description = p?.shortDescription,
			StorageConditions = null, // information is absent in the webscraped data
			UnitType = ParseUnitType(tescoProduct!),
			Pieces = 1,
			Weight = null,
			Volume = null,
			NutritionalValues = null // nutricni hodnoty je treba doimplementovat
		};

		return normalizedProduct;
	}

	private UnitType? ParseUnitType(TescoJsonProduct product)
	{
		if(product.product.unitOfMeasure is null)
			return null;

		string unit = product.product.unitOfMeasure;

		if(unit == "kg")
		{
			return UnitType.Weight;
		}

		return UnitType.Ostatni;
	}

}
