using System.Xml.Linq;
using System.Xml.Serialization;
using LegendsViewer.Common.Interfaces;

namespace LegendsViewer.Common.Models;

[XmlRoot("df_world")]
public class DfWorld
{
    [XmlElement("name")]
    public string? Name { get; set; }
    [XmlElement("altname")]
    public string? AltName { get; set; }
    [XmlElement("landmasses")]
    public List<Landmass>? Landmasses { get; set; } = [];
    [XmlElement("regions")]
    [XmlArrayItem("region")]
    public List<IRegion> Regions { get; set; } = [];
    [XmlElement("underground_regions")]
    [XmlArrayItem("underground_region")]
    public List<IRegion> UndergroundRegions { get; set; } = [];
    
    public static DfWorld FromXml(XElement legendsRoot, XElement legendsPlusRoot)
    {
        var world = new DfWorld
        {
            Name = legendsPlusRoot.Element("name")?.Value,
            AltName = legendsPlusRoot.Element("altname")?.Value
        };
        
        var normalRegions = legendsRoot.Element("regions")?.Elements("region");
        var plusRegions = legendsPlusRoot.Element("regions")?.Elements("region");
        
        if (normalRegions == null)
        {
            throw new ArgumentNullException(nameof(legendsRoot), "Regions are required");
        }
        
        if (plusRegions == null)
        {
            throw new ArgumentNullException(nameof(legendsPlusRoot), "Regions are required");
        }
        
        var regions = normalRegions.Zip(plusRegions, 
            (normal, plus) => (normal, plus));
        
        foreach (var region in regions)
        {
            world.Regions.Add(Region.FromXml(region));
        }
        
        var landmasses = legendsPlusRoot.Element("landmasses")?.Elements("landmass");

        foreach (var landmass in landmasses ?? [])
        {
            world.Landmasses?.Add(Landmass.FromXml(landmass));
        }
        
        var undergroundRegions = legendsRoot.Element("underground_regions")?.Elements("underground_region");
        foreach (var undergroundRegion in undergroundRegions ?? [])
        {
            world.UndergroundRegions.Add(UndergroundRegion.FromXml(undergroundRegion));
        }
        
        return world;
    }
}