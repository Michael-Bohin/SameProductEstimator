using SameProductEstimator;

namespace UnitTests
{
	public class EditDistanceTests
	{
		[Theory]
		[InlineData("ahoj", "ahojabcdefghijklmno", 0)]
		[InlineData("modra", "zelena", 4)]
		[InlineData("Lays bramburky chilli and lime", "Lays bramburky s limetkovou prichuti a dlouhym popiskem od kosiku a taky s chilli", 15)] // ve skutecnosti tohle bude nejspis mensi cislo, ale nemam CPU v hlave abcyh to nasel manualne
		[InlineData("", "", 0)]
		[InlineData("modra", "modra", 0)]
		[InlineData("", "zelena", 0)]
		[InlineData("veverka medved pes", "sova veverka delfin", 10)]
		[InlineData("auto", "aauto", 0)]
		[InlineData("aubto", "aucto", 1)]
		[InlineData("a veverka c", "b vezerka d", 3)]
		public void LengthAdjustedEditDistance(string x, string y, int expectedAdjustedEditDistance)
		{
			// Act 
			int actual = LevenshteinDistance.LengthAdjustedEditDistance(x, y);

			// Assert
			Assert.Equal(expectedAdjustedEditDistance, actual);
		}
	}
}
