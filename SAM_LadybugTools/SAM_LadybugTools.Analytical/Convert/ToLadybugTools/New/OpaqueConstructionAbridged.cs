using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static OpaqueConstructionAbridged ToLadybugTools(this HostPartitionType hostPartitionType, bool reverse = true)
        {
            if (hostPartitionType == null)
                return null;

            List<Architectural.MaterialLayer> materialLayers = hostPartitionType.MaterialLayers;
            if (materialLayers == null || materialLayers.Count == 0)
                return null;

            if (reverse)
            {
                materialLayers.Reverse();
            }


            OpaqueConstructionAbridged result = new OpaqueConstructionAbridged(Query.UniqueName(hostPartitionType, reverse), materialLayers.ConvertAll(x => x.Name), hostPartitionType.Name);
            return result;
        }

        public static OpaqueConstructionAbridged ToLadybugTools(this OpeningType openingType, bool reverse = true)
        {
            if (openingType == null)
                return null;

            List<Architectural.MaterialLayer> materialLayers = openingType.PaneMaterialLayers;
            if (materialLayers == null || materialLayers.Count == 0)
                return null;

            if(reverse)
            {
                materialLayers.Reverse();
            }
            

            OpaqueConstructionAbridged result = new OpaqueConstructionAbridged(Query.UniqueName(openingType, reverse), materialLayers.ConvertAll(x => x.Name), openingType.Name);
            return result;
        }
    }
}