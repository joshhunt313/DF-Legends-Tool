using System.Xml.Linq;

namespace LegendsViewer.Common.Models;

public class Landmass
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public (int, int)? Coord1 { get; set; }
    public (int, int)? Coord2 { get; set; }
    
    public static Landmass FromXml(XElement plus)
    {
        var id = plus.Element("id")?.Value;
        var name = plus.Element("name")?.Value;
        var coord1 = plus.Element("coord_1")?.Value;
        var coord2 = plus.Element("coord_2")?.Value;
        
        return new Landmass
        {
            Id = id != null ? int.Parse(id) : throw new ArgumentNullException(nameof(id), "Landmass ID is required"),
            Name = name,
            Coord1 = coord1 != null ? ParseCoord(coord1) : null,
            Coord2 = coord2 != null ? ParseCoord(coord2) : null
        };
    }
    
    public static (int, int) ParseCoord(string coord)
    {
        var coordinate = coord.Split(',');
        return (int.Parse(coordinate[0]), int.Parse(coordinate[1]));
    }
}