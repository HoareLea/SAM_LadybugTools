using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static WindowConstructionAbridged ToLadybugTools_WindowConstructionAbridged(this ApertureConstruction apertureConstruction)
        {
            if (apertureConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = apertureConstruction.PaneConstructionLayers;
            if (constructionLayers == null)
                return null;

            constructionLayers.Reverse();

            WindowConstructionAbridged result = new WindowConstructionAbridged(Core.LadybugTools.Query.UniqueName(apertureConstruction), constructionLayers.ConvertAll(x => x.Name), apertureConstruction.Name);

            return result;
        }
    }
}