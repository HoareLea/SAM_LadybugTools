using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Door ToLadybugTools_Door(this Aperture aperture, MaterialLibrary materialLibrary = null, int index = -1, int index_Adjacent = -1, string adjacentPanelUniqueName = null, string adjacentSpaceUniqueName = null)
        {
            if (aperture == null)
                return null;

            ApertureConstruction apertureConstruction = aperture.ApertureConstruction;
            if (apertureConstruction == null)
                return null;

            MaterialType materialType = apertureConstruction.PaneConstructionLayers.MaterialType(materialLibrary);

            if (aperture.ApertureType != ApertureType.Door)
            {
                //Opaque Windows to be replaced by Doors
                if (materialType != MaterialType.Opaque)
                    return null;
            }

            PlanarBoundary3D planarBoundary3D = aperture.PlanarBoundary3D;
            if (planarBoundary3D == null)
                return null;

            HoneybeeSchema.AnyOf<Outdoors, Surface> anyOf = new Outdoors();

            Face3D face3D = planarBoundary3D.ToLadybugTools();
            if (!string.IsNullOrEmpty(adjacentPanelUniqueName) && !string.IsNullOrEmpty(adjacentSpaceUniqueName))
            {
                List<string> uniqueNames = new List<string>();
                uniqueNames.Add(Core.LadybugTools.Query.UniqueName(aperture, index_Adjacent));
                uniqueNames.Add(adjacentPanelUniqueName);
                uniqueNames.Add(adjacentSpaceUniqueName);

                anyOf = new Surface(uniqueNames);
            }

            DoorEnergyPropertiesAbridged apertureEnergyPropertiesAbridged = new DoorEnergyPropertiesAbridged(Query.UniqueName(apertureConstruction, !(index_Adjacent != -1 && index <= index_Adjacent)));
            
            Door door = new Door(Core.LadybugTools.Query.UniqueName(aperture, index), face3D, anyOf, new DoorPropertiesAbridged(apertureEnergyPropertiesAbridged), aperture.Name);
            door.IsGlass = materialType == MaterialType.Transparent;
            return door;
        }
    }
}