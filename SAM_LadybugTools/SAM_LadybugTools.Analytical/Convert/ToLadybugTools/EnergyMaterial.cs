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
                opaqueMaterial.Name,
                opaqueMaterial.GetValue<double>(MaterialParameter.DefaultThickness),
                opaqueMaterial.ThermalConductivity,
                opaqueMaterial.Density,
                opaqueMaterial.SpecificHeatCapacity,
                opaqueMaterial.DisplayName,
                0,
                opaqueMaterial.GetValue<double>(OpaqueMaterialParameter.ExternalEmissivity),
                1 - opaqueMaterial.GetValue<double>(OpaqueMaterialParameter.ExternalSolarReflectance),
                1 - opaqueMaterial.GetValue<double>(OpaqueMaterialParameter.ExternalLightReflectance));
        }
    }
}