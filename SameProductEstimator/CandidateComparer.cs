namespace SameProductEstimator;

internal class CandidateComparer : IComparer<(double similarityMeasure, NormalizedProduct Candidate)>
{
	public int Compare((double similarityMeasure, NormalizedProduct Candidate) x, (double similarityMeasure, NormalizedProduct Candidate) y)
	{
		return y.similarityMeasure.CompareTo(x.similarityMeasure); // candidates with highest priority should be first in the list
	}
}