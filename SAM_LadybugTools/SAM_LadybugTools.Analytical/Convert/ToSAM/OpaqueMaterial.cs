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
                energyMaterial.Density,
                energyMaterial.Thickness,
                double.NaN,
                1 - energyMaterial.SolarAbsorptance,
                1 - energyMaterial.SolarAbsorptance,
                1 - energyMaterial.VisibleAbsorptance,
                1 - energyMaterial.VisibleAbsorptance,
                energyMaterial.ThermalAbsorptance,
                energyMaterial.ThermalAbsorptance,
                false
                );

            return result;
        }

        public static Core.OpaqueMaterial ToSAM(this EnergyMaterialNoMass energyMaterialNoMass)
        {
            Core.OpaqueMaterial result = null;

            Core.MaterialLibrary materialLibrary = Analytical.Query.DefaultMaterialLibrary();
            if(materialLibrary != null)
            {
                result = materialLibrary.GetMaterial<Core.OpaqueMaterial>("I00_Mineral Wool batt_25kg/m3_0.038W/mK");
                if(result != null)
                {
                    result = new Core.OpaqueMaterial(energyMaterialNoMass.Identifier, System.Guid.NewGuid(), result, energyMaterialNoMass.DisplayName, result.Name);
                    result.SetValue(MaterialParameter.DefaultThickness, energyMaterialNoMass.RValue * result.ThermalConductivity);
                }
            }

            if(result == null)
            {
                result = new Core.OpaqueMaterial(energyMaterialNoMass.Identifier, null, energyMaterialNoMass.DisplayName, null, double.NaN, double.NaN, double.NaN);
                result.SetValue(MaterialParameter.DefaultThickness, energyMaterialNoMass.RValue * 0.038);
            }

            return result;
        }
    }
}