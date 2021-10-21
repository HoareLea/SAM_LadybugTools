using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Query
    {
        public static double ExteriorArea(this Room room)
        {
            List<HoneybeeSchema.Face> faces = room?.Faces;
            if(faces == null)
            {
                return double.NaN;
            }

            double result = 0;
            foreach(HoneybeeSchema.Face face in faces)
            {
                if(!(face?.BoundaryCondition?.Obj is Outdoors))
                {
                    continue;
                }

                Spatial.Face3D face3D = face.Geometry?.ToSAM();
                if(face3D == null)
                {
                    continue;
                }

                result += face3D.GetArea();
            }

            return result;
        }
    }
}