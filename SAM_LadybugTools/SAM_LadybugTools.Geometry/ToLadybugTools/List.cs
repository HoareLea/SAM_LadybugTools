using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoneybeeDotNet;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Convert
    {
        public static List<List<double>> ToLadybugTools(this IEnumerable<Spatial.Point3D> point3Ds)
        {
            if (point3Ds == null)
                return null;

            List<List<double>> result = new List<List<double>>();

            foreach(Spatial.Point3D point3D in point3Ds)
                result.Add(point3D.ToLadybugTools());            

            return result;
        }

        public static List<double> ToLadybugTools(this Spatial.Point3D point3D)
        {
            if (point3D == null)
                return null;

            return new List<double>() { point3D.X, point3D.Y, point3D.Z };
        }
    }
}
