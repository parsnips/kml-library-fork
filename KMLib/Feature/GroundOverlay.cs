namespace KMLib.Feature
{
    using System.Xml.Serialization;
    using Core.Xml;
    using Abstract;

    public class GroundOverlay : AOverlay
    {
        [XmlIgnore] public bool altitudeModeSpecified;
        [XmlIgnore] public bool altitudeSpecified;
        private LatLonBox m_LatLonBox;
        private double m_altitude;

        private AltitudeMode m_altitudeMode = AltitudeMode.clampedToGround;

        public double altitude
        {
            get { return m_altitude; }
            set
            {
                m_altitude = value;
                altitudeSpecified = true;
            }
        }

        public AltitudeMode altitudeMode
        {
            get { return m_altitudeMode; }
            set
            {
                m_altitudeMode = value;
                altitudeModeSpecified = true;
            }
        }

        public LatLonBox LatLonBox
        {
            get
            {
                if (!XMLSerializeManager.Serializing && m_LatLonBox == null)
                {
                    m_LatLonBox = new LatLonBox();
                }

                return m_LatLonBox;
            }

            set { m_LatLonBox = value; }
        }
    }
}

/*
  <!-- specific to GroundOverlay -->
  <altitude>0</altitude>                    <!-- double -->
  <altitudeMode>clampToGround</altitudeMode>
     <!-- kml:altitudeModeEnum: clampToGround or absolute --> 
  <LatLonBox>
    <north>...</north>                      <! kml:angle90 -->
    <south>...</south>                      <! kml:angle90 -->
    <east>...</east>                        <! kml:angle180 -->
    <west>...</west>                        <! kml:angle180 -->
    <rotation>0</rotation>                  <! kml:angle180 -->
  </LatLonBox>
 */