using HoneybeeSchema;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Aperture ToSAM(this HoneybeeSchema.Aperture aperture)
        {
            if(aperture == null)
            {
                return null;
            }

            Geometry.Spatial.Face3D face3D = Geometry.LadybugTools.Convert.ToSAM(aperture.Geometry);
            if(face3D == null)
            {
                return null;
            }

            ApertureConstruction apertureConstruction = new ApertureConstruction(aperture.DisplayName, ApertureType.Window);

            Aperture result = new Aperture(apertureConstruction, face3D);

            return result;
        }

        public static Aperture ToSAM(this Door door)
        {
            if (door == null)
            {
                return null;
            }

            Geometry.Spatial.Face3D face3D = Geometry.LadybugTools.Convert.ToSAM(door.Geometry);
            if (face3D == null)
            {
                return null;
            }

            ApertureConstruction apertureConstruction = new ApertureConstruction(door.DisplayName, ApertureType.Door);

            Aperture result = new Aperture(apertureConstruction, face3D);

            return result;
        }
    }
}