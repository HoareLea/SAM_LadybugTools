using HoneybeeSchema;
using HoneybeeSchema.Energy;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ApertureConstruction ToSAM(this WindowConstructionAbridged windowConstructionAbridged, Core.MaterialLibrary materialLibrary = null)
        {
            if(windowConstructionAbridged == null)
            {
                return null;
            }

            List<ConstructionLayer> constructionLayers = Query.ConstructionLayers(materialLibrary, windowConstructionAbridged.Materials);

            ApertureConstruction result = new ApertureConstruction(System.Guid.NewGuid(), windowConstructionAbridged.Identifier, ApertureType.Window, constructionLayers);
            return result;
        }

        public static ApertureConstruction ToSAM_ApertureConstruction(this IConstruction construction, Core.MaterialLibrary materialLibrary = null)
        {
            if (construction == null)
            {
                return null;
            }

            if(construction is WindowConstructionAbridged)
            {
                return ((WindowConstructionAbridged)construction).ToSAM(materialLibrary);
            }

            return null;
        }
    }
}