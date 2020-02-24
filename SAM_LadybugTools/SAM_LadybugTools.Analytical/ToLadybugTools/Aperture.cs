using HoneybeeDotNet;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static HoneybeeDotNet.Aperture ToLadybugTools(this Aperture aperture)
        {
            if (aperture == null)
                return null;

            PlanarBoundary3D planarBoundary3D = aperture.GetPlanarBoundary3D();
            if (planarBoundary3D == null)
                return null;

            Face3D face3D = planarBoundary3D.ToLadybugTools();

            return new HoneybeeDotNet.Aperture(aperture.ApertureType.Name + "__" + aperture.Guid.ToString(), face3D, new Outdoors(), new AperturePropertiesAbridged());
        }
    }
}
