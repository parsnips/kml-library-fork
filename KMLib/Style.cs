namespace KMLib
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Xml.Serialization;
    using Abstract;

    public class Style : AStyleSelector
    {
        [XmlIgnore] private bool StyleSpecified;
        private List<Style> m_Style;

        [XmlAttribute("id")]
        public string Id { get; set; }

        // [XmlElement(ElementName = "BallonStyle", Type = typeof(BallonStyle))]
        [XmlElement(ElementName = "IconStyle", Type = typeof (IconStyle))]
        [XmlElement(ElementName = "LabelStyle", Type = typeof (LabelStyle))]
        [XmlElement(ElementName = "LineStyle", Type = typeof (LineStyle))]
// [XmlElement(ElementName = "ListStyle", Type = typeof(ListStyle))]
        [XmlElement(ElementName = "PolyStyle", Type = typeof (PolyStyle))]
        public List<Style> Lists
        {
            get { return m_Style; }
            set { m_Style = value; }
        }

        public void Add(Style style)
        {
            if (m_Style == null)
            {
                m_Style = new List<Style>();
            }

            m_Style.Add(style);
        }
    }

    public class ColorStyle : Style
    {
        #region ColorMode enum

        public enum ColorMode
        {
            normal, 
            random
        } ;

        #endregion

        [XmlIgnore] private bool colorModeSpecified;
        private ColorMode m_colorMode;

        public ColorStyle(Color color)
        {
            Color = color;
        }

        public ColorStyle()
        {
        }

        [XmlElement("color")]
        public ColorKML Color { get; set; }

        [XmlElement("colorMode")]
        public ColorMode colorMode
        {
            get { return m_colorMode; }
            set
            {
                m_colorMode = value;
                colorModeSpecified = true;
            }
        }
    }

    public class PolyStyle : ColorStyle
    {
    }

    public class LineStyle : ColorStyle
    {
        private float m_Width = 1.0f;

        public LineStyle(Color color)
        {
            Color = color;
        }

        public LineStyle(Color color, float width)
        {
            Color = color;
            m_Width = width;
        }

        public LineStyle()
        {
        }

        [XmlElement("width")]
        public float Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }
    }

    public class IconStyle : PolyStyle
    {
        [XmlIgnore] private bool IconSpecified;
        private Icon m_Icon;
        private float m_scale = 1.0f;
        [XmlIgnore] private bool scaleSpecified;

        public IconStyle(Color color)
        {
            Color = color;
        }

        public IconStyle(Icon icon)
        {
            m_Icon = icon;
        }

        public IconStyle(Color color, float scale, Icon icon)
        {
            Color = color;
            m_scale = scale;
            m_Icon = icon;
        }

        public IconStyle()
        {
        }

        [XmlElement("Icon")]
        public Icon Icon
        {
            get { return m_Icon; }
            set
            {
                m_Icon = value;
                IconSpecified = true;
            }
        }

        [XmlElement("scale")]
        public float scale
        {
            get { return m_scale; }
            set
            {
                m_scale = value;
                scaleSpecified = true;
            }
        }
    }

    public class LabelStyle : ColorStyle
    {
        private float m_scale = 1.0f;
        [XmlIgnore] private bool scaleSpecified;

        public LabelStyle()
        {
        }

        public LabelStyle(Color color)
        {
            Color = color;
        }

        public LabelStyle(float scale)
        {
            m_scale = scale;
        }

        public LabelStyle(Color color, float scale)
        {
            Color = color;
            m_scale = scale;
        }

        public LabelStyle(Color color, float scale, ColorMode newColorMode)
        {
            Color = color;
            m_scale = scale;
            colorMode = newColorMode;
        }

        [XmlElement("scale")]
        public float scale
        {
            get { return m_scale; }
            set
            {
                m_scale = value;
                scaleSpecified = true;
            }
        }
    }
}