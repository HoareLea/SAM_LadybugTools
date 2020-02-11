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
            List<double> xs = new List<double>();
            List<double> ys = new List<double>();
            List<double> zs = new List<double>();

            foreach(Spatial.Point3D point3D in point3Ds)
            {
                xs.Add(point3D.X);
                ys.Add(point3D.Y);
                zs.Add(point3D.Z);
            }

            result.Add(xs);
            result.Add(ys);
            result.Add(zs);

            return result;
        }
    }
}
