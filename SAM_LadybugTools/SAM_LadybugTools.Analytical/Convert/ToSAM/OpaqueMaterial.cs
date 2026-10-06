// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
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

            // Restore SAM-specific values preserved in namespaced user_data
            if (Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.VapourDiffusionFactor, out double value))
            {
                result.SetValue(MaterialParameter.VapourDiffusionFactor, value);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.DefaultThickness, out value))
            {
                result.SetValue(Core.MaterialParameter.DefaultThickness, value);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.IgnoreThermalTransmittanceCalculations, out bool ignore))
            {
                result.SetValue(OpaqueMaterialParameter.IgnoreThermalTransmittanceCalculations, ignore);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.InternalEmissivity, out value))
            {
                result.SetValue(OpaqueMaterialParameter.InternalEmissivity, value);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.InternalSolarReflectance, out value))
            {
                result.SetValue(OpaqueMaterialParameter.InternalSolarReflectance, value);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.InternalLightReflectance, out value))
            {
                result.SetValue(OpaqueMaterialParameter.InternalLightReflectance, value);
            }

            if (Query.TryGetSAMGuid(energyMaterial, out System.Guid guid))
            {
                result = new Core.OpaqueMaterial(result.Name, guid, result, result.DisplayName, result.Description);
            }

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
                    result.SetValue(Core.MaterialParameter.DefaultThickness, energyMaterialNoMass.RValue * result.ThermalConductivity);
                }
            }

            if(result == null)
            {
                result = new Core.OpaqueMaterial(energyMaterialNoMass.Identifier, null, energyMaterialNoMass.DisplayName, null, double.NaN, double.NaN, double.NaN);
                result.SetValue(Core.MaterialParameter.DefaultThickness, energyMaterialNoMass.RValue * 0.038);
            }

            return result;
        }

        /// <summary>
        /// Converts a Honeybee frame material to a SAM opaque material so window frames remain
        /// representable: conductivity = conductance * width, thickness = width. Density, specific
        /// heat and the remaining SAM properties are restored from namespaced user_data written by
        /// ToLadybugTools_EnergyWindowFrame, because HoneybeeSchema does not carry them natively.
        /// </summary>
        public static Core.OpaqueMaterial ToSAM(this EnergyWindowFrame energyWindowFrame)
        {
            if (energyWindowFrame == null)
            {
                return null;
            }

            double thermalConductivity = double.NaN;
            if (!double.IsNaN(energyWindowFrame.Conductance) && !double.IsNaN(energyWindowFrame.Width) && energyWindowFrame.Width > 0)
            {
                thermalConductivity = energyWindowFrame.Conductance * energyWindowFrame.Width;
            }

            double value = double.NaN;

            Core.LadybugTools.Query.TryGetUserData(energyWindowFrame, Core.LadybugTools.UserDataKeys.Density, out double density);
            Core.LadybugTools.Query.TryGetUserData(energyWindowFrame, Core.LadybugTools.UserDataKeys.SpecificHeatCapacity, out double specificHeatCapacity);

            Core.OpaqueMaterial result = new Core.OpaqueMaterial(energyWindowFrame.Identifier, null, energyWindowFrame.DisplayName, null, thermalConductivity, specificHeatCapacity, density);
            result.SetValue(Core.MaterialParameter.DefaultThickness, energyWindowFrame.Width);

            if (Core.LadybugTools.Query.TryGetUserData(energyWindowFrame, Core.LadybugTools.UserDataKeys.VapourDiffusionFactor, out value))
            {
                result.SetValue(MaterialParameter.VapourDiffusionFactor, value);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyWindowFrame, Core.LadybugTools.UserDataKeys.IgnoreThermalTransmittanceCalculations, out bool ignore))
            {
                result.SetValue(OpaqueMaterialParameter.IgnoreThermalTransmittanceCalculations, ignore);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyWindowFrame, Core.LadybugTools.UserDataKeys.InternalEmissivity, out value))
            {
                result.SetValue(OpaqueMaterialParameter.InternalEmissivity, value);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyWindowFrame, Core.LadybugTools.UserDataKeys.InternalSolarReflectance, out value))
            {
                result.SetValue(OpaqueMaterialParameter.InternalSolarReflectance, value);
            }

            if (Core.LadybugTools.Query.TryGetUserData(energyWindowFrame, Core.LadybugTools.UserDataKeys.InternalLightReflectance, out value))
            {
                result.SetValue(OpaqueMaterialParameter.InternalLightReflectance, value);
            }

            if (Query.TryGetSAMGuid(energyWindowFrame, out System.Guid guid))
            {
                result = new Core.OpaqueMaterial(result.Name, guid, result, result.DisplayName, result.Description);
            }

            return result;
        }
    }
}