namespace Core.Geometry
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Utils;

    [TypeConverter(typeof (SizeEExpandableConverter))]
    public class SizeE : IXmlSerializable
    {
        #region Private Fields

        private double m_Height;
        private double m_Width;

        #endregion

        #region Constructors

        public SizeE()
        {
        }

        public SizeE(SizeE size)
            : this(size.Width, size.Height)
        {
        }

        public SizeE(PointE pt)
            : this(pt.X, pt.Y)
        {
        }

        public SizeE(double width, double height)
        {
            m_Width = width;
            m_Height = height;
        }

        #endregion

        #region GetSet

        [XmlAttribute]
        public double Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        [XmlAttribute]
        public double Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        [XmlIgnore, Browsable(false)]
        public bool IsEmpty
        {
            get { return m_Height == 0 || m_Width == 0; }
        }

        #endregion

        #region Operator Overloads

        public static SizeE operator +(SizeE c)
        {
            var ans = new SizeE();
            ans.Width = +c.Width;
            ans.Height = +c.Height;
            return ans;
        }

        public static SizeE operator -(SizeE c)
        {
            var ans = new SizeE();
            ans.Width = -c.Width;
            ans.Height = -c.Height;
            return ans;
        }

        public static SizeE operator +(SizeE a, SizeE b)
        {
            return new SizeE(a.Width + b.Width, a.Height + b.Height);
        }

        public static SizeE operator -(SizeE a, SizeE b)
        {
            return new SizeE(a.Width - b.Width, a.Height - b.Height);
        }

        public static SizeE operator *(SizeE a, SizeE b)
        {
            return new SizeE(a.Width*b.Width, a.Height*b.Height);
        }

        public static SizeE operator *(SizeE a, double scalar)
        {
            return new SizeE(a.Width*scalar, a.Height*scalar);
        }

        public static SizeE operator *(SizeE a, float scalar)
        {
            return new SizeE(a.Width*scalar, a.Height*scalar);
        }

        public static SizeE operator /(SizeE a, SizeE b)
        {
            return new SizeE(a.Width/b.Width, a.Height/b.Height);
        }

        public static SizeE operator /(SizeE a, double scalar)
        {
            return new SizeE(a.Width/scalar, a.Height/scalar);
        }

        public static SizeE operator /(SizeE a, float scalar)
        {
            return new SizeE(a.Width/scalar, a.Height/scalar);
        }

        #endregion

        #region Conversions

        public static implicit operator SizeE(SizeF floatSize)
        {
            return new SizeE(floatSize.Width, floatSize.Height);
        }

        public static explicit operator SizeF(SizeE doublePt)
        {
            return new SizeF((float) doublePt.Width, (float) doublePt.Height);
        }

        public static implicit operator SizeE(Size intSize)
        {
            return new SizeE(intSize.Width, intSize.Height);
        }

        public static explicit operator Size(SizeE doublePt)
        {
            return new Size((int) doublePt.Width, (int) doublePt.Height);
        }

        #endregion

// Size2D
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

        #region (De)Serialize

        public virtual string Serialize()
        {
            return m_Width + "," + m_Height;
        }

        public virtual void Deserialize(string str)
        {
            var bits = StringUtils.SplitToList(str, ",");
            m_Width = Convert.ToDouble(bits[0]);
            m_Height = Convert.ToDouble(bits[1]);
        }

        #endregion

        public SizeE Inflate(double amount)
        {
            return Inflate(amount, amount);
        }

        public SizeE Inflate(SizeE amount)
        {
            return Inflate(amount.Width, amount.Height);
        }

        public SizeE Inflate(double X, double Y)
        {
            return new SizeE(m_Width + 2*X, m_Height + 2*Y);
        }

        public void SetMaxDim(double maxSize)
        {
            if (m_Height > m_Width)
            {
                m_Width = maxSize*m_Width/m_Height;
                m_Height = maxSize;
            }
            else
            {
                m_Height = maxSize*m_Height/m_Width;
                m_Width = maxSize;
            }
        }

        public void SetMinDim(double minSize)
        {
            if (m_Height > m_Width)
            {
                m_Height = minSize*m_Height/m_Width;
                m_Width = minSize;
            }
            else
            {
                m_Width = minSize*m_Width/m_Height;
                m_Height = minSize;
            }
        }
    }

    internal class SizeEExpandableConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, 
                                         Type destinationType)
        {
            var size = value as SizeE;
            if (size != null && destinationType == typeof (string))
            {
                return size.Width + ", " + size.Height;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            if (str != null)
            {
                try
                {
                    if (str.Contains(","))
                    {
                        var commapos = str.IndexOf(",");
                        var width = Convert.ToDouble(str.Substring(0, commapos).Trim());
                        var height = Convert.ToDouble(str.Substring(commapos + 1).Trim());
                        return new SizeE(width, height);
                    }
                }
                catch
                {
                    throw new ArgumentException("Please enter a width and height seperated by a comma.");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}