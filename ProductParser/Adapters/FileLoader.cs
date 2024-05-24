namespace SameProductEstimator;

internal class FileLoader
{
	// throws if filepath does not exist 
	public static string LoadFileContents(string relativePath)
	{
		if(File.Exists(relativePath))
		{
			return File.ReadAllText(relativePath);
		}

		throw new FileNotFoundException(relativePath);
	}
}
