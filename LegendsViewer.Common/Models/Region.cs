using System.Xml.Linq;
using System.Xml.Serialization;
using LegendsViewer.Common.Interfaces;
using LegendsViewer.Common.Utilities;

namespace LegendsViewer.Common.Models;

public class Region : IRegion
{
    [XmlElement("id")]
    public int Id { get; set; }
    [XmlElement("name")]
    public string? Name { get; set; }
    [XmlElement("type")]
    public RegionType? Type { get; set; }
    [XmlElement("coords")]
    public List<(int, int)>? Coords { get; set; }
    [XmlElement("evilness")]
    public Alignment? Alignment { get; set; }
    
    public static Region FromXml((XElement normal, XElement plus) region)
    {
        var id = region.normal.Element("id")?.Value;
        var name = region.normal.Element("name")?.Value;
        var type = region.normal.Element("type")?.Value;
        var coords = region.plus.Element("coords")?.Value;
        var alignment = region.plus.Element("evilness")?.Value;
        
        return new Region
        {
            Id = id != null ? int.Parse(id) : throw new ArgumentNullException(nameof(id), "Region ID is required"),
            Name = name,
            Type = type != null ? Enum.Parse<RegionType>(type, true) : null,
            Coords = coords != null ? ParseCoords(coords) : null,
            Alignment = alignment != null ? ParseAlignment(alignment) : null
        };
    }
    
    private static List<(int, int)> ParseCoords(string coords)
    {
        var coordsList = new List<(int, int)>();
        var coordsArray = coords.Split('|');
        
        foreach (var coordString in coordsArray)
        {
            if (string.IsNullOrEmpty(coordString)) continue;
            
            var coordinate = coordString.Split(',');
            coordsList.Add((int.Parse(coordinate[0]), int.Parse(coordinate[1])));
        }
        
        return coordsList;
    }
    
    private static Alignment ParseAlignment(string alignment)
    {
        if (Enum.TryParse<Alignment>(alignment, true, out var result))
        {
            return result;
        }

        return Utilities.Alignment.Unknown;
    }
}