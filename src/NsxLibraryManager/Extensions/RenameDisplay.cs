using NsxLibraryManager.Shared.Dto;
using NsxLibraryManager.Shared.Enums;

namespace NsxLibraryManager.Extensions;

public static class RenameDisplay
{
    // Hover text for the status emoji: the error/warning detail if present, else the state name.
    public static string StatusTooltip(RenameTitleDto dto) =>
        !string.IsNullOrEmpty(dto.ErrorMessage)
            ? dto.ErrorMessage
            : dto.Status switch
            {
                RenameStatus.Ready => "Ready to rename",
                RenameStatus.Warning => "Warning",
                RenameStatus.Error => "Error",
                _ => string.Empty
            };

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
