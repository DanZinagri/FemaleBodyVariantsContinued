using System.Reflection;
using HarmonyLib;
using Verse;

namespace FemaleBodyVariants;

// Patches from the Mod constructor rather than a StaticConstructorOnStartup class, so the work
// happens during mod loading instead of in the UI freeze at the end of startup. The assembly is
// passed explicitly: on .NET 10 (1.7) the parameterless PatchAll() reads the calling assembly off
// the stack, which breaks when the JIT inlines the call (Harmony issue #395).
public class FemaleBodyVariantsMod : Mod
{
	public FemaleBodyVariantsMod(ModContentPack content) : base(content)
	{
		new Harmony("FemaleBodyVariants").PatchAll(Assembly.GetExecutingAssembly());
	}
}
