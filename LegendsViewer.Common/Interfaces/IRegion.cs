using LegendsViewer.Common.Utilities;

namespace LegendsViewer.Common.Interfaces;

public interface IRegion
{
    int Id { get; set; }
    RegionType? Type { get; set; }
}