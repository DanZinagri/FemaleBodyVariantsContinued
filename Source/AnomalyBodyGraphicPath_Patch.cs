using HarmonyLib;
using RimWorld;
using Verse;

namespace FemaleBodyVariants;

// Swaps a mutant (ghoul) or creep joiner body path for its "_Female" variant on female Thin,
// Fat or Hulk pawns, e.g. Ghoulskin_Fat -> Ghoulskin_Fat_Female. Vanilla ships no such art, so
// this only does something when a texture mod provides it.
//
// Patched at the def lookup, like FurDef, so PawnRenderNode_Body and any other caller get the
// same path. Big and Small leaves these bodies to vanilla (BodyGraphics.ShowStandardBody), so
// unlike the naked body patch this one stays active alongside it.
public static class AnomalyBodyGraphicPath_Patch
{
	public static void Swap(Pawn pawn, ref string path)
	{
		if (path == null || pawn.gender != Gender.Female)
		{
			return;
		}
		if (FemaleVariantPath.TryGet(pawn.story.bodyType, path, out string femalePath))
		{
			path = femalePath;
		}
	}
}

[HarmonyPatch(typeof(MutantDef), nameof(MutantDef.GetBodyGraphicPath))]
public static class MutantDef_GetBodyGraphicPath_Patch
{
	public static void Postfix(Pawn pawn, ref string __result)
	{
		AnomalyBodyGraphicPath_Patch.Swap(pawn, ref __result);
	}
}

[HarmonyPatch(typeof(CreepJoinerFormKindDef), nameof(CreepJoinerFormKindDef.GetBodyGraphicPath))]
public static class CreepJoinerFormKindDef_GetBodyGraphicPath_Patch
{
	public static void Postfix(Pawn pawn, ref string __result)
	{
		AnomalyBodyGraphicPath_Patch.Swap(pawn, ref __result);
	}
}
