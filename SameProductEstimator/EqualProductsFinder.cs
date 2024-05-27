
using System.Text;

namespace SameProductEstimator;

internal class EqualProductsFinder
{
	public readonly List<NormalizedProduct> KosikProducts;
	public readonly List<NormalizedProduct> RohlikProducts;
	public readonly List<NormalizedProduct> TescoProducts;
	private const string logginDirectory = "./out/equalProductsFinder/";

	public EqualProductsFinder(List<NormalizedProduct> kosikProducts, List<NormalizedProduct> rohlikProducts, List<NormalizedProduct> tescoProducts)
	{
		AssertAllProductsAreFromSameEshop(kosikProducts, Eshop.Kosik);
		AssertAllProductsAreFromSameEshop(rohlikProducts, Eshop.Rohlik);
		AssertAllProductsAreFromSameEshop(tescoProducts, Eshop.Tesco);
		KosikProducts = kosikProducts;
		RohlikProducts = rohlikProducts;
		TescoProducts = tescoProducts;

		WriteLine("Normalized products have been loaded to same product estimator.");

		Directory.CreateDirectory(logginDirectory);	
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

		SortedDictionary<int, int> equalCandidatesFrequencies = [];

		foreach(NormalizedProduct product in smallerEshop.Products)
			GenerateMostProbableEqualProductsOf(product, largerEshop, outRoot, equalCandidatesFrequencies);

		LogStatsOfCandidates(equalCandidatesFrequencies, smallerEshop, largerEshop);
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
	private static void GenerateMostProbableEqualProductsOf(NormalizedProduct product, EshopSubstrings largerEshop, string outRoot, SortedDictionary<int, int> equalCandidatesFrequencies)
	{
		HashSet<NormalizedProduct> equalCandidates = ListEqualCandidates(product.Name, largerEshop);

		if(!equalCandidatesFrequencies.TryAdd(equalCandidates.Count, 1))
			equalCandidatesFrequencies[equalCandidates.Count]++;

		var equalBySubstringRatio = SortCandidatesBySubstring(product, equalCandidates);
		var equalByCommonPrefixRatio = SortCandidatesByPrefix(product, equalCandidates);
		var equalByEditationDistance = SortCandidatesByEditationDistance(product, equalCandidates);

	}

	/// <summary>
	/// Method splits product name on whitespaces into string array.
	/// The hashset of equal candidates is than created by adding all product references that contain
	/// at least one same substring in their name. This information can be looked up in linear time thanks 
	/// to the substring dictionary in largerEshop class.
	/// 
	/// Only substrings with length of at least three characters are considered, since substrings with 
	/// one or two characters are more likely to connect semantically unrelated products.
	/// </summary>
	/// <param name="productName"></param>
	/// <param name="largerEshop"></param>
	/// <returns></returns>
	private static HashSet<NormalizedProduct> ListEqualCandidates(string productName, EshopSubstrings largerEshop)
	{
		string[] nameParts = productName.Split(' ');
		HashSet<NormalizedProduct> equalCandidates = [];

		foreach(string part in nameParts)
			if(part.Length > 2 && largerEshop.SubstringsToProducts.TryGetValue(part, out List<NormalizedProduct>? value))
				foreach(NormalizedProduct prodctWithSameSubstring  in value)
					equalCandidates.Add(prodctWithSameSubstring);

		return equalCandidates;
	}

	private static List<NormalizedProduct> SortCandidatesBySubstring(NormalizedProduct product, HashSet<NormalizedProduct> candidates)
	{
		/// work to do
		
		return new();
	}

	private static List<NormalizedProduct> SortCandidatesByPrefix(NormalizedProduct product, HashSet<NormalizedProduct> candidates)
	{
		/// work to do

		return new();
	}

	private static List<NormalizedProduct> SortCandidatesByEditationDistance(NormalizedProduct product, HashSet<NormalizedProduct> candidates)
	{
		/// work to do

		return new();
	}

	#region Logging 
	private static void LogStatsOfCandidates(SortedDictionary<int, int> equalCandidatesFrequencies, EshopSubstrings smallerEshop, EshopSubstrings largerEshop)
	{
		Eshop smallerName = smallerEshop.Products.First().Eshop;
		Eshop largerName = largerEshop.Products.First().Eshop;

		using StreamWriter sw = new($"{logginDirectory}candidatesStas{smallerName}_to_{largerName}.txt");
		sw.WriteLine($"Equal candidates frequencies of {smallerName} -> {largerName}");

		int products = 0, candidatesSum = 0;
		StringBuilder sb = new();
		sb.AppendLine("Format -- Equal candidates count : frequency");
		foreach (var kvp in equalCandidatesFrequencies)
		{
			sb.AppendLine($"{kvp.Key} : {kvp.Value}");
			products += kvp.Value;
			candidatesSum += kvp.Key * kvp.Value;
		}

		sw.WriteLine($"Products from smaller eshop: {products} should be equal to {smallerEshop.Products.Count}");
		sw.WriteLine($"Sum of all candidates: {candidatesSum}");
		sw.WriteLine($"Average candidates per product of smaller eshop: {(double)candidatesSum/products}\n\n");
		sw.WriteLine(sb.ToString());
	}
	#endregion
}

