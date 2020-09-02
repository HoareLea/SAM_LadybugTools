using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static HoneybeeSchema.Aperture ToLadybugTools(this Aperture aperture, Panel panel = null, Space space = null)
        {
            if (aperture == null)
                return null;

            PlanarBoundary3D planarBoundary3D = aperture.PlanarBoundary3D;
            if (planarBoundary3D == null)
                return null;

            AnyOf<Outdoors, Surface> anyOf = new Outdoors();

            string uniqueName = Core.LadybugTools.Query.UniqueName(aperture);

            Face3D face3D = planarBoundary3D.ToLadybugTools();
            if(panel != null && panel.IsInternal())
            {
                List<string> uniqueNames = new List<string>();
                uniqueNames.Add(uniqueName);

                uniqueNames.Add(Core.LadybugTools.Query.UniqueName(panel));

                if(space != null)
                    uniqueNames.Add(Core.LadybugTools.Query.UniqueName(space));

                anyOf = new Surface(uniqueNames);
            }

            return new HoneybeeSchema.Aperture(uniqueName, face3D, anyOf, new AperturePropertiesAbridged(), aperture.Name);
        }
    }
}