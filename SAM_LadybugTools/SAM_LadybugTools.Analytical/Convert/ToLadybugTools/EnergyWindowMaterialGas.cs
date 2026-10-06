// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
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

            EnergyWindowMaterialGas result = new EnergyWindowMaterialGas(
                identifier: gasMaterial.Name,
                displayName: gasMaterial.DisplayName,
                userData: null,
                thickness: gasMaterial.GetValue<double>(Core.MaterialParameter.DefaultThickness),
                gasType: gasType.Value);

            result.SetUserData(gasMaterial);

            return result;
        }
    }
}