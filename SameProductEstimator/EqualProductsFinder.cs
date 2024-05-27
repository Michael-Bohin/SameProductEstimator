
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

	/// <summary>
	/// 1. Foreach eshop creates dictionaries substrings in names to list of references of products
	/// 2. Foreach eshop pair
	/// 3.		For eshop e with less products 
	/// 4.			For each product in eshop e
	/// 5.				Creates & saves sorted list of most probable equal products
	/// </summary>
	public void SortProbableEqualProducts()
	{
		EshopSubstrings kosikDict = new(KosikProducts);
		EshopSubstrings rohlikDict = new(RohlikProducts);
		EshopSubstrings tescoDict = new(TescoProducts);

		GenerateMostProbableEqualProducts(kosikDict, rohlikDict);
		GenerateMostProbableEqualProducts(kosikDict, tescoDict);
		GenerateMostProbableEqualProducts(rohlikDict, tescoDict);
	}

	/// <summary>
	/// Pick eshop e with lower number of products.
	/// For each product from eshop e generate list of most probable equal products.
	/// </summary>
	/// <param name="eshopA"></param>
	/// <param name="eshopB"></param>
	private static void GenerateMostProbableEqualProducts(EshopSubstrings eshopA, EshopSubstrings eshopB)
	{
		EshopSubstrings smallerEshop = eshopA.Products.Count < eshopB.Products.Count ? eshopA : eshopB;
		EshopSubstrings largerEshop = eshopA.Products.Count >= eshopB.Products.Count ? eshopA : eshopB;
		string outRoot = CreateLogginDirectory(smallerEshop, largerEshop);

		foreach(NormalizedProduct product in smallerEshop.Products)
			GenerateMostProbableEqualProductsOf(product, largerEshop, outRoot);
	}

	private static string CreateLogginDirectory(EshopSubstrings smallerEshop, EshopSubstrings largerEshop)
	{
		string directoryRoot = $"./out/{smallerEshop.Products[0].Eshop}To{largerEshop.Products[0].Eshop}ProbableEqualProducts/";
		Directory.CreateDirectory(directoryRoot);
		return directoryRoot;
	}

	/// <summary>
	/// 1. Create a preliminary list of possible equal products by creating a list of all products that have 
	///		at least one same substring in their name.
	///		
	/// 2. Create three lists of probable equal products sorted by probability based on 3 different factors:
	///		a. Ratio of equal substrings 
	///		b. Same longest prefix
	///		c. Editacni vzdalenost nazvu
	///		
	/// 3. Cap all lists to at most 38 results and save results into standardized txt files, which 
	///		will serve as input for program where humans will manually assign edges of equality among same products.
	/// </summary>
	/// <param name="product"></param>
	/// <param name="largerEshop"></param>
	/// <param name="outRoot"></param>
	private static void GenerateMostProbableEqualProductsOf(NormalizedProduct product, EshopSubstrings largerEshop, string outRoot)
	{
		List<NormalizedProduct> equalCandidates = ListEqualCandidates(product.Name, largerEshop);
		
		var equalBySubstringRatio = SortCandidatesBySubstring(product, equalCandidates);
		var equalByCommonPrefixRatio = SortCandidatesByPrefix(product, equalCandidates);
		var equalByEditationDistance = SortCandidatesByEditationDistance(product, equalCandidates);

	}

	private static List<NormalizedProduct> ListEqualCandidates(string productName, EshopSubstrings largerEshop)
	{
		// work to do 

		return new();
	}

	private static List<NormalizedProduct> SortCandidatesBySubstring(NormalizedProduct product,  List<NormalizedProduct> candidates)
	{
		/// work to do
		
		return new();
	}

	private static List<NormalizedProduct> SortCandidatesByPrefix(NormalizedProduct product, List<NormalizedProduct> candidates)
	{
		/// work to do

		return new();
	}

	private static List<NormalizedProduct> SortCandidatesByEditationDistance(NormalizedProduct product, List<NormalizedProduct> candidates)
	{
		/// work to do

		return new();
	}
}

