using HarmonyLib;
using Verse;

namespace FemaleBodyVariants;

[StaticConstructorOnStartup]
public static class HarmonyInit
{
	static HarmonyInit()
	{
		new Harmony("FemaleBodyVariants").PatchAll();
	}
}
