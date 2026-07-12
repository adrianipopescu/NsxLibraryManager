namespace NsxLibraryManager.Core.FileLoading;

/// <summary>
/// Infers a 2-char region code from the set of languages a title's NACP declares
/// (via the language index of each populated <c>ApplicationControlProperty.Title</c> entry).
/// Used only as a fallback when titledb has no region for the title.
/// </summary>
public static class RegionResolver
{
    // LibHac ApplicationTitleLanguage indices:
    // 0 AmericanEnglish, 1 BritishEnglish, 2 Japanese, 3 French, 4 German,
    // 5 LatinAmericanSpanish, 6 Spanish, 7 Italian, 8 Dutch, 9 CanadianFrench,
    // 10 Portuguese, 11 Russian, 12 Korean, 13 TraditionalChinese, 14 SimplifiedChinese
    private static readonly int[] Americas = { 0, 5, 9 };
    private static readonly int[] Europe = { 1, 3, 4, 6, 7, 8, 10, 11 };

    // ponytail: language→region is a heuristic with a known ceiling — an English-only EU
    // title and a US title are indistinguishable from the NACP alone. Good enough as a
    // titledb fallback. Order matters: Americas English wins for world releases (they ship
    // American English), Japanese-only maps to JP, then Europe, then Asian-only markets.
    public static string? ResolveRegion(IReadOnlyCollection<int> supportedLanguages)
    {
        if (supportedLanguages.Count == 0) return null;
        if (supportedLanguages.Any(l => Americas.Contains(l))) return "US";
        if (supportedLanguages.Contains(2)) return "JP";
        if (supportedLanguages.Any(l => Europe.Contains(l))) return "GB";
        if (supportedLanguages.Contains(12)) return "KR";
        if (supportedLanguages.Contains(13) || supportedLanguages.Contains(14)) return "HK";
        return null;
    }
}
