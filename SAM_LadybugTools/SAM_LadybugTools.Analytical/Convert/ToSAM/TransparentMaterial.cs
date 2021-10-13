using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Core.TransparentMaterial ToSAM(this EnergyWindowMaterialGlazing energyWindowMaterialGlazing)
        {
            if(energyWindowMaterialGlazing == null)
            {
                return null;
            }

            double externalSolarReflectance = double.NaN;
            if (energyWindowMaterialGlazing.SolarReflectanceBack.Obj is double)
                externalSolarReflectance = (double)energyWindowMaterialGlazing.SolarReflectanceBack.Obj;

            double externalLightReflectance = double.NaN;
            if (energyWindowMaterialGlazing.VisibleReflectanceBack.Obj is double)
                externalLightReflectance = (double)energyWindowMaterialGlazing.VisibleReflectanceBack.Obj;

            Core.TransparentMaterial result = Create.TransparentMaterial(
                energyWindowMaterialGlazing.Identifier,
                energyWindowMaterialGlazing.GetType().Name,
                energyWindowMaterialGlazing.DisplayName,
                null,
                energyWindowMaterialGlazing.Conductivity,
                energyWindowMaterialGlazing.Thickness,
                double.NaN,
                energyWindowMaterialGlazing.SolarTransmittance,
                energyWindowMaterialGlazing.VisibleTransmittance,
                externalSolarReflectance,
                energyWindowMaterialGlazing.SolarReflectance,
                externalLightReflectance,
                energyWindowMaterialGlazing.VisibleReflectance,
                energyWindowMaterialGlazing.EmissivityBack,
                energyWindowMaterialGlazing.Emissivity,
                false
                );

            return result;
        }
    }
}