using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExtraManager.Models
{
    [XmlRoot("Vehicle")]
    public class VehicleData
    {
        // The name attribute specifies the vehicle's model name in the game.
        [XmlAttribute("name")]
        public string Name { get; set; }

        // The ModKit element references the modkit ID associated with this vehicle. 
        [XmlElement("ModKit")]
        public int ModKit { get; set; }

        // List of extras available for the vehicle.
        [XmlArray("Extras")]
        [XmlArrayItem("Extra")]
        public List<Extra> Extras { get; set; }
    }
}
