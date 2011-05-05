namespace Core.Geometry
{
    using System;

    public interface IShapeE
    {
        RectangleE BoundingBox { get; }
        object Tag { get; set; }
        event EventHandler GeometryChanged;
        bool ContainsPoint(PointE point);
        bool MostlyContains(IShapeE iShapeE);
        bool FullyContains(IShapeE iShapeE);
        IShapeE Shift(PointE pointE);
    }

    public interface IShapeEUpdateable : IShapeE
    {
        event ShapeUpdateEventHandler ShapeUpdated;
        void UpdateShape(IShapeE newShape);
    }

    public delegate void ShapeUpdateEventHandler(object sender, ShapeUpdateEventArgs e);

    public class ShapeUpdateEventArgs : EventArgs
    {
        public IShapeE NewShape;
        public IShapeE OldShape;

        public ShapeUpdateEventArgs(IShapeE oldShape, IShapeE newShape)
        {
            OldShape = oldShape;
            NewShape = newShape;
        }
    }

    public abstract class AShapeE : IShapeEUpdateable
    {
        #region IShapeEUpdateable Members

        public event EventHandler GeometryChanged;
        public event ShapeUpdateEventHandler ShapeUpdated;
        public abstract RectangleE BoundingBox { get; }
        public abstract IShapeE Shift(PointE point);

        public virtual bool ContainsPoint(PointE point)
        {
            return false;
        }

        public bool MostlyContains(IShapeE shapeE)
        {
            return MostlyContains(shapeE, 0.95);
        }

        public virtual bool FullyContains(IShapeE shapeE)
        {
            return false;
        }

        public void UpdateShape(IShapeE newShape)
        {
            if (ShapeUpdated != null)
            {
                ShapeUpdated(this, new ShapeUpdateEventArgs(this, newShape));
            }
        }

        public object Tag { get; set; }

        #endregion

        public virtual bool MostlyContains(IShapeE shapeE, double tolerance)
        {
            return false;
        }

        protected void CallGeometryChanged()
        {
            if (GeometryChanged != null)
            {
                GeometryChanged(this, new EventArgs());
            }
        }

        public static int Compare(IShapeE a, IShapeE b)
        {
            if (a.Equals(b))
            {
                return 0;
            }

            if (a == null || a.BoundingBox == null)
            {
                return -1;
            }

            if (b == null || b.BoundingBox == null)
            {
                return 1;
            }

            return b.BoundingBox.CompareTo(a.BoundingBox);
        }
    }
}