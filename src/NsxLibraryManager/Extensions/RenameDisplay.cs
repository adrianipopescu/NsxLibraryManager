using NsxLibraryManager.Shared.Enums;

namespace NsxLibraryManager.Extensions;

public static class RenameDisplay
{
    // ISO 3166 alpha-2 -> regional-indicator flag emoji (e.g. "US" -> 🇺🇸). Empty when unknown.
    public static string RegionFlag(string? region)
    {
        if (string.IsNullOrWhiteSpace(region) || region.Length != 2) return string.Empty;
        var r = region.ToUpperInvariant();
        if (r[0] is < 'A' or > 'Z' || r[1] is < 'A' or > 'Z') return string.Empty;
        return char.ConvertFromUtf32(0x1F1E6 + (r[0] - 'A')) + char.ConvertFromUtf32(0x1F1E6 + (r[1] - 'A'));
    }

    public static string StatusEmoji(RenameStatus status) => status switch
    {
        RenameStatus.Ready => "✅",      // ✅ ok
        RenameStatus.Warning => "⚠️", // ⚠️ warning
        RenameStatus.Error => "🛑", // 🛑 stop
        _ => string.Empty
    };
}
