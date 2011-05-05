namespace KMLib
{
    using System.Xml.Serialization;
    using Abstract;

    public class Polygon : AGeometry
    {
        private BoundaryIs m_OuterBoundaryIs = new BoundaryIs(); // --required for all polygons

        [XmlElement("outerBoundaryIs")]
        public BoundaryIs OuterBoundaryIs
        {
            get { return m_OuterBoundaryIs; }
            set { m_OuterBoundaryIs = value; }
        }

        [XmlElement("innerBoundaryIs")]
        public BoundaryIs InnerBoundaryIs { get; set; }
    }
}