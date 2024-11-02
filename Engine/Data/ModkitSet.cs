namespace ExtraManager.Engine.Data
{
    [XmlRoot("Modkits")]
    public class ModkitSet
    {
        [XmlElement("Modkit")]
        public List<Modkit> Modkits { get; set; }
    }
}
