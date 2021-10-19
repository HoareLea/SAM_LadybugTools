using SAM.Geometry.Spatial;
using System.Collections.Generic;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Query
    {
        public static Shell Shell(this HoneybeeSchema.Room room)
        {
            if(room == null)
            {
                return null;
            }

            List<HoneybeeSchema.Face> faces = room.Faces;
            if (faces == null && faces.Count < 3)
            {
                return null;
            }

            List<Face3D> face3Ds = new List<Face3D>();
            foreach (HoneybeeSchema.Face face in room.Faces)
            {
                Face3D face3D = Convert.ToSAM(face.Geometry);
                if (face3D == null)
                {
                    return null;
                }

                face3Ds.Add(face3D);
            }

            return new Shell(face3Ds);
        }
    }
}