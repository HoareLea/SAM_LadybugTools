﻿using HoneybeeSchema;
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static EnergyWindowMaterialGas ToLadybugTools_EnergyWindowMaterialGas(this GasMaterial gasMaterial, double thickness)
        {
            if (gasMaterial == null || string.IsNullOrEmpty(gasMaterial.Name))
                return null;

            GasType? gasType = gasMaterial.GasType();
            if (gasType == null || !gasType.HasValue)
                return null;

            return new EnergyWindowMaterialGas(
                identifier: gasMaterial.Name, 
                displayName: gasMaterial.DisplayName, 
                userData: null,
                thickness: thickness, 
                gasType: gasType.Value);
        }
    }
}