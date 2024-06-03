namespace SameProductEstimator;

public class LevenshteinDistance 
{
	public static int LengthAdjustedEditDistance(string x, string y)
	{
		int n = x.Length;
		int m = y.Length;
		int[,] editDistanceTable = new int[n + 1, m + 1];

		// Vyplnovani editDistanceTable[][] je v bottom up smeru
		for (int i = 0; i <= n; i++)
			for (int j = 0; j <= m; j++)
			{
				if (i == 0) // je-li prvni string prazdny, jediny zpusob je vlozeni znaku druheho stringu
					editDistanceTable[i, j] = j;  // Min. operations = j

				else if (j == 0) // opacne je-li druhy string prazdny, postupujeme odebiranim znaku prvniho stringu
					editDistanceTable[i, j] = i; // Min. operations = i

				else if (x[i - 1] == y[j - 1]) // je-li posledni znak stejny, ignoruj posledni znak a skoc na zbytek obou stringu
					editDistanceTable[i, j] = editDistanceTable[i - 1, j - 1];

				else // jsou-posledni znaky ruzne, zvaz vsechny tri moznosti a vyber minimum
					editDistanceTable[i, j] = 1 + Math.Min(Math.Min(editDistanceTable[i - 1, j],    // Odebrani
													 editDistanceTable[i, j - 1]),					// Vlozeni
											 editDistanceTable[i - 1, j - 1]);						// Prepsani
			}

		int result = editDistanceTable[n, m] - Math.Abs(n - m); // od vysledku odecteme rozdil delek stringu v absolutni hodnote

		// celkem prirozene je zrejme, ze rozdil delek muze byt nanejvys editacni vzdalenost, 
		// ale z principu defenzivniho programovani se zabijeme pokud by tento 
		// invariant neplatil (muze nastat jen pri bugu v kodu)
		if(result < 0)
			throw new InvalidOperationException("Nemozny vysledek, v kodu je nekde chyba.");

		return  result;
	}
}
