using HarmonyLib;
using RimWorld;
using Verse;

namespace FemaleBodyVariants;

// Swaps a fur body path for its "_Female" variant on female Thin, Fat or Hulk pawns.
//
// This sits on the FurDef lookup rather than the fur render node, so every caller sees the
// swapped path: the vanilla fur node, Big and Small's fur nodes, and any mod that reads a
// FurDef path to build a mask. That is intended, since those should follow the body shape.
[HarmonyPatch(typeof(FurDef), nameof(FurDef.GetFurBodyGraphicPath))]
public static class FurDef_GetFurBodyGraphicPath_Patch
{
	public static void Postfix(Pawn pawn, ref string __result)
	{
		if (__result == null || pawn.gender != Gender.Female)
		{
			return;
		}
		if (pawn.Drawer.renderer.CurRotDrawMode == RotDrawMode.Dessicated)
		{
			return;
		}
		// Mutants and creep joiners with their own body art keep the fur that matches it.
		if (ModsConfig.AnomalyActive)
		{
			if (pawn.IsMutant && !pawn.mutant.Def.bodyTypeGraphicPaths.NullOrEmpty())
			{
				return;
			}
			if (pawn.IsCreepJoiner && !pawn.creepjoiner.form.bodyTypeGraphicPaths.NullOrEmpty())
			{
				return;
			}
		}
		// The lookup that produced __result matched on pawn.story.bodyType, so that def is the
		// key this path was chosen for.
		if (FemaleVariantPath.TryGet(pawn.story.bodyType, __result, out string femalePath))
		{
			__result = femalePath;
		}
	}
}
