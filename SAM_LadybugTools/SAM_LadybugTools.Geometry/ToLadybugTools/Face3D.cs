using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Convert
    {
        public static Face3D ToLadybugTools(this Spatial.Face3D face3D)
        {
            if (face3D == null)
                return null;

            if (face3D is Spatial.ICurvable3D)
            {
                List<Spatial.ICurve3D> curve3Ds = ((Spatial.ICurvable3D)face3D).GetCurves();
                if (curve3Ds != null)
                {
                    List<List<double>> list = ToLadybugTools(curve3Ds.ConvertAll(x => x.GetStart()));
                    if (list != null && list.Count > 0)
                        return new Face3D(list);
                }
            }

            return null;
        }
    }
}