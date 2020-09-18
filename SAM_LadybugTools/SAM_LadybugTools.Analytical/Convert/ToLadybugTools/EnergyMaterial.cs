using HoneybeeSchema;
using SAM.Core;
using SAM.Geometry.Spatial;
using System.Collections.Generic;
using SAM.Analytical;

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


        public static EnergyWindowMaterialGlazing ToLadybugTools(this TransparentMaterial transparentMaterial)
        {
            if (transparentMaterial == null || string.IsNullOrEmpty(transparentMaterial.Name))
                return null;

            return new EnergyWindowMaterialGlazing(transparentMaterial.Name, transparentMaterial.DisplayName, transparentMaterial.DefaultThickness(), transparentMaterial.SolarTransmittance(), transparentMaterial.InternalSolarReflectance(), transparentMaterial.ExternalSolarReflectance(), transparentMaterial.LightTransmittance(), transparentMaterial.InternalLightReflectance(), transparentMaterial.ExternalLightReflectance(),0, transparentMaterial.InternalEmissivity(), transparentMaterial.ExternalEmissivity(), transparentMaterial.ThermalConductivity, 1, false);
        }
    }
}