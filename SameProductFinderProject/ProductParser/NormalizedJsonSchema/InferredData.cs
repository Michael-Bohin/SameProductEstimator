using System.Globalization;
using System.Text;

namespace SameProductEstimator;

internal class InferredData
{
	public readonly string[] nameParts;
	public readonly List<string> lowerCaseNameParts = [];
	public readonly string uniqueFileName;

	public InferredData(string productName)
	{
		nameParts = productName.Split(' ');
		foreach(string part in nameParts)
			lowerCaseNameParts.Add(part.ToLower());	

		uniqueFileName = FilterLetters(productName);
	}

	public static string FilterLetters(string s)
	{
		string normalizedString = s.Normalize(NormalizationForm.FormD);
		StringBuilder stringBuilder = new();
		foreach (char c in normalizedString)
			if (c == ' ') 
			{ 
				stringBuilder.Append('_');
			} else if( char.IsLetter(c) && CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
			{
				char letter = Char.ToLower(c);
				stringBuilder.Append(letter);
				if (stringBuilder.Length > 60)
					break;
			}

		return stringBuilder.ToString();
	}
}

