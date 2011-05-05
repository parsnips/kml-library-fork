namespace KMLib
{
    using System;
    using Core.Geometry;

    public class CoordManager
    {
        // --longitude is X, latitude is Y
        private double degreeInKm = 111;
        private double kmInFt = 3280.8399;
        private PointE m_LatLongOrigin = new PointE(-77.05, 38.97);

        public PointE LatLongOrigin
        {
            get { return m_LatLongOrigin; }
            set { m_LatLongOrigin = value; }
        }

        public Point3D ConvertToLatLong(double x, double y, double z)
        {
            var ans = new Point3D();
            ans.X = m_LatLongOrigin.X + ConvertFtToLong(x);
            ans.Y = m_LatLongOrigin.Y + ConvertFtToLat(y);
            ans.Z = z/(kmInFt/1000);
            return ans;
        }

        private double ConvertFtToLong(double ft)
        {
            return ConvertFtToLat(ft)/Math.Cos(m_LatLongOrigin.Y*(Math.PI/180));
        }

        private double ConvertFtToLat(double ft)
        {
            return (ft/kmInFt)/degreeInKm;
        }
    }
}