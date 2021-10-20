using HoneybeeSchema;
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static EnergyMaterial ToLadybugTools(this OpaqueMaterial opaqueMaterial)
        {
            if (opaqueMaterial == null || string.IsNullOrEmpty(opaqueMaterial.Name))
                return null;

            return new EnergyMaterial(
                identifier: opaqueMaterial.Name,
                thickness: opaqueMaterial.GetValue<double>(MaterialParameter.DefaultThickness),
                conductivity: opaqueMaterial.ThermalConductivity,
                density: opaqueMaterial.Density,
                specificHeat: opaqueMaterial.SpecificHeatCapacity,
                displayName: opaqueMaterial.DisplayName,
                userData: null,
                roughness: Roughness.MediumSmooth,
                thermalAbsorptance: opaqueMaterial.GetValue<double>(OpaqueMaterialParameter.ExternalEmissivity),
                solarAbsorptance: 1 - opaqueMaterial.GetValue<double>(OpaqueMaterialParameter.ExternalSolarReflectance),
                visibleAbsorptance: 1 - opaqueMaterial.GetValue<double>(OpaqueMaterialParameter.ExternalLightReflectance));
        }
    }
}