using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static WindowConstructionAbridged ToLadybugTools_WindowConstructionAbridged(this WindowType windowType, bool reverse = true)
        {
            if (windowType == null)
                return null;

            List<Architectural.MaterialLayer> materialLayers = windowType.PaneMaterialLayers;
            if (materialLayers == null)
                return null;

            if(reverse)
            {
                materialLayers.Reverse();
            }

            WindowConstructionAbridged result = new WindowConstructionAbridged(Query.UniqueName(windowType, reverse), materialLayers.ConvertAll(x => x.Name), windowType.Name);

            return result;
        }
    }
}