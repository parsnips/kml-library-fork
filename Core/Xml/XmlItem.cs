namespace Core.Xml
{
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class XmlItem<T> : IXmlSerializable
    {
        private T m_Value;

        public XmlItem()
        {
        }

        public XmlItem(T value)
        {
            m_Value = value;
        }

        public T Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader r)
        {
            if (r.IsEmptyElement)
            {
                return;
            }

            r.ReadStartElement();
            var ser = XmlTypeAssociator<T>.GetSerializer(r.Name);
            XMLSerializeManager.Report("Deserializing: " + r.Name);
            m_Value = (T) ser.Deserialize(r);
            r.MoveToContent();
            r.ReadEndElement();
        }

        public void WriteXml(XmlWriter w)
        {
            if (m_Value == null)
            {
                return;
            }

            var ser = XmlTypeAssociator<T>.GetSerializer(m_Value.GetType());
            XMLSerializeManager.Report("Serializing value: " + m_Value.GetType().Name);
            ser.Serialize(w, m_Value);
        }

        #endregion
    }
}