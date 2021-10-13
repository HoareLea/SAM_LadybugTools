using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Core.OpaqueMaterial ToSAM(this EnergyMaterial energyMaterial)
        {
            if (energyMaterial == null)
            {
                return null;
            }

            Core.OpaqueMaterial result = Create.OpaqueMaterial(
                energyMaterial.Identifier,
                energyMaterial.GetType().Name,
                energyMaterial.DisplayName,
                null,
                energyMaterial.Conductivity,
                energyMaterial.SpecificHeat,
                double.NaN,
                energyMaterial.Thickness,
                double.NaN,
                1 - energyMaterial.SolarAbsorptance,
                double.NaN,
                1 - energyMaterial.VisibleAbsorptance,
                double.NaN,
                energyMaterial.ThermalAbsorptance,
                double.NaN,
                false
                );

            return result;
        }
    }
}