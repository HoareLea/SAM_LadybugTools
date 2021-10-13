using HoneybeeSchema;
using HoneybeeSchema.Energy;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Construction ToSAM(this OpaqueConstructionAbridged opaqueConstructionAbridged, Core.MaterialLibrary materialLibrary = null)
        {
            if(opaqueConstructionAbridged == null)
            {
                return null;
            }

            List<ConstructionLayer> constructionLayers = Query.ConstructionLayers(materialLibrary, opaqueConstructionAbridged.Materials);

            Construction result = new Construction(opaqueConstructionAbridged.Identifier, constructionLayers);
            return result;
        }

        public static Construction ToSAM_Construction(this IConstruction construction, Core.MaterialLibrary materialLibrary = null)
        {
            if (construction == null)
            {
                return null;
            }

            if(construction is OpaqueConstructionAbridged)
            {
                return ((OpaqueConstructionAbridged)construction).ToSAM(materialLibrary);
            }

            return null;
        }
    }
}