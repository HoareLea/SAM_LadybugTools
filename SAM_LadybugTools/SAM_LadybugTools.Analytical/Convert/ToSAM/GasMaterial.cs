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

            DefaultGasType defaultGasType = Query.DefaultGasType(energyWindowMaterialGas.GasType);

            Core.GasMaterial result = Create.GasMaterial(
                energyWindowMaterialGas.Identifier,
                energyWindowMaterialGas.GetType().Name,
                energyWindowMaterialGas.DisplayName,
                energyWindowMaterialGas.GasType.ToString(),
                double.NaN,
                double.NaN,
                double.NaN,
                double.NaN,
                energyWindowMaterialGas.Thickness,
                double.NaN,
                energyWindowMaterialGas.UValue,
                defaultGasType
                );

            return result;
        }

        public static Core.GasMaterial ToSAM_GasMaterial(this EnergyMaterial energyMaterial)
        {
            if (energyMaterial == null)
            {
                return null;
            }

            DefaultGasType defaultGasType = Analytical.Query.DefaultGasType(energyMaterial.Identifier, energyMaterial.DisplayName);

            Core.GasMaterial result = Create.GasMaterial(
                energyMaterial.Identifier,
                energyMaterial.GetType().Name,
                energyMaterial.DisplayName,
                null,
                energyMaterial.Conductivity,
                energyMaterial.SpecificHeat,
                energyMaterial.Density,
                double.NaN,
                energyMaterial.Thickness,
                double.NaN,
                energyMaterial.Thickness / energyMaterial.Conductivity,
                defaultGasType
                );

            return result;
        }
    }
}