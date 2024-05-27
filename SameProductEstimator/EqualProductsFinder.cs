
namespace SameProductEstimator;

internal class EqualProductsFinder
{
	public readonly List<NormalizedProduct> KosikProducts;
	public readonly List<NormalizedProduct> RohlikProducts;
	public readonly List<NormalizedProduct> TescoProducts;

	public EqualProductsFinder(List<NormalizedProduct> kosikProducts, List<NormalizedProduct> rohlikProducts, List<NormalizedProduct> tescoProducts)
	{
		AssertAllProductsAreFromSameEshop(kosikProducts, Eshop.Kosik);
		AssertAllProductsAreFromSameEshop(rohlikProducts, Eshop.Rohlik);
		AssertAllProductsAreFromSameEshop(tescoProducts, Eshop.Tesco);
		KosikProducts = kosikProducts;
		RohlikProducts = rohlikProducts;
		TescoProducts = tescoProducts;

		WriteLine("Normalized products have been loaded to same product estimator.");
	}

	private static void AssertAllProductsAreFromSameEshop(List<NormalizedProduct> products, Eshop eshop)
	{
		foreach(var product in products)
			if(product.Eshop != eshop)
				throw new ArgumentException($"Product is expected to be normalized from eshop {eshop}, but instead it is from {product.Eshop}.");
	}

	public void SortProbableEqualProducts()
	{

	}
}

