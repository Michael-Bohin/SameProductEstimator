
namespace SameProductEstimator.Rohlik;

#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Rootobject
{
	public RohlikJsonProduct[] Property1 { get; set; }
}

public class RohlikJsonProduct
{
	public string url { get; set; }
	public int id { get; set; }
	public string name { get; set; }
	public string slug { get; set; }
	public int mainCategoryId { get; set; }
	public string unit { get; set; }
	public string textualAmount { get; set; }
	public Badge[] badges { get; set; }
	public bool archived { get; set; }
	public bool premiumOnly { get; set; }
	public string brand { get; set; }
	public string[] images { get; set; }
	public Country[] countries { get; set; }
	public bool canBeFavorite { get; set; }
	public bool canBeRated { get; set; }
	public object[] information { get; set; }
	public object image3dData { get; set; }
	public object adviceForSafeUse { get; set; }
	public string countryOfOriginFlagIcon { get; set; }
	public Productstory productStory { get; set; }
	public Filter[] filters { get; set; }
	public bool weightedItem { get; set; }
	public object packageRatio { get; set; }
	public int sellerId { get; set; }
	public string flag { get; set; }
	public object[] attachments { get; set; }
	public int productId { get; set; }
	public Price price { get; set; }
	public Priceperunit pricePerUnit { get; set; }
	public Sale[] sales { get; set; }
	public string lastMinuteTitle { get; set; }
	public int warehouseId { get; set; }
	public Packageinfo packageInfo { get; set; }
	public bool preorderEnabled { get; set; }
	public int maxBasketAmount { get; set; }
	public string maxBasketAmountReason { get; set; }
	public string unavailabilityReason { get; set; }
	public object deliveryRestriction { get; set; }
	public object expectedReplenishment { get; set; }
	public int availabilityDimension { get; set; }
	public Shelflife shelfLife { get; set; }
	public object billablePackaging { get; set; }
	public Tooltip[] tooltips { get; set; }
	public Freshness freshness { get; set; }
	public bool inStock { get; set; }
	public Category[] categories { get; set; }
	public Nutritionalvalue[] nutritionalValues { get; set; }
	public Ingredient[] ingredients { get; set; }
	public object plainIngredients { get; set; }
	public Allergens allergens { get; set; }
	public int?[] similarProductIds { get; set; }
	public int?[] otherProductsOfBrand { get; set; }
	public string htmlDescription { get; set; }
}

public class Productstory
{
	public int id { get; set; }
	public int productId { get; set; }
	public string title { get; set; }
	public string text { get; set; }
	public string image { get; set; }
	public string cardColor { get; set; }
	public string textColor { get; set; }
}

public class Price
{
	public decimal amount { get; set; }
	public string currency { get; set; }
}

public class Priceperunit
{
	public float amount { get; set; }
	public string currency { get; set; }
}

public class Packageinfo
{
	public float amount { get; set; }
	public string unit { get; set; }
}

public class Shelflife
{
	public string type { get; set; }
	public int? average { get; set; }
	public int? minimal { get; set; }
	public object bestBefore { get; set; }
}

public class Freshness
{
	public string message { get; set; }
}

public class Allergens
{
	public string[] contained { get; set; }
	public string[] possiblyContained { get; set; }
}

public class Badge
{
	public string type { get; set; }
	public string title { get; set; }
	public object subtitle { get; set; }
	public string tooltip { get; set; }
}

public class Country
{
	public string name { get; set; }
	public string nameId { get; set; }
	public string code { get; set; }
}

public class Filter
{
	public string type { get; set; }
	public string slug { get; set; }
	public Value[] values { get; set; }
}

public class Value
{
	public string name { get; set; }
	public string slug { get; set; }
}

public class Sale
{
	public int id { get; set; }
	public int amount { get; set; }
	public bool unlimitedAmount { get; set; }
	public Shelflife1 shelfLife { get; set; }
}

public class Shelflife1
{
	public string type { get; set; }
	public int? average { get; set; }
	public int? minimal { get; set; }
	public DateTime? bestBefore { get; set; }
}

public class Tooltip
{
	public string type { get; set; }
	public bool closable { get; set; }
	public int? triggerAmount { get; set; }
	public object size { get; set; }
	public string message { get; set; }
	public bool actionable { get; set; }
}

public class Category
{
	public int id { get; set; }
	public string type { get; set; }
	public string name { get; set; }
	public string slug { get; set; }
	public int level { get; set; }
}

public class Nutritionalvalue
{
	public int productId { get; set; }
	public string portion { get; set; }
	public Values values { get; set; }
}

public class Values
{
	public Energykj? energyKJ { get; set; }
	public Energykcal? energyKCal { get; set; }
	public Fats? fats { get; set; }
	public Saturatedfats? saturatedFats { get; set; }
	public Carbohydrates? carbohydrates { get; set; }
	public Sugars? sugars { get; set; }
	public Protein? protein { get; set; }
	public Salt? salt { get; set; }
	public Fiber? fiber { get; set; }
}

public class Energykj
{
	public decimal? amount { get; set; }
	public string unit { get; set; }
}

public class Energykcal
{
	public decimal? amount { get; set; }
	public string unit { get; set; }
}

public class Fats
{
	public float? amount { get; set; }
	public string unit { get; set; }
}

public class Saturatedfats
{
	public float? amount { get; set; }
	public string unit { get; set; }
}

public class Carbohydrates
{
	public float? amount { get; set; }
	public string unit { get; set; }
}

public class Sugars
{
	public float? amount { get; set; }
	public string unit { get; set; }
}

public class Protein
{
	public float? amount { get; set; }
	public string unit { get; set; }
}

public class Salt
{
	public float? amount { get; set; }
	public string unit { get; set; }
}

public class Fiber
{
	public float? amount { get; set; }
	public string unit { get; set; }
}

public class Ingredient
{
	public string type { get; set; }
	public Ingredient1[] ingredients { get; set; }
	public string title { get; set; }
	public Value1 value { get; set; }
	public string code { get; set; }
	public string link { get; set; }
}

public class Value1
{
	public float amount { get; set; }
	public string unit { get; set; }
}

public class Ingredient1
{
	public string type { get; set; }
	public Ingredient2[] ingredients { get; set; }
	public string title { get; set; }
	public string code { get; set; }
	public string link { get; set; }
	public Value2 value { get; set; }
}

public class Value2
{
	public float amount { get; set; }
	public string unit { get; set; }
}

public class Ingredient2
{
	public string type { get; set; }
	public object[] ingredients { get; set; }
	public string title { get; set; }
	public string code { get; set; }
	public string link { get; set; }
}

#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.