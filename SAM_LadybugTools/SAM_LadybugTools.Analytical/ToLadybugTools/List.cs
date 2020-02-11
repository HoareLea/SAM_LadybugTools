using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoneybeeDotNet;
using SAM.Geometry.Spatial;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<List<double>> ToLadybugTools(this Edge3DLoop edge3DLoop)
        {
            if (edge3DLoop == null)
                return null;

            List<List<double>> result = new List<List<double>>();
            foreach (Edge3D edge3D in edge3DLoop.Edge3Ds)
            {
                List<Segment3D> segment3Ds = edge3D.ToSegments();
                result = Geometry.LadybugTools.Modify.AppendList(result, Geometry.LadybugTools.Convert.ToLadybugTools(Segment3D.GetPoints(segment3Ds)));
            }

            return result;
        }
    }
}
