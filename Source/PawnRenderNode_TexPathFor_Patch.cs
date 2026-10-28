using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace FemaleBodyVariants;

// Swaps a render node's per-body-type texture for its "_Female" variant on female Thin, Fat or
// Hulk pawns, e.g. a gene node's <Hulk>Tails/MyTail_Hulk</Hulk> -> Tails/MyTail_Hulk_Female.
//
// TexPathFor is the base lookup behind nearly every gene, hediff and trait node, so this only
// acts when the returned path is the node's own bodyTypeGraphicPaths entry for the pawn's body
// type. Plain texPath / texPathFemale nodes, and subclasses that override TexPathFor without
// calling the base (VEF's PawnRenderNode_Omni, most Big and Small nodes), are left alone.
[HarmonyPatch(typeof(PawnRenderNode), "TexPathFor")]
public static class PawnRenderNode_TexPathFor_Patch
{
	public static void Postfix(PawnRenderNode __instance, Pawn pawn, ref string __result)
	{
		if (__result == null || pawn.gender != Gender.Female)
		{
			return;
		}
		List<BodyTypeGraphicData> paths = __instance.Props.bodyTypeGraphicPaths;
		BodyTypeDef bodyType = pawn.story?.bodyType;
		if (paths == null || bodyType == null)
		{
			return;
		}
		// Vanilla returns the first entry matching the body type; anything else came from texPath
		// or an override, not from this list.
		for (int i = 0; i < paths.Count; i++)
		{
			if (paths[i].bodyType == bodyType)
			{
				if (paths[i].texturePath == __result && FemaleVariantPath.TryGet(bodyType, __result, out string femalePath))
				{
					__result = femalePath;
				}
				return;
			}
		}
	}
}
