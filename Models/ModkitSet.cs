using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExtraManager.Models
{
    [XmlRoot("Modkits")]
    public class ModkitSet
    {
        [XmlElement("Modkit")]
        public List<Modkit> Modkits { get; set; }
    }
}
