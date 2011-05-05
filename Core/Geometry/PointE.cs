namespace Core.Geometry
{
    using System;
    using System.Drawing;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Utils;

    public interface IPoint
    {
        double X { get; set; }
        double Y { get; set; }

        bool Equals(IPoint test, double tolerance);
        string Serialize();
        void Deserialize(string str);
    }

    public class PointE : AShapeE, IXmlSerializable, IPoint
    {
        #region Private Fields

        private double m_X;
        private double m_Y;

        #endregion

        #region Constructors

        public PointE()
        {
        }

        public PointE(double x, double y)
        {
            m_X = x;
            m_Y = y;
        }

        public PointE(PointE pt)
            : this(pt.X, pt.Y)
        {
        }

        #endregion

        #region GetSet

        public double Abs
        {
            get { return Math.Sqrt(X*X + Y*Y); }
        }

        public double Magnitude
        {
            get { return Math.Sqrt(X*X + Y*Y); }
        }

        public double X
        {
            get { return m_X; }
            set
            {
                m_X = value;
                CallGeometryChanged();
            }
        }

        public double Y
        {
            get { return m_Y; }
            set
            {
                m_Y = value;
                CallGeometryChanged();
            }
        }

        public PointE GetNormalized()
        {
            var magnitude = Magnitude;

            return new PointE(X/magnitude, Y/magnitude);
        }

        #endregion

// Point2D
        public override RectangleE BoundingBox
        {
            get { return null; }
        }

        #region IPoint Members

        public virtual bool Equals(IPoint test, double tolerance)
        {
            return Math.Abs(test.X - X) < tolerance && Math.Abs(test.Y - Y) < tolerance;
        }

        #endregion

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
            return m_X + "," + m_Y;
        }

        public virtual void Deserialize(string str)
        {
            var bits = StringUtils.SplitToList(str, ",");
            m_X = Convert.ToDouble(bits[0]);
            m_Y = Convert.ToDouble(bits[1]);
        }

        #endregion

        public PointE AbsDifference(PointE point)
        {
            return new PointE(Math.Abs(X - point.X), Math.Abs(Y - point.Y));
        }

        public double Dist(PointE point)
        {
            var x = X - point.X;
            var y = Y - point.Y;
            return Math.Sqrt(x*x + y*y);
        }

        public void Normalize()
        {
            var magnitude = Magnitude;
            X = X/magnitude;
            Y = Y/magnitude;
        }

        public double DotProduct(PointE vector)
        {
            return X*vector.X + Y*vector.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            var test = obj as PointE;
            return test.X == X && test.Y == Y;
        }

        public override int GetHashCode()
        {
            return (int) X ^ (int) Y;
        }

        public override string ToString()
        {
            return "PointE: (" + X + ", " + Y + ")";
        }

        public PointE ShiftPt(PointE offset)
        {
            return new PointE(X + offset.X, Y + offset.Y);
        }

        public override IShapeE Shift(PointE offset)
        {
            return ShiftPt(offset);
        }

        public bool HasSamePoints(PointE nxtPt)
        {
            return nxtPt.X == X && nxtPt.Y == Y;
        }

        public bool HasSamePoints(PointE nxtPt, double tol)
        {
            if (HasSamePoints(nxtPt))
            {
                return true;
            }

            if (Math.Abs(nxtPt.X - X) > tol)
            {
                return false;
            }

            if (Math.Abs(nxtPt.Y - Y) > tol)
            {
                return false;
            }

            return true;

// return Calc.GetDist(this, nxtPt) < tol;
        }

        public PointE TransformByRotationAbout(double rotation, PointE center)
        {
            var t = this - center; // translate to origin

// apply rotation matrix cos x  -sin x
            // sin x   cos x which rotates about origin
            var cos = Math.Cos(rotation);
            var sin = Math.Sin(rotation);
            t = new PointE(cos*t.X - sin*t.Y, sin*t.X + cos*t.Y);
            return t + center; // translate back
        }

        public void UpdatePoint(PointE pointE)
        {
            if (pointE == null)
            {
                return;
            }

            m_X = pointE.m_X;
            m_Y = pointE.m_Y;
        }

        #region Conversions

        public static implicit operator PointE(PointF floatPt)
        {
            return new PointE(floatPt.X, floatPt.Y);
        }

        public static explicit operator PointF(PointE doublePt)
        {
            return new PointF((float) doublePt.X, (float) doublePt.Y);
        }

        public static implicit operator PointE(Point intPt)
        {
            return new PointE(intPt.X, intPt.Y);
        }

        public static explicit operator Point(PointE doublePt)
        {
            return new Point((int) doublePt.X, (int) doublePt.Y);
        }

        public static implicit operator PointE(SizeE size)
        {
            return new PointE(size.Width, size.Height);
        }

        public static explicit operator SizeE(PointE doublePt)
        {
            return new SizeE((float) doublePt.X, (float) doublePt.Y);
        }

        #endregion

        #region Operator Overloads

        public static PointE operator +(PointE c)
        {
            var ans = new PointE();
            ans.X = +c.X;
            ans.Y = +c.Y;
            return ans;
        }

        public static PointE operator -(PointE c)
        {
            var ans = new PointE();
            ans.X = -c.X;
            ans.Y = -c.Y;
            return ans;
        }

        public static PointE operator +(PointE a, PointE b)
        {
            return new PointE(a.X + b.X, a.Y + b.Y);
        }

        public static PointE operator -(PointE a, PointE b)
        {
            return new PointE(a.X - b.X, a.Y - b.Y);
        }

        public static PointE operator *(PointE a, PointE b)
        {
            return new PointE(a.X*b.X, a.Y*b.Y);
        }

        public static PointE operator *(PointE a, double muliplier)
        {
            return new PointE(a.X*muliplier, a.Y*muliplier);
        }

        public static PointE operator /(PointE a, PointE b)
        {
            return new PointE(a.X/b.X, a.Y/b.Y);
        }

        public static PointE operator /(PointE a, double divisor)
        {
            return new PointE(a.X/divisor, a.Y/divisor);
        }

        #endregion
    }
}