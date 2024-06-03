namespace SameProductEstimator.Tesco;


#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Rootobject
{
	public TescoJsonProduct[] Property1 { get; set; }
}

public class TescoJsonProduct
{
	public Promotion[] promotions { get; set; }
	public bool isSponsoredProduct { get; set; }
	public bool isWhyNotTry { get; set; } // lol, property naming of the year goes to Tesco software developers :D :D 
	public Product product { get; set; }
}

public class Product
{
	public string typename { get; set; }
	public object context { get; set; }
	public string id { get; set; }
	public object modelMetadata { get; set; }
	public object gtin { get; set; }
	public object adId { get; set; }
	public string baseProductId { get; set; }
	public string title { get; set; }
	public object seller { get; set; }
	public object brandName { get; set; }
	public string shortDescription { get; set; }
	public string defaultImageUrl { get; set; }
	public string superDepartmentId { get; set; }
	public string superDepartmentName { get; set; }
	public string departmentId { get; set; }
	public string departmentName { get; set; }
	public string aisleId { get; set; }
	public string aisleName { get; set; }
	public object shelfId { get; set; }
	public object shelfName { get; set; }
	public string displayType { get; set; }
	public string productType { get; set; }
	public object charges { get; set; }
	public float averageWeight { get; set; }
	public int bulkBuyLimit { get; set; }
	public int maxQuantityAllowed { get; set; }
	public int groupBulkBuyLimit { get; set; }
	public object bulkBuyLimitMessage { get; set; }
	public object bulkBuyLimitGroupId { get; set; }
	public object timeRestrictedDelivery { get; set; }
	public object restrictedDelivery { get; set; }
	public bool isForSale { get; set; }
	public bool isInFavourites { get; set; }
	public object isNew { get; set; }
	public object isRestrictedOrderAmendment { get; set; }
	public string status { get; set; }
	public object maxWeight { get; set; }
	public object minWeight { get; set; }
	public object increment { get; set; }
	public object details { get; set; }
	public Catchweightlist[] catchWeightList { get; set; }
	public object[] restrictions { get; set; }
	public decimal price { get; set; }
	public float unitPrice { get; set; }
	public string unitOfMeasure { get; set; }
	public object[] substitutions { get; set; }
}

public class Catchweightlist
{
	public float price { get; set; }
	public float weight { get; set; }
}

public class Promotion
{
	public string promotionId { get; set; }
	public string promotionType { get; set; }
	public DateTime startDate { get; set; }
	public DateTime endDate { get; set; }
	public object unitSellingInfo { get; set; }
	public string offerText { get; set; }
	public object price { get; set; }
	public string[] attributes { get; set; }
}

#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.