namespace KMLib
{
    using System.Xml.Serialization;
    using Core.Geometry;
    using Abstract;

    public class LinearRing : AGeometry
    {
        private Coordinates m_Coordinates = new Coordinates();

        [XmlElement("coordinates")]
        public Coordinates Coordinates
        {
            get { return m_Coordinates; }
            set { m_Coordinates = value; }
        }

        public void CloseRing()
        {
            if (Coordinates == null || Coordinates.Count < 3)
            {
                return;
            }

            if (Coordinates[0].SameCoordsAs(Coordinates[Coordinates.Count - 1]))
            {
                return;
            }

            Coordinates.Add(new Point3D(Coordinates[0]));
        }
    }
}