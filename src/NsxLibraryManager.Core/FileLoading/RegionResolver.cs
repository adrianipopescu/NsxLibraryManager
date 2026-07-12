namespace NsxLibraryManager.Core.FileLoading;

/// <summary>
/// Infers an eShop country code (ISO alpha-2, the vocabulary titledb stores in
/// <c>Title.Region</c>) from the set of languages a title's NACP declares. Used only as a
/// fallback when titledb has no region for the title.
/// </summary>
public static class RegionResolver
{
    // LibHac ApplicationTitleLanguage index -> the representative eShop country for that language.
    private static readonly Dictionary<int, string> LanguageToCountry = new()
    {
        [0] = "US",   // AmericanEnglish
        [1] = "GB",   // BritishEnglish
        [2] = "JP",   // Japanese
        [3] = "FR",   // French
        [4] = "DE",   // German
        [5] = "MX",   // LatinAmericanSpanish
        [6] = "ES",   // Spanish
        [7] = "IT",   // Italian
        [8] = "NL",   // Dutch
        [9] = "CA",   // CanadianFrench
        [10] = "PT",  // Portuguese
        [11] = "RU",  // Russian
        [12] = "KR",  // Korean
        [13] = "HK",  // TraditionalChinese
        [14] = "CN",  // SimplifiedChinese
    };

    // When several languages are present, the primary storefront is picked in this order:
    // American English (world/NA releases ship it) -> Japanese -> the single-country Asian
    // markets -> European languages. Returns that language's country.
    // ponytail: a multi-country language (French -> FR/BE/CH, Latin-American Spanish -> MX/AR)
    // resolves to its largest market; the exact eShop country a multi-language title was filed
    // under isn't recoverable from the NACP. Good enough as a titledb fallback.
    private static readonly int[] MarketPriority = { 0, 2, 12, 13, 14, 1, 4, 3, 6, 7, 8, 10, 11, 5, 9 };

    public static string? ResolveRegion(IReadOnlyCollection<int> supportedLanguages)
    {
        foreach (var language in MarketPriority)
        {
            if (supportedLanguages.Contains(language))
                return LanguageToCountry[language];
        }
        return null;
    }
}
