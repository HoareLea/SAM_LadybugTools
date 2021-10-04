using SAM.Geometry.Spatial;
using System.Collections.Generic;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Convert
    {
        public static Point3D ToSAM(this List<double> coordinates)
        {
            if(coordinates == null || coordinates.Count < 3)
            {
                return null;
            }

            return new Point3D(coordinates[0], coordinates[1], coordinates[2]);
        }
    }
}