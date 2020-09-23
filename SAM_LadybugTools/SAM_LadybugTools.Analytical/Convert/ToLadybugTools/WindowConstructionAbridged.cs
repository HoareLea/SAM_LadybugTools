using HoneybeeSchema;
using SAM.Core;
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

            WindowConstructionAbridged result = new WindowConstructionAbridged(apertureConstruction.Name, constructionLayers.ConvertAll(x => x.Name), apertureConstruction.Name);

            return result;
        }
    }
}