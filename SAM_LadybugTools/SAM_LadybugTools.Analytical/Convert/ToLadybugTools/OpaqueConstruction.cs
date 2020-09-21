using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static OpaqueConstruction ToLadybugTools(this Construction construction)
        {
            if (construction == null)
                return null;

            List<ConstructionLayer> constructionLayers = construction.ConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;

            OpaqueConstruction opaqueConstruction = new OpaqueConstruction(construction.Name, constructionLayers.ConvertAll(x => x.Name), null, construction.Name);
            return opaqueConstruction;
        }

        public static OpaqueConstruction ToLadybugTools(this Construction construction, MaterialLibrary materialLibrary)
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

        public static OpaqueConstruction ToLadybugTools(this ApertureConstruction apertureConstruction)
        {
            if (apertureConstruction == null)
                return null;

            Construction frameConstruction = apertureConstruction.FrameConstruction;
            if (frameConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = frameConstruction.ConstructionLayers;
            if (constructionLayers == null || constructionLayers.Count == 0)
                return null;

            OpaqueConstruction opaqueConstruction = new OpaqueConstruction(apertureConstruction.Name, constructionLayers.ConvertAll(x => x.Name), null, apertureConstruction.Name);
            return opaqueConstruction;
        }

        public static OpaqueConstruction ToLadybugTools(this ApertureConstruction apertureConstruction, MaterialLibrary materialLibrary)
        {
            if (apertureConstruction == null || materialLibrary == null)
                return null;

            Construction frameConstruction = apertureConstruction.FrameConstruction;
            if (frameConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = frameConstruction.ConstructionLayers;
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

            OpaqueConstruction opaqueConstruction = new OpaqueConstruction(apertureConstruction.Name, constructionLayers.ConvertAll(x => x.Name), materials, apertureConstruction.Name);
            return opaqueConstruction;

        }
    }
}