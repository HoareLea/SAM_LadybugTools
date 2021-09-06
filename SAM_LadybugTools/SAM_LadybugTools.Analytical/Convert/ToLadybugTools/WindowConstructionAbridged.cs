using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static WindowConstructionAbridged ToLadybugTools_WindowConstructionAbridged(this ApertureConstruction apertureConstruction, bool reverse = true)
        {
            if (apertureConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = apertureConstruction.PaneConstructionLayers;
            if (constructionLayers == null)
                return null;

            if(reverse)
            {
                constructionLayers.Reverse();
            }

            WindowConstructionAbridged result = new WindowConstructionAbridged(Query.UniqueName(apertureConstruction, reverse), constructionLayers.ConvertAll(x => x.Name), apertureConstruction.Name);

            return result;
        }
    }
}