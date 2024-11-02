namespace ExtraManager.Engine.Data
{
    public class Extra
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }
    }
}
