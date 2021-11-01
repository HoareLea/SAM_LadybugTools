using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static HoneybeeSchema.Aperture ToLadybugTools(this Window window, ArchitecturalModel architecturalModel, Space space)
        {
            if (window == null || architecturalModel == null)
                return null;

            WindowType windowType = window.Type;
            if (windowType == null)
            {
                return null;
            }

            //Opaque Windows to be replaced by Doors
            if (architecturalModel.GetMaterialType(windowType.PaneMaterialLayers) == MaterialType.Opaque)
            {
                return null;
            }

            IHostPartition hostPartition = architecturalModel.GetHostPartition(window);
            
            int index = -1;
            int index_Adjacent = -1;
            List<Space> spaces = null;
            if (hostPartition != null)
            {
                spaces = architecturalModel.GetSpaces(hostPartition);
                if (spaces != null && spaces.Count != 0)
                {
                    index = spaces.FindIndex(x => x.Guid == space.Guid);
                    index = architecturalModel.UniqueIndex(spaces[index]);
                    
                    index_Adjacent = spaces.FindIndex(x => x.Guid != space.Guid);
                    index_Adjacent = architecturalModel.UniqueIndex(spaces[index_Adjacent]);
                }
            }

            HoneybeeSchema.AnyOf<Outdoors, Surface> anyOf = null;
            if(index == -1 || index_Adjacent == -1)
            {
                anyOf = new Outdoors();
            }
            else
            {
                bool reversed = index_Adjacent < index;
                List<string> uniqueNames = new List<string>();
                uniqueNames.Add(Core.LadybugTools.Query.UniqueName(window, index_Adjacent));
                uniqueNames.Add(Core.LadybugTools.Query.UniqueName(hostPartition as SAMObject, index_Adjacent));
                uniqueNames.Add(Core.LadybugTools.Query.UniqueName(spaces[index_Adjacent]));
                anyOf = new Surface(uniqueNames);
            }

            Face3D face3D = Geometry.LadybugTools.Convert.ToLadybugTools(window);

            ApertureEnergyPropertiesAbridged apertureEnergyPropertiesAbridged = new ApertureEnergyPropertiesAbridged(construction: Query.UniqueName(window.Type, !(index_Adjacent != -1 && index <= index_Adjacent)));

            return new HoneybeeSchema.Aperture(
                identifier: Core.LadybugTools.Query.UniqueName(window, index),
                geometry: face3D,
                boundaryCondition: anyOf,
                properties: new AperturePropertiesAbridged(apertureEnergyPropertiesAbridged),
                displayName: window.Name);
        }
    }
}