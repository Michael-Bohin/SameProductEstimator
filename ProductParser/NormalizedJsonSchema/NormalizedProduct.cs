using System.Text;

namespace SameProductEstimator;

internal class NormalizedProduct
{
	// critical properties of product that may not have null value
	public string Name, URL;
	public decimal Price;
	public Eshop Eshop;

	// non-critical nullable properties
	public string? Producer = null, Description = null, StorageConditions = null;

	public UnitType? UnitType = null;

	// Pieces, Wieght and Volume have non-null value for respective UnitType
	// Weight is measured in grams or kilograms? 
	// Volume is measered in liters or mililiters?
	// Constructor checks that always exactly one of the is not null
	// Further all values must nonnegative values, with tighter ranges coming with more experience about data
	public int? Pieces = null;
	public decimal? Weight = null, Volume = null;

	public NutritionalValues? NutritionalValues = null;

	// own semantic over data from eshops:
	public InferredData InferredData;

	internal NormalizedProduct(string name, string url, decimal price, Eshop eshop)
	{
		Name = AssertStringIsNotNullOrEmpty(name);
		URL = AssertStringIsNotNullOrEmpty(url); 
		Eshop = eshop;

		if(price < 0)
			throw new ArgumentException($"{price}");
		Price = price;

		InferredData = new(name);
	}

	public override string ToString()
	{
		StringBuilder sb = new();
		sb.AppendLine(Name);
		sb.AppendLine($"{Price}");
		sb.AppendLine($"{Eshop}");
		sb.AppendLine(URL);
		sb.AppendLine(Description);
		sb.AppendLine(Producer);
		sb.AppendLine(StorageConditions + "\n");

		sb.AppendLine($"{UnitType}");
		sb.AppendLine($"{Pieces}");
		sb.AppendLine($"{Weight}");
		sb.AppendLine($"{Volume}");

		sb.AppendLine($"{NutritionalValues}");

		// nutritional values are yet omitted..	
		return sb.ToString();
	}
	private static string AssertStringIsNotNullOrEmpty(string s)
	{
		if(string.IsNullOrEmpty(s))
			throw new ArgumentException(s);
		return s;
	}

	public void SetDescription(string description) => Description = description;
	public void SetProducer(string producer) => Producer = producer;
	public void SetStorageConditions(string conditions)	=> StorageConditions = conditions;
	
	public void SetPieces(int pieces)
	{
		if(UnitType is not null)
		{
			throw new InvalidOperationException($"Unit type has been attempted to be set twice.");
		}

		UnitType = SameProductEstimator.UnitType.Pieces;
		Pieces = pieces;
	}

	public void SetWeight(decimal weight)
	{
		if (UnitType is not null)
		{
			throw new InvalidOperationException($"Unit type has been attempted to be set twice.");
		}
		UnitType = SameProductEstimator.UnitType.Weight;
		Weight = weight;
	}

	public void SetVolume(decimal volume)
	{
		if (UnitType is not null)
		{
			throw new InvalidOperationException($"Unit type has been attempted to be set twice.");
		}
		UnitType = SameProductEstimator.UnitType.Volume;
		Volume = volume;
	}
}

