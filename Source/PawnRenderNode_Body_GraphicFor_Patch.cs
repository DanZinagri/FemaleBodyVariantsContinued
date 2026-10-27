using HarmonyLib;
using UnityEngine;
using Verse;

namespace FemaleBodyVariants;

// Swaps the naked body graphic of a female Thin, Fat or Hulk pawn for the "_Female" variant.
// GraphicFor only runs when the pawn's render tree is rebuilt, never per frame.
[HarmonyPatch(typeof(PawnRenderNode_Body), nameof(PawnRenderNode_Body.GraphicFor))]
public static class PawnRenderNode_Body_GraphicFor_Patch
{
	// Big and Small's framework performs this same swap in its own GraphicFor postfix
	// (GenderMethods.TrySetGenderBody), which runs after this one and would redo the work.
	// It does not cover fur, so only this patch stands down.
	public static bool Prepare()
	{
		return ModLister.GetActiveModWithIdentifier("RedMattis.BetterPrerequisites", ignorePostfix: true) == null;
	}

	public static void Postfix(PawnRenderNode_Body __instance, Pawn pawn, ref Graphic __result)
	{
		if (__result == null || pawn.gender != Gender.Female)
		{
			return;
		}
		// Only act when vanilla chose the plain naked body. Dessicated bodies, the EmptyImage
		// placeholder, and graphics already replaced by an earlier patch all carry a different
		// path and are left alone. Mutant and creep joiner bodies are swapped at their own
		// lookup instead (AnomalyBodyGraphicPath_Patch).
		string basePath = pawn.story?.bodyType?.bodyNakedGraphicPath;
		if (basePath == null || __result.path != basePath)
		{
			return;
		}
		if (FemaleVariantPath.TryGet(pawn.story.bodyType, basePath, out string femalePath))
		{
			__result = GraphicDatabase.Get<Graphic_Multi>(femalePath, __instance.ShaderFor(pawn), Vector2.one, __instance.ColorFor(pawn));
		}
	}
}
