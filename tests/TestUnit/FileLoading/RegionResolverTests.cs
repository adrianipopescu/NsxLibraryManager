using NsxLibraryManager.Core.FileLoading;
using Xunit;

namespace TestUnit.FileLoading;

public class RegionResolverTests
{
    // One language present, mapped to its region. Covers every ApplicationTitleLanguage index.
    [Theory]
    [InlineData(0, "US")]   // AmericanEnglish
    [InlineData(5, "US")]   // LatinAmericanSpanish  -> Americas
    [InlineData(9, "US")]   // CanadianFrench        -> Americas (French, but not Europe)
    [InlineData(2, "JP")]   // Japanese
    [InlineData(1, "GB")]   // BritishEnglish        -> Europe (English, but not US)
    [InlineData(3, "GB")]   // French
    [InlineData(4, "GB")]   // German
    [InlineData(6, "GB")]   // Spanish (Spain)
    [InlineData(7, "GB")]   // Italian
    [InlineData(8, "GB")]   // Dutch
    [InlineData(10, "GB")]  // Portuguese
    [InlineData(11, "GB")]  // Russian
    [InlineData(12, "KR")]  // Korean
    [InlineData(13, "HK")]  // TraditionalChinese
    [InlineData(14, "HK")]  // SimplifiedChinese
    public void ResolveRegion_SingleLanguage_MapsToRegion(int language, string expected)
    {
        Assert.Equal(expected, RegionResolver.ResolveRegion(new[] { language }));
    }

    // Inputs that don't resolve to any region.
    [Fact]
    public void ResolveRegion_Empty_ReturnsNull()
        => Assert.Null(RegionResolver.ResolveRegion(System.Array.Empty<int>()));

    [Fact]
    public void ResolveRegion_UnknownIndex_ReturnsNull()
        => Assert.Null(RegionResolver.ResolveRegion(new[] { 99 }));

    // Order-dependent combinations — the priority is Americas > Japan > Europe > Korea > China.
    [Theory]
    [InlineData(new[] { 2, 0 }, "US")]                 // world release ships American English -> US, not JP
    [InlineData(new[] { 2, 4 }, "JP")]                 // Japanese + German, no Americas -> JP beats Europe
    [InlineData(new[] { 4, 12 }, "GB")]                // German + Korean -> Europe beats Korea
    [InlineData(new[] { 12, 13 }, "KR")]               // Korean + Chinese -> Korea beats China
    [InlineData(new[] { 9, 3 }, "US")]                 // CanadianFrench (Americas) + French (Europe) -> US
    [InlineData(new[] { 99, 4 }, "GB")]                // unknown index ignored, German -> GB
    [InlineData(new[] { 0, 1, 2, 3, 4, 5, 6 }, "US")]  // broad multi-language world release -> US
    public void ResolveRegion_MultipleLanguages_PrioritisesByRegion(int[] languages, string expected)
    {
        Assert.Equal(expected, RegionResolver.ResolveRegion(languages));
    }
}
