using NsxLibraryManager.Shared.Enums;

namespace NsxLibraryManager.Shared.Dto;

public class RenameTitleDto
{
    public int Id { get; set; }
    public bool UpdateLibrary { get; set; } = false;
    public required string SourceFileName { get; set; }
    public string? DestinationFileName { get; set; }
    public string? TitleId { get; set; }
    public string? TitleName { get; set; }
    // The resolved region that goes into the destination path (rendered as a flag in the grid).
    public string? Region { get; set; }
    // Traffic-light status derived from Error/Duplicate/unmatched, for the grid indicator.
    public RenameStatus Status { get; set; } = RenameStatus.Ready;
    public bool RenamedSuccessfully { get; set; }
    public bool Error { get; set; }
    public string? ErrorMessage { get; set; }
    // True when the library already holds another file with the same title id (a cross-format
    // dupe the filename check can't catch). DuplicateOf is the path of that existing file.
    public bool Duplicate { get; set; }
    public string? DuplicateOf { get; set; }
}
