using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static HoneybeeSchema.Aperture ToLadybugTools(this Aperture aperture, MaterialLibrary materialLibrary = null, int index = -1, int index_Adjacent = -1, string adjacentPanelUniqueName = null, string adjacentSpaceUniqueName = null)
        {
            if (aperture == null)
                return null;

            if (aperture.ApertureType == ApertureType.Door)
                return null;

            ApertureConstruction apertureConstruction = aperture.ApertureConstruction;
            if (apertureConstruction == null)
                return null;

            //Opaque Windows to be replaced by Doors
            if (apertureConstruction.PaneConstructionLayers.MaterialType(materialLibrary) == MaterialType.Opaque)
                return null;

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

            ApertureEnergyPropertiesAbridged apertureEnergyPropertiesAbridged = new ApertureEnergyPropertiesAbridged(Query.UniqueName(apertureConstruction, !(index_Adjacent != -1 && index <= index_Adjacent)));

            return new HoneybeeSchema.Aperture(
                identifier: Core.LadybugTools.Query.UniqueName(aperture, index),
                geometry: face3D,
                boundaryCondition: anyOf,
                properties: new AperturePropertiesAbridged(apertureEnergyPropertiesAbridged),
                displayName: aperture.Name);
        }
    }
}