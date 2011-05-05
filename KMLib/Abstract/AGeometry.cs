namespace KMLib.Abstract
{
    using System.Xml.Serialization;

    public abstract class AGeometry
    {
        [XmlIgnore] public bool AltitudeModeSpecified;
        private AltitudeMode m_altitudeMode = AltitudeMode.relativeToGround;

        [XmlElement("tessellate")]
        public IntBool Tessellate { get; set; }

        [XmlElement("extrude")]
        public IntBool Extrude { get; set; }

        [XmlElement("altitudeMode")]
        public AltitudeMode AltitudeMode
        {
            get { return m_altitudeMode; }
            set
            {
                m_altitudeMode = value;
                AltitudeModeSpecified = true;
            }
        }
    }
}