using System.IO.Compression;
using System.Text;

namespace SameProductEstimator;

internal class FileHandler
{
	public static string LoadJsonFromPath(string relativePath)
	{
		if (File.Exists(relativePath))
		{
			return File.ReadAllText(relativePath);
		}

		throw new FileNotFoundException(relativePath);
	}

	public static string LoadJsonFromPath(string path, string extractPath)
	{
		if (Directory.Exists(extractPath))
			Directory.Delete(extractPath, true);

		Directory.CreateDirectory(extractPath);

		ZipFile.ExtractToDirectory(path, extractPath);

		string jsonFilePath = Directory.GetFiles(extractPath, "*.json").FirstOrDefault()!;
		string json = File.ReadAllText(jsonFilePath!, Encoding.UTF8);

		Directory.Delete(extractPath, true);

		return json;
	}
}
