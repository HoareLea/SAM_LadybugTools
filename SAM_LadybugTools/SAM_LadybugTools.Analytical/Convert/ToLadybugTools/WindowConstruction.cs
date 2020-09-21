using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static WindowConstruction ToLadybugTools_WindowConstruction(this ApertureConstruction apertureConstruction)
        {
            if (apertureConstruction == null)
                return null;

            Construction paneConstruction = apertureConstruction.PaneConstruction;
            if (paneConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = paneConstruction.ConstructionLayers;
            if (constructionLayers == null)
                return null;

            WindowConstruction result = new WindowConstruction(apertureConstruction.Name, constructionLayers.ConvertAll(x => x.Name), null, apertureConstruction.Name);

            return result;
        }

        public static WindowConstruction ToLadybugTools_WindowConstruction(this ApertureConstruction apertureConstruction, MaterialLibrary materialLibrary)
        {
            if (apertureConstruction == null)
                return null;

            Construction paneConstruction = apertureConstruction.PaneConstruction;
            if (paneConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = paneConstruction.ConstructionLayers;
            if (constructionLayers == null)
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

            WindowConstruction result = new WindowConstruction(apertureConstruction.Name, constructionLayers.ConvertAll(x => x.Name), materials, apertureConstruction.Name);

            return result;
        }
    }
}