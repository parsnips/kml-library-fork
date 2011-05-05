namespace KMLib
{
    using System.Xml.Serialization;

    public class Snippet
    {
        private int m_maxLines = 2;

        public Snippet()
        {
        }

        public Snippet(string text)
        {
            Text = text;
        }

        [XmlAttribute]
        public int maxLines
        {
            get { return m_maxLines; }
            set { m_maxLines = value; }
        }

        [XmlText]
        public string Text { get; set; }

        public static implicit operator string(Snippet comp)
        {
            return comp.Text;
        }

        public static implicit operator Snippet(string comp)
        {
            return new Snippet(comp);
        }
    }

    /*public class Snippet : AXmlSerializable
    {
        private int m_maxLines = 2;
        public int maxLines {
            get {
                return m_maxLines;
            }
            set {
                m_maxLines = value;
            }
        }

        private string m_Text;
        public string Text {
            get {
                return m_Text;
            }
            set {
                m_Text = value;
            }
        }

        public override void ReadXml(System.Xml.XmlReader r) {
            r.ReadStartElement("Snippet");
            if (r.HasAttributes) {
                r.MoveToAttribute("maxLines");
                m_maxLines = r.ReadContentAsInt();
            }
            r.MoveToContent();
            m_Text = r.ReadString();
            r.ReadEndElement();
        }

        public override void WriteXml(System.Xml.XmlWriter w) {
            w.WriteStartElement("Snippet");
            w.WriteAttributeString("maxLines", m_maxLines.ToString());
            w.WriteString(m_Text);
            w.WriteEndElement();
        }
    }*/
}