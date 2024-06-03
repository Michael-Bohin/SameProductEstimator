using System.Text;

namespace SameProductEstimator;
internal class NutritionalValues
{
	public int EnergetickaHodnotaKJ, EnergetickaHodnotaKCAL;

	public decimal Tuky, ZTohoNasyceneMastneKyseliny, Sacharidy, ZTohoCukry, Bilkoviny, Sul, Vlaknina;

	public override string ToString()
	{
		StringBuilder sb = new();
		sb.AppendLine("Nutricni hodnoty na 100 g:");
		sb.AppendLine($"Enerigeticka hodnota KJ {EnergetickaHodnotaKJ}");
		sb.AppendLine($"Enerigeticka hodnota KCAL {EnergetickaHodnotaKCAL}");
		sb.AppendLine($"Tuky {Tuky}");
		sb.AppendLine($"Mastne kyseliny {ZTohoNasyceneMastneKyseliny}");
		sb.AppendLine($"Sacharidy{Sacharidy}");
		sb.AppendLine($"Cukry {ZTohoCukry}");
		sb.AppendLine($"Bilkoviny {Bilkoviny}");
		sb.AppendLine($"Sul {Sul}");
		sb.AppendLine($"Vlaknina {Vlaknina}");

		return sb.ToString();	
	}

	public NutritionalValues(int energetickaHodnotaKJ, int energetickaHodnotaKCAL, decimal tuky, 
		decimal zTohoNasyceneMastneKyseliny, decimal sacharidy, decimal zTohoCukry,
		decimal bilkoviny, decimal sul, decimal vlaknina) 
	{ 
		EnergetickaHodnotaKJ = energetickaHodnotaKJ;
		EnergetickaHodnotaKCAL = energetickaHodnotaKCAL;	
		Tuky = tuky;
		ZTohoNasyceneMastneKyseliny = zTohoNasyceneMastneKyseliny;
		Sacharidy = sacharidy;
		ZTohoCukry = zTohoCukry;
		Bilkoviny = bilkoviny;
		Sul = sul;
		Vlaknina = vlaknina;

		AssertIsNonNegative(EnergetickaHodnotaKJ);
		AssertIsNonNegative(EnergetickaHodnotaKCAL);
		AssertIsNonNegative(Tuky);
		AssertIsNonNegative(ZTohoNasyceneMastneKyseliny);
		AssertIsNonNegative(Sacharidy);
		AssertIsNonNegative(ZTohoCukry);
		AssertIsNonNegative(Bilkoviny);
		AssertIsNonNegative(Sul);
		AssertIsNonNegative(Vlaknina);
	}

	private static void AssertIsNonNegative(decimal d)
	{
		if (d < 0)
			throw new ArgumentException($"{d}");
	}

	private static void AssertIsNonNegative(int i)
	{
		if(i < 0)
			throw new ArgumentException($"{i}");
	}
}
