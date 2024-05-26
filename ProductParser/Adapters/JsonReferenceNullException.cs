namespace SameProductEstimator;

internal class JsonReferenceNullException : ArgumentException
{
	public JsonReferenceNullException(string message) : base(message) { }

	public JsonReferenceNullException() : base() { }
}
