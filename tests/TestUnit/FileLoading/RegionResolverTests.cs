using NsxLibraryManager.Core.FileLoading;
using Xunit;

namespace TestUnit.FileLoading;

public class RegionResolverTests
{
    [Theory]
    [InlineData(new[] { 0 }, "US")]        // American English
    [InlineData(new[] { 0, 2, 4 }, "US")]  // world release that ships American English
    [InlineData(new[] { 5 }, "US")]        // Latin-American Spanish → Americas
    [InlineData(new[] { 2 }, "JP")]        // Japanese only
    [InlineData(new[] { 1, 4 }, "GB")]     // British English + German
    [InlineData(new[] { 12 }, "KR")]       // Korean
    [InlineData(new[] { 13 }, "HK")]       // Traditional Chinese
    [InlineData(new[] { 14 }, "HK")]       // Simplified Chinese
    public void ResolveRegion_MapsSupportedLanguagesToRegion(int[] languages, string expected)
    {
        Assert.Equal(expected, RegionResolver.ResolveRegion(languages));
    }

    [Fact]
    public void ResolveRegion_ReturnsNull_WhenNoLanguages()
    {
        Assert.Null(RegionResolver.ResolveRegion(System.Array.Empty<int>()));
    }
}
