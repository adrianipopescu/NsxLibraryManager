namespace NsxLibraryManager.Shared.Enums;

public enum RenameStatus
{
    Ready,    // matched, no dupe -> ready to rename
    Warning,  // duplicate, or couldn't match a region/titledb entry -> soft, not a failure
    Error     // couldn't build a valid destination name at all
}
