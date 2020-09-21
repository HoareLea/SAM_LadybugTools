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

            return new EnergyMaterial(opaqueMaterial.Name, opaqueMaterial.DefaultThickness(), opaqueMaterial.ThermalConductivity, opaqueMaterial.Density, opaqueMaterial.SpecificHeatCapacity, opaqueMaterial.DisplayName, null, opaqueMaterial.ExternalEmissivity(), 1 - opaqueMaterial.ExternalSolarReflectance(), 1 - opaqueMaterial.ExternalLightReflectance());
        }
    }
}