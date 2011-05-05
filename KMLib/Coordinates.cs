namespace KMLib
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Core.Geometry;
    using Core.Utils;

    public class Coordinates : List<Point3D>, IXmlSerializable
    {
        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader r)
        {
            var str = r.ReadElementContentAsString();
            Deserialize(str);
        }

        public virtual void WriteXml(XmlWriter w)
        {
            w.WriteString(Serialize());
        }

        #endregion

        public virtual string Serialize()
        {
            var ans = string.Empty;
            for (var i = 0; i < Count; i++)
            {
                ans += this[i].Serialize();
                if (i < Count - 1)
                {
                    ans += " ";
                }
            }

            return ans;
        }

        public virtual void Deserialize(string str)
        {
            str = StringUtils.RemoveExcessWhiteSpace(str);
            var bits = StringUtils.SplitToList(str, " ");
            Clear();
            for (var i = 0; i < bits.Count; i++)
            {
                Add(Point3D.MakePointFromStr(bits[i]));
            }
        }
    }
}