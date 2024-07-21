using System.Xml.Serialization;

namespace ExtraManager.Models
{
    public class Extra
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }
    }
}
