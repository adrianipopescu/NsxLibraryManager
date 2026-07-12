namespace NsxLibraryManager.Core.FileLoading;

/// <summary>
/// Infers an eShop country code from the set of languages a title's NACP declares, used only
/// as a fallback when titledb has no region for the title. Only maps languages to countries
/// that titledb actually uses in <c>Title.Region</c> (ISO alpha-2), so the inferred value is
/// never a code no catalogued title has; a language with no titledb country resolves to null.
/// </summary>
public static class RegionResolver
{
    // LibHac ApplicationTitleLanguage index -> representative eShop country present in titledb.
    // Languages whose obvious country titledb never uses (Latin-American Spanish -> MX,
    // Italian -> IT, Dutch -> NL, Portuguese -> PT) are intentionally absent: they fall through
    // to another supported language or to null.
    private static readonly Dictionary<int, string> LanguageToCountry = new()
    {
        [0] = "US",   // AmericanEnglish
        [1] = "GB",   // BritishEnglish
        [2] = "JP",   // Japanese
        [3] = "FR",   // French
        [4] = "DE",   // German
        [6] = "ES",   // Spanish
        [9] = "CA",   // CanadianFrench
        [11] = "RU",  // Russian
        [12] = "KR",  // Korean
        [13] = "HK",  // TraditionalChinese
        [14] = "CN",  // SimplifiedChinese
    };

    // When several languages are present, the primary storefront is picked in this order:
    // American English (world/NA releases ship it) -> Japanese -> single-country Asian markets
    // -> European languages. Returns that language's country.
    // ponytail: a language can span countries (French -> FR/BE/CH); resolves to its largest
    // titledb market. Good enough as a fallback that only fires when titledb had nothing.
    private static readonly int[] MarketPriority = { 0, 2, 12, 13, 14, 1, 4, 3, 6, 11, 9 };

    public static string? ResolveRegion(IReadOnlyCollection<int> supportedLanguages)
    {
        foreach (var language in MarketPriority)
        {
            if (supportedLanguages.Contains(language) && LanguageToCountry.TryGetValue(language, out var country))
                return country;
        }
        return null;
    }
}
