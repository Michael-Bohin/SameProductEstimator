namespace SameProductEstimator;

public class LCSFinder
{
	public static int LongestCommonSubsequence(string x, string y)
	{
		int m = x.Length;
		int n = y.Length;
		int[,] lcsLengthTable = new int[m + 1, n + 1];

		for (int i = 1; i <= m; i++)
		{
			for (int j = 1; j <= n; j++)
			{
				if (x[i - 1] == y[j - 1])
				{
					lcsLengthTable[i, j] = lcsLengthTable[i - 1, j - 1] + 1;
				} else
				{
					lcsLengthTable[i, j] = Math.Max(lcsLengthTable[i - 1, j], lcsLengthTable[i, j - 1]);
				}
			}
		}

		return lcsLengthTable[m, n];
	}
}