using System.Xml.Linq;
using LegendsViewer.Common.Interfaces;
using LegendsViewer.Common.Utilities;

namespace LegendsViewer.Common.Models;

public class UndergroundRegion : IRegion
{
    public int Id { get; set; }
    public RegionType? Type { get; set; }
    public int Depth { get; set; }
    
    public static UndergroundRegion FromXml(XElement undergroundRegion)
    {
        var id = undergroundRegion.Element("id")?.Value;
        var type = undergroundRegion.Element("type")?.Value;
        var depth = undergroundRegion.Element("depth")?.Value;
        
        return new UndergroundRegion
        {
            Id = id != null ? int.Parse(id) : throw new ArgumentNullException(nameof(id), "Underground region ID is required"),
            Type = type != null ? Enum.Parse<RegionType>(type, true) : null,
            Depth = depth != null ? int.Parse(depth) : throw new ArgumentNullException(nameof(depth), "Underground region depth is required")
        };
    }
}