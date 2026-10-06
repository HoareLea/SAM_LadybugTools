// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
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

            // Restore SAM-specific values preserved in namespaced user_data
            if (Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGlazing, Core.LadybugTools.UserDataKeys.VapourDiffusionFactor, out double value))
            {
                result.SetValue(MaterialParameter.VapourDiffusionFactor, value);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGlazing, Core.LadybugTools.UserDataKeys.DefaultThickness, out value))
            {
                result.SetValue(Core.MaterialParameter.DefaultThickness, value);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGlazing, Core.LadybugTools.UserDataKeys.IsBlind, out bool isBlind))
            {
                result.SetValue(TransparentMaterialParameter.IsBlind, isBlind);
            }

            if (Query.TryGetSAMGuid(energyWindowMaterialGlazing, out System.Guid guid))
            {
                result = new Core.TransparentMaterial(result.Name, guid, result, result.DisplayName, result.Description);
            }

            return result;
        }
    }
}