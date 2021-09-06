using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static OpaqueConstructionAbridged ToLadybugTools(this Construction construction, bool reverse = true)
        {
            if (construction == null)
                return null;

            List<ConstructionLayer> constructionLayers = construction.ConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;

            if (reverse)
            {
                constructionLayers.Reverse();
            }


            OpaqueConstructionAbridged result = new OpaqueConstructionAbridged(Query.UniqueName(construction, reverse), constructionLayers.ConvertAll(x => x.Name), construction.Name);
            return result;
        }

        public static OpaqueConstructionAbridged ToLadybugTools(this ApertureConstruction apertureConstruction)
        {
            if (apertureConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = apertureConstruction.PaneConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;

            constructionLayers.Reverse();

            OpaqueConstructionAbridged result = new OpaqueConstructionAbridged(Core.LadybugTools.Query.UniqueName(apertureConstruction), constructionLayers.ConvertAll(x => x.Name), apertureConstruction.Name);
            return result;
        }
    }
}