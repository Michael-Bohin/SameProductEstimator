namespace SameProductEstimator;

internal abstract class Adapter
{
	public abstract List<NormalizedProduct> GetNormalizedProducts(string json);
}

