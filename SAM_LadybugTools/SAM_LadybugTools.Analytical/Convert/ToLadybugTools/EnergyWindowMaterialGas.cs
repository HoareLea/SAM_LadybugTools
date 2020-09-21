using HoneybeeSchema;
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static EnergyWindowMaterialGas ToLadybugTools_EnergyWindowMaterialGas(this GasMaterial gasMaterial)
        {
            if (gasMaterial == null || string.IsNullOrEmpty(gasMaterial.Name))
                return null;

            GasType? gasType = gasMaterial.GasType();
            if (gasType == null || !gasType.HasValue)
                return null;

            return new EnergyWindowMaterialGas(Query.PaneMaterialName(gasMaterial), gasMaterial.DisplayName, gasMaterial.DefaultThickness(), gasType);
        }
    }
}