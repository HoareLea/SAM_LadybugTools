using SAM.Geometry.Object.Spatial;
using SAM.Geometry.Spatial;
using System.Collections.Generic;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Convert
    {
        public static HoneybeeSchema.Face3D ToLadybugTools(this Face3D face3D)
        {
            if (face3D == null)
                return null;

            ISegmentable3D externalEdge = face3D.GetExternalEdge3D() as ISegmentable3D;
            if (externalEdge == null)
                throw new System.NotImplementedException();

            List<Point3D> point3Ds = externalEdge.GetPoints();
            if (point3Ds == null || point3Ds.Count < 3)
                return null;

            List<List<double>> boundary = ToLadybugTools(point3Ds);
            if (boundary == null || boundary.Count == 0)
                return null;

            List<List<List<double>>> holes = new List<List<List<double>>>();
            List<IClosedPlanar3D> internalEdges = face3D.GetInternalEdge3Ds();
            if(internalEdges != null && internalEdges.Count > 0)
            {
                foreach(IClosedPlanar3D internalEdge_Temp in internalEdges)
                {
                    ISegmentable3D internalEdge = internalEdge_Temp as ISegmentable3D;
                    if (internalEdge == null)
                        throw new System.NotImplementedException();

                    point3Ds = internalEdge.GetPoints();
                    if (point3Ds == null || point3Ds.Count < 3)
                        continue;

                    List<List<double>> hole = ToLadybugTools(point3Ds);
                    if (hole == null || hole.Count == 0)
                        continue;

                    holes.Add(hole);
                }
            }

            return new HoneybeeSchema.Face3D(boundary, holes);
        }

        public static HoneybeeSchema.Face3D ToLadybugTools(this IFace3DObject face3DObject)
        {
            return ToLadybugTools(face3DObject?.Face3D);
        }
    }
}