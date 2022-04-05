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

            return new EnergyWindowMaterialGlazing(
                identifier: transparentMaterial.Name,
                displayName: transparentMaterial.DisplayName,
                userData: null,
                thickness: transparentMaterial.GetValue<double>(Core.MaterialParameter.DefaultThickness),
                solarTransmittance: transparentMaterial.GetValue<double>(TransparentMaterialParameter.SolarTransmittance),
                solarReflectance: transparentMaterial.GetValue<double>(TransparentMaterialParameter.InternalSolarReflectance),
                solarReflectanceBack: transparentMaterial.GetValue<double>(TransparentMaterialParameter.ExternalSolarReflectance),
                visibleTransmittance: transparentMaterial.GetValue<double>(TransparentMaterialParameter.LightTransmittance),
                visibleReflectance: transparentMaterial.GetValue<double>(TransparentMaterialParameter.InternalLightReflectance),
                visibleReflectanceBack: transparentMaterial.GetValue<double>(TransparentMaterialParameter.ExternalLightReflectance),
                infraredTransmittance: 0,
                emissivity: transparentMaterial.GetValue<double>(TransparentMaterialParameter.InternalEmissivity),
                emissivityBack: transparentMaterial.GetValue<double>(TransparentMaterialParameter.ExternalEmissivity),
                conductivity: transparentMaterial.ThermalConductivity,
                dirtCorrection: 1,
                solarDiffusing: false);
        }
    }
}