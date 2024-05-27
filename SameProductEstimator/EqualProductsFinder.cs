
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SameProductEstimator;

internal partial class EqualProductsFinder
{
	public readonly List<NormalizedProduct> KosikProducts;
	public readonly List<NormalizedProduct> RohlikProducts;
	public readonly List<NormalizedProduct> TescoProducts;
	private const string logginDirectory = "./out/equalProductsFinder/";
	private const string resultDirectory = "./out/equalProductsFinder/results/";

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
		Directory.CreateDirectory(resultDirectory);
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
	public async Task SortProbableEqualProductsAsync()
	{
		EshopSubstrings kosikDict = new(KosikProducts);
		EshopSubstrings rohlikDict = new(RohlikProducts);
		EshopSubstrings tescoDict = new(TescoProducts);

		Task task1 = Task.Run(() => GenerateMostProbableEqualProducts(kosikDict, rohlikDict));
		Task task2 = Task.Run(() => GenerateMostProbableEqualProducts(kosikDict, tescoDict));
		Task task3 = Task.Run(() => GenerateMostProbableEqualProducts(rohlikDict, tescoDict));

		await Task.WhenAll(task1, task2, task3);
	}

	/// <summary>
	/// Pick eshop e with lower number of products.
	/// For each product from eshop e generate list of most probable equal products.
	/// Foreach each product from smaller eshop sort the candidates by differet measures.
	///		a. Ratio of equal substrings 
	///		b. Same longest prefix
	///		c. Longest common subsequence
	///		d. Editacni vzdalenost nazvu
	/// </summary>
	/// <param name="eshopA"></param>
	/// <param name="eshopB"></param>
	private static void GenerateMostProbableEqualProducts(EshopSubstrings eshopA, EshopSubstrings eshopB)
	{
		EshopSubstrings smallerEshop = eshopA.Products.Count < eshopB.Products.Count ? eshopA : eshopB;
		EshopSubstrings largerEshop = eshopA.Products.Count >= eshopB.Products.Count ? eshopA : eshopB;
		CreateLogginDirectory(smallerEshop, largerEshop);

		var equalCandidatesOfProducts = FindEqualCandidatesOfProducts(smallerEshop, largerEshop);

		foreach (var (Product, Candidates) in equalCandidatesOfProducts)
		{
			SortCandidatesBySubstring(Product, Candidates, largerEshop);
			SortCandidatesByPrefix(Product, Candidates, largerEshop);
			SortCandidatesByLongestCommonSubsequence(Product, Candidates, largerEshop);

			// var equalByEditationDistance = SortCandidatesByEditationDistance(product, equalCandidates);
		}
	}

	private static void CreateLogginDirectory(EshopSubstrings smallerEshop, EshopSubstrings largerEshop)
	{
		string directoryRoot = $"./out/{smallerEshop.Products[0].Eshop}To{largerEshop.Products[0].Eshop}ProbableEqualProducts/";
		Directory.CreateDirectory(directoryRoot);
	}

	/// <summary>
	/// Create a preliminary list of possible equal products by creating a list of all products that have 
	///	at least one same substring in their name.
	///		
	/// </summary>
	/// <param name="product"></param>
	/// <param name="largerEshop"></param>
	/// <param name="outRoot"></param>
	private static List<(NormalizedProduct Product, HashSet<NormalizedProduct> EqualCandidates)> FindEqualCandidatesOfProducts(EshopSubstrings smallerEshop, EshopSubstrings largerEshop)
	{
		SortedDictionary<int, int> equalCandidatesFrequencies = [];
		List<(NormalizedProduct Product, HashSet<NormalizedProduct> EqualCandidates)> equalCandidatesOfProducts = new();
		foreach (NormalizedProduct product in smallerEshop.Products)
		{
			HashSet<NormalizedProduct> equalCandidates = ListEqualCandidates(product, largerEshop);
			if (!equalCandidatesFrequencies.TryAdd(equalCandidates.Count, 1))
				equalCandidatesFrequencies[equalCandidates.Count]++;

			equalCandidatesOfProducts.Add((product, equalCandidates));
		}

		LogStatsOfCandidates(equalCandidatesFrequencies, smallerEshop, largerEshop);
		return equalCandidatesOfProducts;
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
	private static HashSet<NormalizedProduct> ListEqualCandidates(NormalizedProduct product, EshopSubstrings largerEshop)
	{
		HashSet<NormalizedProduct> equalCandidates = [];

		foreach(string part in product.InferredData.lowerCaseNameParts)
			if(part.Length > 2 && largerEshop.SubstringsToProducts.TryGetValue(part, out List<NormalizedProduct>? value))
				foreach(NormalizedProduct prodctWithSameSubstring  in value)
					equalCandidates.Add(prodctWithSameSubstring);

		return equalCandidates;
	}

	/// <summary>
	/// Input: 
	/// product - one concrete product from smaller eshop
	/// candidates - n candidates of equal products from larger eshop
	/// 
	/// Foreach pair (product, candidate i) method calculates similarity by equal substrings ratio which is defined as:
	/// 
	/// substrings similarity = equal substrings count / Min( product.name.Split(' ').Length, candidate.name.Split(' ').Length )
	/// 
	/// In words, we take number of equal substrings recieved after spliting product's name on whitespace and divide with 
	/// the smaller number of substrings of both products.
	/// 
	/// Output:
	/// Sorted list of equal candidates from larger eshop of normalized product of smaller eshop. 
	/// Sorted by substrings similarity.
	/// 
	/// </summary>
	/// <param name="product"></param>
	/// <param name="candidates"></param>
	/// <returns></returns>
	private static void SortCandidatesBySubstring(NormalizedProduct product, HashSet<NormalizedProduct> candidates, EshopSubstrings largerEshop)
	{
		var sortedCandidates = SortCandidates(product, candidates, CalculateSubstringSimilarity);
		LogSortedCandidates("substringSimilarity", ref substringResults, product, largerEshop, sortedCandidates);
	}

	private static List<(double Similarity, NormalizedProduct Candidate)> SortCandidates(NormalizedProduct product, HashSet<NormalizedProduct> candidates, Func<NormalizedProduct, NormalizedProduct, double> calculateSimilarity)
	{
		var sortedCandidates = candidates.Select(candidate =>
			(Similarity: calculateSimilarity(product, candidate), Candidate: candidate))
			.ToList();

		sortedCandidates.Sort(new CandidateComparer());
		return sortedCandidates;
	}

	private static double CalculateSubstringSimilarity(NormalizedProduct product, NormalizedProduct candidate)
	{
		HashSet<string> productSubstrings = GetSubstringsSet(product);
		HashSet<string> candidateSubstrings = GetSubstringsSet(candidate);

		int sameSubstringsCount = 0;
		foreach(string substring in productSubstrings)
			if(candidateSubstrings.Contains(substring))
				sameSubstringsCount++;

		if(sameSubstringsCount == 0) 
			throw new ArgumentException("In this part of code, only product with at least one same substring may be called. Critical error in code architecture detected!");

		int minSubstringCount = Math.Min(productSubstrings.Count, candidateSubstrings.Count);	

		return (double)sameSubstringsCount / minSubstringCount;
	}

	private static HashSet<string> GetSubstringsSet(NormalizedProduct product) => [.. product.InferredData.lowerCaseNameParts];

	/// <summary>
	/// Input: 
	/// product - one concrete product from smaller eshop
	/// candidates - n candidates of equal products from larger eshop
	/// 
	/// Foreach pair (product, candidate i) method calculates similarity by common prefix length ratio which is defined as:
	/// 
	/// string productName = product.name.RemoveWS().ToLower()
	/// string candidateName = (candidate i).name.RemoveWS().ToLower()
	/// common prefix similarity = CommonPrefixLength( productName,  candidateName) / Math.Min(productName.Length, candidateName.Length)
	/// 
	/// In words, we first parse the names by removing whitespaces and make all characters lower case. 
	/// Than we divide the length of common prefix by smaller length out of both names.
	/// 
	/// Output:
	/// Sorted list of equal candidates from larger eshop of normalized product of smaller eshop. 
	/// Sorted by  common prefix similarity.
	///
	/// </summary>
	/// <param name="product"></param>
	/// <param name="candidates"></param>
	/// <returns></returns>
	private static void SortCandidatesByPrefix(NormalizedProduct product, HashSet<NormalizedProduct> candidates, EshopSubstrings largerEshop)
	{
		var sortedCandidates = SortCandidates(product, candidates, CalculatePrefixSimilarity);
		LogSortedCandidates("prefixSimilarity", ref prefixResults, product, largerEshop, sortedCandidates);
	}

	private static double CalculatePrefixSimilarity(NormalizedProduct product, NormalizedProduct candidate)
	{
		string parsedProductName = RemoveWS(product.Name).ToLower();	
		string parsedCandidateName = RemoveWS(candidate.Name).ToLower();

		int commonPrefixLength = CommonPrefixLength(parsedProductName, parsedCandidateName);

		return (double)commonPrefixLength / Math.Min(parsedProductName.Length, parsedCandidateName.Length);
	}

	private static string RemoveWS(string s) => MathAllWhiteSpaceChars().Replace(s, "");

	private static int CommonPrefixLength(string parsedProductName, string parsedCandidateName)
	{
		if (parsedProductName == null || parsedCandidateName == null || parsedProductName.Length == 0 || parsedCandidateName.Length == 0)
			throw new ArgumentException("Critical error in code architecture detected. Parsed product names at this point main not be null or empty.");

		int prefixLength = 0;
		for (int i = 0; i < Math.Min(parsedProductName.Length, parsedCandidateName.Length); i++)
			if (parsedProductName[i] == parsedCandidateName[i])
			{
				prefixLength++;
			} else
			{
				break;
			}

		return prefixLength;
	}

	/// <summary>
	/// Input: 
	/// product - one concrete product from smaller eshop
	/// candidates - n candidates of equal products from larger eshop
	/// 
	/// Foreach pair (product, candidate i) method calculates similarity by longest common subsequence ratio which is defined as:
	/// 
	/// LCS ratio = LCS length / Min( product.name.RemoveWS().Length, candidate.name.RemoveWS().Length )
	/// 
	/// In words, we take the length of LCS of parsed product names and divide with 
	/// the smaller number of substrings of both products.
	/// 
	/// Output:
	/// Sorted list of equal candidates from larger eshop of normalized product of smaller eshop. 
	/// Sorted by substrings similarity.
	/// 
	/// </summary>
	/// <param name="product"></param>
	/// <param name="candidates"></param>
	/// <returns></returns>
	private static void SortCandidatesByLongestCommonSubsequence(NormalizedProduct product, HashSet<NormalizedProduct> candidates, EshopSubstrings largerEshop)
	{
		var sortedCandidates = SortCandidates(product, candidates, CalculateLCS);
		LogSortedCandidates("LongestCommonSubsequenceSimilarity", ref LCSResults, product, largerEshop, sortedCandidates);
	}

	private static double CalculateLCS(NormalizedProduct product, NormalizedProduct candidate)
	{
		string parsedProductName = RemoveWS(product.Name).ToLower();
		string parsedCandidateName = RemoveWS(candidate.Name).ToLower();

		int LCS = LCSFinder.LongestCommonSubsequence(parsedProductName, parsedCandidateName);

		return (double)LCS / Math.Min(parsedProductName.Length, parsedCandidateName.Length);
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

		double averageCandidatesPerProduct = (double)candidatesSum / products;
		int numberOfProductPairs = smallerEshop.Products.Count * largerEshop.Products.Count;
		double candidatesAllPairsRatioPercentage = ((double)candidatesSum / numberOfProductPairs) / 100;

		sw.WriteLine($"Products from smaller eshop: {FormatWithSpaces(products)} should be equal to {FormatWithSpaces(smallerEshop.Products.Count)}");
		sw.WriteLine($"Sum of all candidates: {FormatWithSpaces(candidatesSum)}");
		sw.WriteLine($"Average candidates per product of smaller eshop: {averageCandidatesPerProduct:f2}");
		sw.WriteLine($"Smaller eshop has {FormatWithSpaces(smallerEshop.Products.Count)} products and larger eshop has {FormatWithSpaces(largerEshop.Products.Count)} products.");
		sw.WriteLine($"Meaning there are {FormatWithSpaces(numberOfProductPairs)} possible pairs of equal products.");
		sw.WriteLine($"ListEqualCandidates method managed to narrow down the candidate list to {FormatWithSpaces(candidatesSum)}");
		sw.WriteLine($"Which is {candidatesAllPairsRatioPercentage:f2} % of possible pairs.\n\n");

		sw.WriteLine(sb.ToString());
	}

	private static string FormatWithSpaces(int number) => number.ToString("n0", CultureInfo.InvariantCulture).Replace(",", " ");

	[GeneratedRegex(@"\s+")]
	private static partial Regex MathAllWhiteSpaceChars();

	private static int substringResults = 0;
	private static int prefixResults = 0;
	private static int LCSResults = 0;

	private static void LogSortedCandidates(string similarityType, ref int resultsCounter, NormalizedProduct product, EshopSubstrings largerEshop, List<(double Similarity, NormalizedProduct Candidate)> sortedCandidates)
	{
		Eshop largerName = largerEshop.Products.First().Eshop;
		string directory = $"{logginDirectory}{product.Eshop}_to_{largerName}/{similarityType}/";
		Directory.CreateDirectory(directory);

		using StreamWriter sw = new($"{directory}{++resultsCounter}.txt");
		sw.WriteLine($"Equal candidates of {product.Name}, to be found at url: {product.URL}");

		foreach ((double similarity, NormalizedProduct candidate) in sortedCandidates)
		{
			sw.WriteLine($"{similarity:f4}\t{candidate.Name}\t{candidate.URL}");
		}
	}

	#endregion
}

