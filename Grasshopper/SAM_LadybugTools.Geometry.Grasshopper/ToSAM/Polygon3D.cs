using System.Collections.Generic;

namespace SAM.Geometry.Grasshopper.LadybugTools
{
    public static partial class Convert
    {
        public static Spatial.Polygon3D ToSAM_Polygon3D(this object polygon)
        {
            var geometry = (polygon as dynamic)._geometry;
            if (geometry == null)
                return null;

            var polygon2D = geometry._polygon2d;
            if (polygon2D == null)
                return null;

            List<Spatial.Point3D> points = new List<Spatial.Point3D>();
            foreach (var vertex in polygon2D._vertices)
                points.Add(Convert.ToSAM_Point3D(vertex));

            return new SAM.Geometry.Spatial.Polygon3D(points);
        }
    }
}