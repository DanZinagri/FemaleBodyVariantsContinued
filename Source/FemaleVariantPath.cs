using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace FemaleBodyVariants;

// Resolves "<path>_Female" for a Thin, Fat or Hulk body path when that texture exists.
//
// The answer is remembered per base path for the session. A ContentFinder miss walks every
// running mod's content, then Unity's Resources folder, then every mod asset bundle, which
// costs milliseconds per probe on a large mod list. The fur probes miss for most pawns
// because this mod ships only the naked body variants, and the lookup repeats on every
// render tree rebuild. The set of textures is fixed after startup, so caching is safe.
// Main thread only, same as ContentFinder itself.
public static class FemaleVariantPath
{
	private static readonly Dictionary<string, string> cache = new Dictionary<string, string>();

	public static bool TryGet(string basePath, out string femalePath)
	{
		if (cache.TryGetValue(basePath, out femalePath))
		{
			return femalePath != null;
		}
		femalePath = null;
		if (!basePath.Contains("_Female")
			&& (basePath.Contains("_Thin") || basePath.Contains("_Fat") || basePath.Contains("_Hulk")))
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
}
