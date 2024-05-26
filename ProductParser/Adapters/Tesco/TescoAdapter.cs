using SameProductEstimator.Rohlik;

namespace SameProductEstimator.Tesco;

internal class TescoAdapter : Adapter<TescoJsonProduct>
{
	protected override string GetNameOf() => nameof(TescoAdapter);

	protected override string GetRelativeDataPath() => RuntimeConfig.tescoProductDataRelativePath;

	protected override Eshop GetEshopType() => Eshop.Tesco;

	protected override bool TryGetNormalized(TescoJsonProduct tescoProduct, out NormalizedProduct normalizedProduct)
	{
		if (AnyCriticalPropertyIsNull(tescoProduct))
		{
			normalizedProduct = null!;
			return false;
		}

		Product p = tescoProduct.product;
		
		string name = p.title;
		string url = $"https://nakup.itesco.cz/groceries/cs-CZ/products/{p.id}";
		decimal price = p.price;

		normalizedProduct = new(name, url, price, Eshop.Tesco) {
			Producer = null,  // information is absent in the webscraped data
			Description = p?.shortDescription, 
			StorageConditions = null, // information is absent in the webscraped data
			UnitType = ParseUnitType(tescoProduct!),
			Pieces = 1,
			Weight = null,
			Volume = null,
			NutritionalValues = null // nutricni hodnoty je treba doimplementovat
		};

		return true;
	}

	private static bool AnyCriticalPropertyIsNull(TescoJsonProduct product)
	{
		if (product is null || product?.product is null)
			return true;

		Product p = product.product;

		return p?.title is null || p?.id is null || p?.price is null;
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
