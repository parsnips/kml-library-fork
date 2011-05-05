namespace Core.Xml
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using ExtendedObjects;

    /// <summary>
    ///   Although MS Lists are XML serializable, there is no way to serialize Lists of interface types. XmlLists allow you to associate derived types using "AddType". This is an alternative to using the XmlArrayItem attribute that has to be applied to a particular property or field.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public class XmlList<T> : EventList<T>, IXmlSerializable
    {
        public XmlList()
        {
        }

        public XmlList(IEnumerable<T> collection) : base(collection)
        {
        }

        public XmlList(int capacity) : base(capacity)
        {
        }

        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader r)
        {
            var depth = r.Depth;
            r.ReadStartElement();
            var hasRead = false;
            while (r.Depth > depth)
            {
                if (r.IsStartElement())
                {
                    var ser = XmlTypeAssociator<T>.GetSerializer(r);
                    XMLSerializeManager.Report("Deserializing: " + r.Name);
                    Add((T) ser.Deserialize(r));
                    r.MoveToContent();
                }
                else
                {
                    r.Read();
                }

                hasRead = true;
            }

            if (hasRead)
            {
                r.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter w)
        {
            foreach (var item in this)
            {
                var ser = XmlTypeAssociator<T>.GetSerializer(item.GetType());
                XMLSerializeManager.Report("Serializing item: " + item.GetType().Name);
                var ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);
                ser.Serialize(w, item, ns);
            }
        }

        #endregion
    }
}