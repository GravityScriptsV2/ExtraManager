using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExtraManager.Models
{
    [XmlRoot("Vehicle")]
    public class VehicleData
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("Extras")]
        [XmlArrayItem("Extra")]
        public List<Extra> Extras { get; set; }
    }
}
