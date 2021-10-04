using SAM.Geometry.Spatial;
using System.Collections.Generic;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Convert
    {
        public static List<Point3D> ToSAM(this List<List<double>> values)
        {
            if(values == null)
            {
                return null;
            }

            List<Point3D> result = new List<Point3D>();
            foreach(List<double> coordinates in values)
            {
                Point3D point3D = coordinates?.ToSAM();
                if(point3D == null)
                {
                    continue;
                }

                result.Add(point3D);
            }

            return result;
        }
    }
}