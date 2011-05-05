namespace KMLib
{
    using System.Xml.Serialization;

    public abstract class ALatLonBox
    {
        public ALatLonBox()
        {
        }

        public ALatLonBox(double n, double s, double e, double w)
        {
            north = n;
            south = s;
            east = e;
            west = w;
        }

        public ALatLonBox(ALatLonBox box)
        {
            north = box.north;
            south = box.south;
            east = box.east;
            west = box.west;
        }

        public double north { get; set; }

        public double south { get; set; }

        public double east { get; set; }

        public double west { get; set; }
    }

    public class LatLonBox : ALatLonBox
    {
        private double m_rotation;
        [XmlIgnore] public bool rotationSpecified;

        public LatLonBox()
        {
        }

        public LatLonBox(double n, double s, double e, double w)
            : base(n, s, e, w)
        {
        }

        public LatLonBox(ALatLonBox box)
            : base(box)
        {
        }

        public double rotation
        {
            get { return m_rotation; }
            set
            {
                m_rotation = value;
                rotationSpecified = true;
            }
        }
    }

    public class LatLonAltBox : ALatLonBox
    {
        private double m_maxAltitude;
        private double m_minAltitude;
        [XmlIgnore] public bool maxAltitudeSpecified;
        [XmlIgnore] public bool minAltitudeSpecified;

        public LatLonAltBox()
        {
        }

        public LatLonAltBox(double n, double s, double e, double w)
            : base(n, s, e, w)
        {
        }

        public LatLonAltBox(ALatLonBox box)
            : base(box)
        {
        }

        public double minAltitude
        {
            get { return m_minAltitude; }
            set
            {
                m_minAltitude = value;
                minAltitudeSpecified = true;
            }
        }

        public double maxAltitude
        {
            get { return m_maxAltitude; }
            set
            {
                m_maxAltitude = value;
                maxAltitudeSpecified = true;
            }
        }
    }

    /*

    <LatLonAltBox>
      <north>43.374</north>
      <south>42.983</south>
      <east>-0.335</east>
      <west>-1.423</west>
      <minAltitude>0</minAltitude>
      <maxAltitude>0</maxAltitude>
    </LatLonAltBox>
 
     <LatLonBox>
    <north>...</north>                      <! kml:angle90 -->
    <south>...</south>                      <! kml:angle90 -->
    <east>...</east>                        <! kml:angle180 -->
    <west>...</west>                        <! kml:angle180 -->
    <rotation>0</rotation>                  <! kml:angle180 -->
  </LatLonBox>
     */
}