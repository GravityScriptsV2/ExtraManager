namespace ExtraManager.Data
{
    public class Modkit
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlElement("WindowTint")]
        public int? WindowTint { get; set; }
    }
}
