using SAM.Geometry.Spatial;
using System.Collections.Generic;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Convert
    {
        public static Face3D ToSAM(this HoneybeeSchema.Face3D face3D)
        {
            if(face3D == null)
            {
                return null;
            }

            Plane plane = face3D.Plane?.ToSAM();

            IClosedPlanar3D externalEdge3D = null;

            List<Point3D> point3Ds = face3D.Boundary?.ToSAM();
            if(point3Ds == null || point3Ds.Count < 3)
            {
                return null;
            }

            if(plane != null)
            {
                externalEdge3D = new Polygon3D(plane, point3Ds.ConvertAll(x => plane.Convert(x)));
            }
            else
            {
                externalEdge3D = new Polygon3D(point3Ds);
            }

            plane = externalEdge3D.GetPlane();
            if(plane == null)
            {
                return null;
            }

            List<IClosedPlanar3D> internalEdge3Ds = null;
            if (face3D.Holes != null)
            {
                internalEdge3Ds = new List<IClosedPlanar3D>();
                foreach(List<List<double>> values in face3D.Holes)
                {
                    point3Ds = values?.ToSAM();
                    if(point3Ds == null || point3Ds.Count < 3)
                    {
                        continue;
                    }

                    internalEdge3Ds.Add(new Polygon3D(plane, point3Ds.ConvertAll(x => plane.Convert(x))));
                }
            }

            return Face3D.Create(externalEdge3D.GetPlane(), plane.Convert(externalEdge3D), internalEdge3Ds?.ConvertAll(x => plane.Convert(x)));
        }
    }
}