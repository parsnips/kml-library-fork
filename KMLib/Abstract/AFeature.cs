namespace KMLib.Abstract
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Core.Xml;

    public abstract class AFeature : AObject
    {
        [XmlIgnore] private bool StyleSpecified;
        [XmlIgnore] private bool TimeSpanSpecified;
        [XmlIgnore] private bool TimeStampSpecified;
        private Region m_Region;
        private List<Style> m_Style;
        private TimeSpan m_TimeSpan;
        private TimeStamp m_TimeStamp;
        public string name { get; set; }

        public IntBool open { get; set; }

        public IntBool visibility { get; set; }

        public Snippet Snippet { get; set; }

        public string phoneNumber { get; set; }

        public string description { get; set; }

        public LookAt LookAt { get; set; }

        public string styleUrl { get; set; }

        public Region Region
        {
            get
            {
                if (!XMLSerializeManager.Serializing && m_Region == null)
                {
                    m_Region = new Region();
                }

                return m_Region;
            }

            set { m_Region = value; }
        }

        public Metadata Metadata { get; set; }

        [XmlElement("TimeStamp")]
        public TimeStamp TimeStamp
        {
            get { return m_TimeStamp; }
            set
            {
                m_TimeStamp = value;
                TimeStampSpecified = true;
            }
        }

        [XmlElement("TimeSpan")]
        public TimeSpan TimeSpan
        {
            get { return m_TimeSpan; }
            set
            {
                m_TimeSpan = value;
                TimeSpanSpecified = true;
            }
        }

        [XmlElement(ElementName = "Style", Type = typeof (Style))]
        public List<Style> Lists
        {
            get { return m_Style; }
            set { m_Style = value; }
        }

        public void AddStyle(Style style)
        {
            if (m_Style == null)
            {
                m_Style = new List<Style>();
            }

            m_Style.Add(style);
        }
    }
}

/*
<name>...</name>                      <!-- string -->
<visibility>1</visibility>            <!-- boolean -->
<open>1</open>                        <!-- boolean -->
<address>...</address>                <!-- string -->
<AddressDetails xmlns="urn:oasis:names:tc:ciq:xsdschema:xAL:2.0">...
  </AddressDetails>                 <!-- string -->
<phoneNumber>...</phoneNumber>        <!-- string -->
<Snippet maxLines="2">...</Snippet>   <!-- string -->
<description>...</description>        <!-- string -->
<LookAt>...</LookAt>
<TimePrimitive>...</TimePrimitive>
<styleUrl>...</styleUrl>              <!-- anyURI -->
<StyleSelector>...</StyleSelector>
<Region>...</Region>
<Metadata>...</Metadata>
 */