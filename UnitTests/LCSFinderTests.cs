using SameProductEstimator;

namespace UnitTests
{
	public class LCSFinderTests
	{
		[Theory]
		[InlineData("ahoj", "modraahojzelena", 4)]
		[InlineData("abcd", "efgh", 0)]
		[InlineData("abcdZefgh", "ijklZnopq", 1)]
		[InlineData("  ", "  ", 2)]
		[InlineData("orel", "orel", 4)]
		// [InlineData("orelLiskaMedvedKockaPes", "VeverkaSykorkaMedvedDelfinSova", 6)]
		// [InlineData("Lorem ipsum", "Ja jsem se vas chtela zeptat jestli mate sklep", 2)] // Lorem - jsem -> 2
		[InlineData("", "Ja jsem se vas chtela zeptat jestli mate sklep", 0)]
		[InlineData("", "", 0)]
		public void LongestCommonSubsequence(string x, string y, int expectedLCS)
		{
			// Act 
			int actual = LCSFinder.LongestCommonSubsequence(x, y);

			// Assert 
			Assert.Equal(expectedLCS, actual);
		}
	}
}