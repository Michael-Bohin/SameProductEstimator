namespace SameProductEstimator;

internal class NormalizedProduct
{
	public string Name, Producer, Description, StorageConditions, URL;

	public Eshop Eshop;

	public decimal Price;

	public UnitType UnitType;

	// Pieces, Wieght and Volume have non-null value for respective UnitType
	// Weight is measured in grams or kilograms? 
	// Volume is measered in liters or mililiters?
	// Constructor checks that always exactly one of the is not null
	// Further all values must nonnegative values, with tighter ranges coming with more experience about data
	public int? Pieces;
	public decimal? Weight, Volume;

	public NutritionalValues NutritionalValues;

	internal NormalizedProduct(string name, string producer, string description, string storageConditions, string URL, Eshop eshop,
		decimal price, UnitType unitType, int? pieces, decimal? weight, decimal? volume, NutritionalValues nutritionalValues)
	{
		AssertStringIsNotNullOrEmpty(name);
		AssertStringIsNotNullOrEmpty(producer);
		AssertStringIsNotNullOrEmpty(description);
		AssertStringIsNotNullOrEmpty(storageConditions);
		AssertStringIsNotNullOrEmpty(URL);

		Name = name;
		Producer = producer;
		Description = description;
		StorageConditions = storageConditions;
		this.URL = URL;

		Eshop = eshop;

		if(price < 0)
			throw new ArgumentException($"{price}");

		Price = price;

		AssertUnitTypeIsNotNullAndNonNegativeAndUnusedAreNull(pieces, weight, volume, unitType);

		UnitType = unitType;
		Pieces = pieces;
		Weight = weight;
		Volume = volume;

		NutritionalValues = nutritionalValues;
	}

	private static void AssertStringIsNotNullOrEmpty(string s)
	{
		if(string.IsNullOrEmpty(s))
			throw new ArgumentException(s);
	}

	private static void AssertUnitTypeIsNotNullAndNonNegativeAndUnusedAreNull(int? pieces, decimal? weight, decimal? volume, UnitType unitType)
	{
		if(pieces is not null)
		{
			if(pieces < 0)		
				throw new ArgumentException($"{pieces}");

			if(unitType != UnitType.Pieces)
				throw new ArgumentException("Pieces are not null, but unit type carries different enum label.");
		}

		if(weight is not null)
		{
			if(weight < 0)
				throw new ArgumentException($"{weight}");

			if (unitType != UnitType.Weight)
				throw new ArgumentException("Weight is not null, but unit type carries different enum label.");
		}

		if(volume is not null)
		{
			if (volume < 0)
				throw new ArgumentException($"{volume}");

			if (unitType != UnitType.Volume)
				throw new ArgumentException("Volume is not null, but unit type carries different enum label.");
		}
	}
}

