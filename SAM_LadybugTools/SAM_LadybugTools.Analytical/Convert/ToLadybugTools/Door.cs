using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Door ToLadybugTools_Door(this Aperture aperture, int index = -1, int index_Adjacent = -1, string adjacentPanelUniqueName = null, string adjacentSpaceUniqueName = null)
        {
            if (aperture == null)
                return null;

            if (aperture.ApertureType != ApertureType.Door)
                return null;

            PlanarBoundary3D planarBoundary3D = aperture.PlanarBoundary3D;
            if (planarBoundary3D == null)
                return null;

            AnyOf<Outdoors, Surface> anyOf = new Outdoors();

            Face3D face3D = planarBoundary3D.ToLadybugTools();
            if (!string.IsNullOrEmpty(adjacentPanelUniqueName) && !string.IsNullOrEmpty(adjacentSpaceUniqueName))
            {
                List<string> uniqueNames = new List<string>();
                uniqueNames.Add(Core.LadybugTools.Query.UniqueName(aperture, index_Adjacent));
                uniqueNames.Add(adjacentPanelUniqueName);
                uniqueNames.Add(adjacentSpaceUniqueName);

                anyOf = new Surface(uniqueNames);
            }

            return new Door(Core.LadybugTools.Query.UniqueName(aperture, index), face3D, anyOf, new DoorPropertiesAbridged(), aperture.Name);
        }
    }
}