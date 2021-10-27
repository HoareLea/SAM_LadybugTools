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

            return new EnergyMaterialNoMass(identifier: gasMaterial.Name, rValue: gasMaterial.GetValue<double>(GasMaterialParameter.HeatTransferCoefficient), displayName: gasMaterial.DisplayName);
        }
    }
}