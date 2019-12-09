using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel.Types;


namespace SAM_Ladybug.Geometry.Grasshopper
{
    public static partial class Convert
    {
        public static SAM.Geometry.Spatial.Polygon3D ToSAM_Polygon3D(this object polygon)
        {
            var geometry = (polygon as dynamic)._geometry;
            if (geometry == null)
                return null;

            var polygon2D = geometry._polygon2d;
            if (polygon2D == null)
                return null;

            List<SAM.Geometry.Spatial.Point3D> points = new List<SAM.Geometry.Spatial.Point3D>();
            foreach(var vertex in polygon2D._vertices)
                points.Add(Convert.ToSAM_Point3D(vertex));

            return new SAM.Geometry.Spatial.Polygon3D(points);
        }
    }
}
