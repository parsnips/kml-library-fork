namespace KMLib.Abstract
{
    using System.Xml.Serialization;
    using Core.Xml;

    public abstract class AOverlay : AFeature
    {
        [XmlIgnore] public bool drawOrderSpecified;

        private Icon m_Icon;
        private int m_drawOrder;
        public ColorKML color { get; set; }

        public int drawOrder
        {
            get { return m_drawOrder; }
            set
            {
                m_drawOrder = value;
                drawOrderSpecified = true;
            }
        }

        public Icon Icon
        {
            get
            {
                if (!XMLSerializeManager.Serializing && m_Icon == null)
                {
                    m_Icon = new Icon();
                }

                return m_Icon;
            }

            set { m_Icon = value; }
        }
    }

    /*
  <!-- specific to Overlay -->
  <color>ffffffff</color>                   <!-- kml:color -->
  <drawOrder>0</drawOrder>                  <!-- int -->
  <Icon>
    <href>...</href>
  </Icon>
<!-- /Overlay -->*/
}