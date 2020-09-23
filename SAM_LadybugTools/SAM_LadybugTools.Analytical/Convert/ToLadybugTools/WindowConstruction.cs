using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static WindowConstruction ToLadybugTools_WindowConstruction(this ApertureConstruction apertureConstruction, MaterialLibrary materialLibrary)
        {
            if (apertureConstruction == null)
                return null;

            List<ConstructionLayer> constructionLayers = apertureConstruction.PaneConstructionLayers;
            if (constructionLayers == null)
                return null;

            List<AnyOf<EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas, EnergyWindowMaterialGasCustom, EnergyWindowMaterialGasMixture>> materials = new List<AnyOf<EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas, EnergyWindowMaterialGasCustom, EnergyWindowMaterialGasMixture>>();
            foreach (ConstructionLayer constructionLayer in constructionLayers)
            {
                IMaterial material = materialLibrary.GetMaterial(constructionLayer.Name);
                if (material == null)
                {
                    materials.Add(null);
                    continue;
                }

                if (material is GasMaterial)
                    materials.Add(((GasMaterial)material).ToLadybugTools_EnergyWindowMaterialGas());
                else if (material is TransparentMaterial)
                    materials.Add(((TransparentMaterial)material).ToLadybugTools());
            }

            WindowConstruction result = new WindowConstruction(apertureConstruction.Name, constructionLayers.ConvertAll(x => x.Name), materials, apertureConstruction.Name);

            return result;
        }
    }
}