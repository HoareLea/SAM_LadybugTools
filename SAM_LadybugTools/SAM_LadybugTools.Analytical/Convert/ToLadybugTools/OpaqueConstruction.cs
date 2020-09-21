using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static OpaqueConstruction OpaqueConstruction(this Construction construction)
        {
            if (construction == null)
                return null;

            List<ConstructionLayer> constructionLayers = construction.ConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;

            OpaqueConstruction opaqueConstruction = new OpaqueConstruction(construction.Name, constructionLayers.ConvertAll(x => x.Name), null, construction.Name);
            return opaqueConstruction;
        }

        public static OpaqueConstruction OpaqueConstruction(this Construction construction, MaterialLibrary materialLibrary)
        {
            if (construction == null || materialLibrary == null)
                return null;

            List<ConstructionLayer> constructionLayers = construction.ConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;

            List<AnyOf<EnergyMaterial, EnergyMaterialNoMass>> materials = new List<AnyOf<EnergyMaterial, EnergyMaterialNoMass>>();
            foreach (ConstructionLayer constructionLayer in constructionLayers)
            {
                IMaterial material = materialLibrary.GetMaterial(constructionLayer.Name);
                if (material == null)
                {
                    materials.Add(null);
                    continue;
                }

                if (material is GasMaterial)
                    materials.Add(((GasMaterial)material).ToLadybugTools());
                else if (material is OpaqueMaterial)
                    materials.Add(((OpaqueMaterial)material).ToLadybugTools());
            }

            OpaqueConstruction opaqueConstruction = new OpaqueConstruction(construction.Name, constructionLayers.ConvertAll(x => x.Name), materials, construction.Name);
            return opaqueConstruction;

        }
    }
}