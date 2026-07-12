using NsxLibraryManager.Core.FileLoading;
using Xunit;

namespace TestUnit.FileLoading;

public class RegionResolverTests
{
    // One language present, mapped to its eShop country. Covers every ApplicationTitleLanguage index.
    [Theory]
    [InlineData(0, "US")]   // AmericanEnglish
    [InlineData(1, "GB")]   // BritishEnglish
    [InlineData(2, "JP")]   // Japanese
    [InlineData(3, "FR")]   // French
    [InlineData(4, "DE")]   // German
    [InlineData(5, "MX")]   // LatinAmericanSpanish
    [InlineData(6, "ES")]   // Spanish
    [InlineData(7, "IT")]   // Italian
    [InlineData(8, "NL")]   // Dutch
    [InlineData(9, "CA")]   // CanadianFrench
    [InlineData(10, "PT")]  // Portuguese
    [InlineData(11, "RU")]  // Russian
    [InlineData(12, "KR")]  // Korean
    [InlineData(13, "HK")]  // TraditionalChinese
    [InlineData(14, "CN")]  // SimplifiedChinese
    public void ResolveRegion_SingleLanguage_MapsToCountry(int language, string expected)
    {
        Assert.Equal(expected, RegionResolver.ResolveRegion(new[] { language }));
    }

    // Inputs that don't resolve to any country.
    [Fact]
    public void ResolveRegion_Empty_ReturnsNull()
        => Assert.Null(RegionResolver.ResolveRegion(System.Array.Empty<int>()));

    [Fact]
    public void ResolveRegion_UnknownIndex_ReturnsNull()
        => Assert.Null(RegionResolver.ResolveRegion(new[] { 99 }));

    // Market priority for multi-language titles: American English > Japanese > Asian
    // single-country markets > European languages.
    [Theory]
    [InlineData(new[] { 2, 0 }, "US")]                 // world release ships American English -> US, not JP
    [InlineData(new[] { 2, 4 }, "JP")]                 // Japanese + German, no American English -> JP
    [InlineData(new[] { 4, 12 }, "KR")]                // German + Korean -> Asian market wins over Europe
    [InlineData(new[] { 12, 13 }, "KR")]               // Korean + Chinese -> Korea before China
    [InlineData(new[] { 3, 4 }, "DE")]                 // French + German, no English -> German (larger EU market)
    [InlineData(new[] { 99, 3 }, "FR")]                // unknown index ignored, French -> FR
    [InlineData(new[] { 0, 1, 2, 3, 4, 5, 6 }, "US")]  // broad multi-language world release -> US
    public void ResolveRegion_MultipleLanguages_PicksPrimaryMarket(int[] languages, string expected)
    {
        Assert.Equal(expected, RegionResolver.ResolveRegion(languages));
    }
}
