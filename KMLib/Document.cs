namespace KMLib
{
    using System.Xml.Serialization;

    public class ADoc
    {
        public Style Style { get; set; }

        public string description { get; set; }

        [XmlElement("visibility")]
        public IntBool Visible { get; set; }

        public string name { get; set; }

        public LookAt LookAt { get; set; }
    }
}