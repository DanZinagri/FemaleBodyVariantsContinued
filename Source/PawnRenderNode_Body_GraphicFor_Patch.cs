using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace FemaleBodyVariants;

[HarmonyPatch(typeof(PawnRenderNode_Body), "GraphicFor")]
public static class PawnRenderNode_Body_GraphicFor_Patch
{
	[HarmonyPostfix]
	public static void Postfix(PawnRenderNode_Body __instance, ref Pawn pawn, ref Graphic __result)
	{
		if (__result == null || (int)pawn.Drawer.renderer.CurRotDrawMode == 2 || (ModsConfig.AnomalyActive && pawn.IsMutant && !GenList.NullOrEmpty<BodyTypeGraphicData>((IList<BodyTypeGraphicData>)pawn.mutant.Def.bodyTypeGraphicPaths)) || (ModsConfig.AnomalyActive && pawn.IsCreepJoiner && pawn.story.bodyType != null && !GenList.NullOrEmpty<BodyTypeGraphicData>((IList<BodyTypeGraphicData>)pawn.creepjoiner.form.bodyTypeGraphicPaths)) || (int)pawn.Drawer.renderer.CurRotDrawMode == 4)
		{
			return;
		}
		object obj = pawn.story?.bodyType?.bodyNakedGraphicPath;
		if (obj == null || __result.path.Contains("EmptyImage"))
		{
			return;
		}
		string bodyNakedGraphicPath = pawn.story.bodyType.bodyNakedGraphicPath;
		if ((int)pawn.gender != 1 && bodyNakedGraphicPath != null && !bodyNakedGraphicPath.Contains("_Female") && (bodyNakedGraphicPath.Contains("_Thin") || bodyNakedGraphicPath.Contains("_Fat") || bodyNakedGraphicPath.Contains("_Hulk")))
		{
			string text = bodyNakedGraphicPath + "_Female";
			if ((Object)(object)ContentFinder<Texture2D>.Get(text + "_south", false) != (Object)null)
			{
				Shader val = ((PawnRenderNode)__instance).ShaderFor(pawn);
				__result = GraphicDatabase.Get<Graphic_Multi>(text, val, Vector2.one, ((PawnRenderNode)__instance).ColorFor(pawn));
			}
		}
	}
}
