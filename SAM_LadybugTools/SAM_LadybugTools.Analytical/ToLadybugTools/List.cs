using System.Collections.Generic;

using SAM.Geometry.Spatial;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<List<double>> ToLadybugTools(this BoundaryEdge3DLoop boundaryEdge3DLoop)
        {
            if (boundaryEdge3DLoop == null)
                return null;

            List<List<double>> result = new List<List<double>>();
            foreach (BoundaryEdge3D boundaryEdge3D in boundaryEdge3DLoop.BoundaryEdge3Ds)
            {
                List<Segment3D> segment3Ds = boundaryEdge3D.ToSegments();
                result = Geometry.LadybugTools.Modify.AppendList(result, Geometry.LadybugTools.Convert.ToLadybugTools(segment3Ds.ConvertAll(x => x.GetStart())));
            }

            return result;
        }
    }
}
