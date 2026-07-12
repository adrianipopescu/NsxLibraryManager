using NsxLibraryManager.Core.FileLoading;
using Xunit;

namespace TestUnit.FileLoading;

public class RegionResolverTests
{
    // Languages that map to a country titledb actually uses.
    [Theory]
    [InlineData(0, "US")]   // AmericanEnglish
    [InlineData(1, "GB")]   // BritishEnglish
    [InlineData(2, "JP")]   // Japanese
    [InlineData(3, "FR")]   // French
    [InlineData(4, "DE")]   // German
    [InlineData(6, "ES")]   // Spanish
    [InlineData(9, "CA")]   // CanadianFrench
    [InlineData(11, "RU")]  // Russian
    [InlineData(12, "KR")]  // Korean
    [InlineData(13, "HK")]  // TraditionalChinese
    [InlineData(14, "CN")]  // SimplifiedChinese
    public void ResolveRegion_SingleLanguage_MapsToTitledbCountry(int language, string expected)
    {
        Assert.Equal(expected, RegionResolver.ResolveRegion(new[] { language }));
    }

    // Languages whose obvious country titledb never uses resolve to null, not a phantom code.
    [Theory]
    [InlineData(5)]   // LatinAmericanSpanish -> MX (not in titledb)
    [InlineData(7)]   // Italian -> IT (not in titledb)
    [InlineData(8)]   // Dutch -> NL (not in titledb)
    [InlineData(10)]  // Portuguese -> PT (not in titledb)
    public void ResolveRegion_LanguageWithoutTitledbCountry_ReturnsNull(int language)
    {
        Assert.Null(RegionResolver.ResolveRegion(new[] { language }));
    }

    [Fact]
    public void ResolveRegion_Empty_ReturnsNull()
        => Assert.Null(RegionResolver.ResolveRegion(System.Array.Empty<int>()));

    [Fact]
    public void ResolveRegion_UnknownIndex_ReturnsNull()
        => Assert.Null(RegionResolver.ResolveRegion(new[] { 99 }));

    // Market priority + fall-through past unmapped languages.
    [Theory]
    [InlineData(new[] { 2, 0 }, "US")]                 // world release ships American English -> US, not JP
    [InlineData(new[] { 2, 4 }, "JP")]                 // Japanese + German, no American English -> JP
    [InlineData(new[] { 4, 12 }, "KR")]                // German + Korean -> Asian market wins over Europe
    [InlineData(new[] { 12, 13 }, "KR")]               // Korean + Chinese -> Korea before China
    [InlineData(new[] { 3, 4 }, "DE")]                 // French + German, no English -> German (larger EU market)
    [InlineData(new[] { 5, 6 }, "ES")]                 // Latin-Am Spanish (unmapped) falls through to Spanish -> ES
    [InlineData(new[] { 7, 4 }, "DE")]                 // Italian (unmapped) falls through to German -> DE
    [InlineData(new[] { 7, 8, 10 }, null)]             // only unmapped languages -> null
    [InlineData(new[] { 0, 1, 2, 3, 4, 5, 6 }, "US")]  // broad multi-language world release -> US
    public void ResolveRegion_MultipleLanguages_PicksPrimaryTitledbMarket(int[] languages, string? expected)
    {
        Assert.Equal(expected, RegionResolver.ResolveRegion(languages));
    }
}
