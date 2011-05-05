namespace Core.Xml
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class XmlDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader r)
        {
            r.ReadStartElement();
            var hasRead = false;
            r.ReadStartElement("dictionary");
            var begin_d = r.Depth;
            while (readStartElementOK("item", r))
            {
                r.ReadStartElement("key");
                var keySer = XmlTypeAssociator<TKey>.GetSerializer(r);
                XMLSerializeManager.Report("Deserializing Key: " + r.Name);
                var key = (TKey) keySer.Deserialize(r);
                readUntilEndElement("key", r);

                r.ReadStartElement("value");
                var valueSer = XmlTypeAssociator<TValue>.GetSerializer(r);
                XMLSerializeManager.Report("Deserializing Value: " + r.Name);
                var value = (TValue) valueSer.Deserialize(r);
                readUntilEndElement("value", r);

                Add(key, value);

                r.ReadEndElement(); // --item

                r.MoveToContent();
                hasRead = true;
            }

            r.ReadEndElement(); // --dictionary
            if (hasRead)
            {
                r.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter w)
        {
            w.WriteStartElement("dictionary");
            foreach (var key in Keys)
            {
                w.WriteStartElement("item");

                w.WriteStartElement("key");
                var keySer = XmlTypeAssociator<TKey>.GetSerializer(key.GetType());
                XMLSerializeManager.Report("Serializing key: " + key.GetType().Name);
                keySer.Serialize(w, key);
                w.WriteEndElement();

                w.WriteStartElement("value");
                var value = this[key];
                var valueSer = XmlTypeAssociator<TValue>.GetSerializer(value.GetType());
                XMLSerializeManager.Report("Serializing value: " + value.GetType().Name);
                valueSer.Serialize(w, value);
                w.WriteEndElement();

                w.WriteEndElement();
            }

            w.WriteEndElement();
        }

        #endregion

        private bool readStartElementOK(string nodeName, XmlReader r)
        {
            try
            {
                r.ReadStartElement(nodeName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void readUntilEndElement(string p, XmlReader r)
        {
            // --sometimes a Value Type may not move the reader's cursor to after its last element, so we need to make sure to move it to the right place...
            while (r.Name != p)
            {
                r.ReadEndElement();
            }

            r.ReadEndElement();
        }
    }
}