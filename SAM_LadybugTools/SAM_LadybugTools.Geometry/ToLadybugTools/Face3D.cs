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
        public static Face3D ToLadybugTools(this Spatial.Face face)
        {
            if (face == null)
                return null;

            if(face is Spatial.ICurvable3D)
            {
                List<Spatial.ICurve3D> curve3Ds = ((Spatial.ICurvable3D)face).GetCurves();
                if(curve3Ds != null)
                {
                    List<List<double>> list = ToLadybugTools(curve3Ds.ConvertAll(x => x.GetStart()));
                    if(list != null && list.Count > 0)
                        return new Face3D(list);
                }
            }

            return null;
        }
    }
}
