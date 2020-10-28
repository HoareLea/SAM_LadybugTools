using HoneybeeSchema;
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static EnergyWindowMaterialGlazing ToLadybugTools(this TransparentMaterial transparentMaterial)
        {
            if (transparentMaterial == null || string.IsNullOrEmpty(transparentMaterial.Name))
                return null;

            //return new EnergyWindowMaterialGlazing(Query.PaneMaterialName(transparentMaterial), transparentMaterial.DisplayName, transparentMaterial.DefaultThickness(), transparentMaterial.SolarTransmittance(), transparentMaterial.InternalSolarReflectance(), transparentMaterial.ExternalSolarReflectance(), transparentMaterial.LightTransmittance(), transparentMaterial.InternalLightReflectance(), transparentMaterial.ExternalLightReflectance(),0, transparentMaterial.InternalEmissivity(), transparentMaterial.ExternalEmissivity(), transparentMaterial.ThermalConductivity, 1, false);
            return new EnergyWindowMaterialGlazing(
                transparentMaterial.Name, 
                transparentMaterial.DisplayName, 
                transparentMaterial.GetValue<double>(MaterialParameter.DefaultThickness), 
                transparentMaterial.GetValue<double>(TransparentMaterialParameter.SolarTransmittance), 
                transparentMaterial.GetValue<double>(TransparentMaterialParameter.InternalSolarReflectance), 
                transparentMaterial.GetValue<double>(TransparentMaterialParameter.ExternalSolarReflectance), 
                transparentMaterial.GetValue<double>(TransparentMaterialParameter.LightTransmittance), 
                transparentMaterial.GetValue<double>(TransparentMaterialParameter.InternalLightReflectance), 
                transparentMaterial.GetValue<double>(TransparentMaterialParameter.ExternalLightReflectance), 
                0, 
                transparentMaterial.GetValue<double>(TransparentMaterialParameter.InternalEmissivity), 
                transparentMaterial.GetValue<double>(TransparentMaterialParameter.ExternalEmissivity), 
                transparentMaterial.ThermalConductivity, 
                1, 
                false);
        }
    }
}