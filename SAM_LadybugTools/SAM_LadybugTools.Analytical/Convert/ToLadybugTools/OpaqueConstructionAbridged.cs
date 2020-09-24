using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static OpaqueConstructionAbridged ToLadybugTools(this Construction construction)
        {
            if (construction == null)
                return null;

            List<ConstructionLayer> constructionLayers = construction.ConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;

            OpaqueConstructionAbridged result = new OpaqueConstructionAbridged(construction.Name, constructionLayers.ConvertAll(x => x.Name), construction.Name);
            return result;
        }

        public static OpaqueConstructionAbridged ToLadybugTools(this ApertureConstruction apertureConstruction)
        {
            if (apertureConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = apertureConstruction.FrameConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;



            OpaqueConstructionAbridged result = new OpaqueConstructionAbridged(apertureConstruction.Name, constructionLayers.ConvertAll(x => x.Name), apertureConstruction.Name);
            return result;
        }
    }
}