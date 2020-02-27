using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static HoneybeeSchema.Aperture ToLadybugTools(this Aperture aperture)
        {
            if (aperture == null)
                return null;

            PlanarBoundary3D planarBoundary3D = aperture.PlanarBoundary3D;
            if (planarBoundary3D == null)
                return null;

            Face3D face3D = planarBoundary3D.ToLadybugTools();

            return new HoneybeeSchema.Aperture(Core.LadybugTools.Query.UniqueName(aperture), face3D, new Outdoors(), new AperturePropertiesAbridged());
        }
    }
}
