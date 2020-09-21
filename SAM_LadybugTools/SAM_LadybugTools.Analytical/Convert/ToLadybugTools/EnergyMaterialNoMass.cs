using HoneybeeSchema;
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static EnergyMaterialNoMass ToLadybugTools(this GasMaterial gasMaterial)
        {
            if (gasMaterial == null || string.IsNullOrEmpty(gasMaterial.Name))
                return null;

            GasType? gasType = gasMaterial.GasType();
            if (gasType == null || !gasType.HasValue)
                return null;

            return new EnergyMaterialNoMass(gasMaterial.Name, double.NaN, gasMaterial.DisplayName);
        }
    }
}