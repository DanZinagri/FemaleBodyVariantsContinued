using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace FemaleBodyVariants;

[HarmonyPatch(typeof(FurDef), "GetFurBodyGraphicPath")]
public static class FurDef_GetFurBodyGraphicPath_Patch
{
	[HarmonyPostfix]
	public static void Postfix(FurDef __instance, ref Pawn pawn, ref string __result)
	{
		if (__result == null || (int)pawn.Drawer.renderer.CurRotDrawMode == 4 || (ModsConfig.AnomalyActive && pawn.IsMutant && !GenList.NullOrEmpty<BodyTypeGraphicData>((IList<BodyTypeGraphicData>)pawn.mutant.Def.bodyTypeGraphicPaths)) || (ModsConfig.AnomalyActive && pawn.IsCreepJoiner && pawn.story.bodyType != null && !GenList.NullOrEmpty<BodyTypeGraphicData>((IList<BodyTypeGraphicData>)pawn.creepjoiner.form.bodyTypeGraphicPaths)))
		{
			return;
		}
		object obj = pawn.story?.bodyType?.bodyNakedGraphicPath;
		if (obj == null)
		{
			return;
		}
		string text = __result;
		if ((int)pawn.gender != 1 && text != null && !text.Contains("_Female") && (text.Contains("_Thin") || text.Contains("_Fat") || text.Contains("_Hulk")))
		{
			string text2 = text + "_Female";
			if ((Object)(object)ContentFinder<Texture2D>.Get(text2 + "_south", false) != (Object)null)
			{
				__result = text2;
			}
		}
	}
}
