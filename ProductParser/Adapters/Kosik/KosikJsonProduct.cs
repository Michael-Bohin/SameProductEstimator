namespace SameProductEstimator.Kosik;

#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Rootobject
{
	public KosikJsonProduct[] Property1 { get; set; }
}

public class KosikJsonProduct
{
	public Breadcrumb[] breadcrumbs { get; set; }
	public object[] gifts { get; set; }
	public object[] shoppingLists { get; set; }
	public Product product { get; set; }
}

public class Product
{
	public int id { get; set; }
	public string name { get; set; }
	public string image { get; set; }
	public string url { get; set; }
	public decimal? price { get; set; }
	public int returnablePackagePrice { get; set; }
	public string unit { get; set; }
	public decimal recommendedPrice { get; set; }
	public int percentageDiscount { get; set; }
	public Productquantity productQuantity { get; set; }
	public Label[] labels { get; set; }
	public string actionLabel { get; set; }
	public string countryCode { get; set; }
	public string[] pictographs { get; set; }
	public int maxInCart { get; set; }
	public int? limitInCart { get; set; }
	public object firstOrderDay { get; set; }
	public object lastOrderDay { get; set; }
	public object plannedStock { get; set; }
	public object relatedProduct { get; set; }
	public Maincategory mainCategory { get; set; }
	public Priceperunit pricePerUnit { get; set; }
	public Cumulativeprice[] cumulativePrices { get; set; }
	public object[] giftIds { get; set; }
	public bool favorite { get; set; }
	public bool purchased { get; set; }
	public int unitStep { get; set; }
	public int vendorId { get; set; }
	public object pharmacyCertificate { get; set; }
	public object[] productGroups { get; set; }
	public float recommendedSellPrice { get; set; }
	public Detail detail { get; set; }
	public bool hasAssociatedProducts { get; set; }
	public bool eLicence { get; set; }
	public object marketplaceVendor { get; set; }
}

public class Productquantity
{
	public string prefix { get; set; }
	public decimal value { get; set; }
	public string unit { get; set; }
}

public class Maincategory
{
	public int id { get; set; }
	public string name { get; set; }
	public string url { get; set; }
	public object image { get; set; }
	public bool highlighted { get; set; }
	public int vendorId { get; set; }
}

public class Priceperunit
{
	public decimal price { get; set; }
	public string unit { get; set; }
}

public class Detail
{
	public bool adultOnly { get; set; }

	public Brand? brand { get; set; }

	public string sapId { get; set; }
	public object[] shoppingListIds { get; set; }
	public string[] photos { get; set; }
	public Supplierinfo[] supplierInfo { get; set; }
	public Origin[] origin { get; set; }
	public Description[] description { get; set; }
	public Ingredient[] ingredients { get; set; }
	public KosikNutritionalValues nutritionalValues { get; set; }
	public Parametergroup[] parameterGroups { get; set; }
	public Bestbefore bestBefore { get; set; }
	public object associationCode { get; set; }
	public bool unlisted { get; set; }
	public object metaDescription { get; set; }
}

public class Brand
{
	public int id { get; set; }
	public string? name { get; set; }
	public string url { get; set; }
}

public class KosikNutritionalValues
{
	public int valuesPerGrams { get; set; }
	public string title { get; set; }
	public NutritionalValue[] values { get; set; }
}

public class NutritionalValue
{
	public string title { get; set; }
	public object prefix { get; set; }
	public string value { get; set; }
	public string unit { get; set; }
}

public class Bestbefore
{
	public int usual { get; set; }
	public int guaranteed { get; set; }
}

public class Supplierinfo
{
	public string title { get; set; }
	public string type { get; set; }
	public string value { get; set; }
}

public class Origin
{
	public string title { get; set; }
	public string type { get; set; }
	public string value { get; set; }
}

public class Description
{
	public string title { get; set; }
	public string type { get; set; }
	public string value { get; set; }
}

public class Ingredient
{
	public string title { get; set; }
	public string type { get; set; }
	public string value { get; set; }
}

public class Parametergroup
{
	public string title { get; set; }
	public Item[] items { get; set; }
}

public class Item
{
	public string title { get; set; }
	public string value { get; set; }
}

public class Label
{
	public int id { get; set; }
	public string name { get; set; }
	public string url { get; set; }
	public string background { get; set; }
	public int priority { get; set; }
	public string styleKey { get; set; }
	public bool excludeFromBox { get; set; }
}

public class Cumulativeprice
{
	public int quantity { get; set; }
	public float price { get; set; }
	public Priceperunit1 pricePerUnit { get; set; }
	public object associationCode { get; set; }
}

public class Priceperunit1
{
	public float price { get; set; }
	public string unit { get; set; }
}

public class Breadcrumb
{
	public int id { get; set; }
	public string name { get; set; }
	public string url { get; set; }
}

#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.