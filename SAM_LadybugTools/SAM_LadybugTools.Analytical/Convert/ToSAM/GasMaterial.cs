using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Core.GasMaterial ToSAM(this EnergyWindowMaterialGas energyWindowMaterialGas)
        {
            if (energyWindowMaterialGas == null)
            {
                return null;
            }

            Core.GasMaterial result = Create.GasMaterial(
                energyWindowMaterialGas.Identifier,
                energyWindowMaterialGas.GetType().Name,
                energyWindowMaterialGas.DisplayName,
                energyWindowMaterialGas.GasType.ToString(),
                energyWindowMaterialGas.Thickness,
                double.NaN,
                energyWindowMaterialGas.UValue
                );

            return result;
        }

        public static Core.GasMaterial ToSAM_GasMaterial(this EnergyMaterial energyMaterial)
        {
            if (energyMaterial == null)
            {
                return null;
            }

            Core.GasMaterial result = Create.GasMaterial(
                energyMaterial.Identifier,
                energyMaterial.GetType().Name,
                energyMaterial.DisplayName,
                null,
                energyMaterial.Thickness,
                double.NaN,
                double.NaN
                );

            return result;
        }
    }
}