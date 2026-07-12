using LibHac.Ncm;
using LibHac.Tools.FsSystem.NcaUtils;

namespace NsxLibraryManager.Core.FileLoading.Interface;

public interface IContent
{
    public ContentMetaType Type { get;}

    public string TitleId { get; }
    
    public string Name { get; set; }
    public string Publisher { get; set; }

    // Region inferred from the NACP supported-language flags; null when it can't be determined.
    public string? Region { get; set; }

    public string ApplicationTitleId { get; }

    public string PatchTitleId { get; }

    public TitleVersion MinimumApplicationVersion { get; }

    //public NacpData? NacpData { get; set; }

    public TitleVersion Version { get; }

    public int PatchNumber { get; }
    
    public byte[]? Icon { get; set; }
}