using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace FemaleBodyVariants;

// Resolves "<path>_Female" for the body path of a Thin, Fat or Hulk pawn when that texture exists.
//
// Eligibility is decided by the pawn's body type def, not by how the path is spelled, so a
// FurDef entry such as <Fat>.../MerfolkBodies/fat</Fat> qualifies. The vanilla naming is kept
// as a fallback for a mod-defined body type that follows it.
public static class FemaleVariantPath
{
	private static readonly Dictionary<string, string> cache = new Dictionary<string, string>();

	public static bool TryGet(BodyTypeDef bodyType, string basePath, out string femalePath)
	{
		femalePath = null;
		if (basePath == null || !IsSharedBodyType(bodyType, basePath))
		{
			return false;
		}
		if (cache.TryGetValue(basePath, out femalePath))
		{
			return femalePath != null;
		}
		if (!basePath.Contains("_Female"))
		{
			string candidate = basePath + "_Female";
			if (ContentFinder<Texture2D>.Get(candidate + "_south", reportFailure: false) != null)
			{
				femalePath = candidate;
			}
		}
		cache[basePath] = femalePath;
		return femalePath != null;
	}

	// Thin, Fat and Hulk are the body types both genders share, so they are the ones a female
	// variant can exist for. Checked before the cache because one path can serve several body
	// types in a FurDef, and the answer must not leak from an eligible type to an ineligible one.
	private static bool IsSharedBodyType(BodyTypeDef bodyType, string path)
	{
		if (bodyType == BodyTypeDefOf.Thin || bodyType == BodyTypeDefOf.Fat || bodyType == BodyTypeDefOf.Hulk)
		{
			return true;
		}
		return path.Contains("_Thin") || path.Contains("_Fat") || path.Contains("_Hulk");
	}
}
