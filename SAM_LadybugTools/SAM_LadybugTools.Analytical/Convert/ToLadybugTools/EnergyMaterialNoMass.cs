using HoneybeeSchema;
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static EnergyMaterialNoMass ToLadybugTools(this GasMaterial gasMaterial, double angle, double thickness = double.NaN)
        {
            if (gasMaterial == null || string.IsNullOrEmpty(gasMaterial.Name))
                return null;

            GasType? gasType = gasMaterial.GasType();
            if (gasType == null || !gasType.HasValue || gasType != GasType.Air)
                return null;

            double thickness_Temp = thickness;
            if (double.IsNaN(thickness_Temp))
                thickness_Temp = gasMaterial.GetValue<double>(Core.MaterialParameter.DefaultThickness);

            double airspaceThermalResistance = Analytical.Query.AirspaceThermalResistance(angle, thickness_Temp);
            if (double.IsNaN(airspaceThermalResistance))
                return null;

            return new EnergyMaterialNoMass(identifier: gasMaterial.Name, rValue: airspaceThermalResistance, displayName: gasMaterial.DisplayName);
        }
    }
}