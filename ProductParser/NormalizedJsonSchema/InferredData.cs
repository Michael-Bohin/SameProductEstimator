namespace SameProductEstimator;

internal class InferredData
{
	public readonly string[] nameParts;
	public readonly List<string> lowerCaseNameParts = [];

	public InferredData(string productName)
	{
		nameParts = productName.Split(' ');
		foreach(string part in nameParts)
			lowerCaseNameParts.Add(part.ToLower());	
	}
}

