// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Modify
    {
        /// <summary>
        /// Writes the SAM-specific OpaqueMaterial data that HoneybeeSchema cannot represent
        /// into namespaced Honeybee user_data so a SAM -> Honeybee -> SAM round trip is lossless.
        /// </summary>
        public static void SetUserData(this HoneybeeSchema.IIDdBaseModel dDBaseModel, OpaqueMaterial opaqueMaterial)
        {
            if (dDBaseModel == null || opaqueMaterial == null)
            {
                return;
            }

            Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.Guid, opaqueMaterial.Guid.ToString());
            Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.MaterialType, opaqueMaterial.MaterialType.ToString());

            double value = opaqueMaterial.GetValue<double>(Core.MaterialParameter.DefaultThickness);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.DefaultThickness, value);
            }

            value = opaqueMaterial.GetValue<double>(MaterialParameter.VapourDiffusionFactor);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.VapourDiffusionFactor, value);
            }

            if (opaqueMaterial.TryGetValue(OpaqueMaterialParameter.IgnoreThermalTransmittanceCalculations, out bool ignoreThermalTransmittanceCalculations))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.IgnoreThermalTransmittanceCalculations, ignoreThermalTransmittanceCalculations);
            }

            value = opaqueMaterial.GetValue<double>(OpaqueMaterialParameter.InternalEmissivity);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.InternalEmissivity, value);
            }

            value = opaqueMaterial.GetValue<double>(OpaqueMaterialParameter.InternalSolarReflectance);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.InternalSolarReflectance, value);
            }

            value = opaqueMaterial.GetValue<double>(OpaqueMaterialParameter.InternalLightReflectance);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.InternalLightReflectance, value);
            }
        }

        /// <summary>
        /// Writes the SAM-specific GasMaterial data that HoneybeeSchema cannot represent
        /// into namespaced Honeybee user_data so a SAM -> Honeybee -> SAM round trip is lossless.
        /// </summary>
        public static void SetUserData(this HoneybeeSchema.IIDdBaseModel dDBaseModel, GasMaterial gasMaterial)
        {
            if (dDBaseModel == null || gasMaterial == null)
            {
                return;
            }

            Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.Guid, gasMaterial.Guid.ToString());
            Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.MaterialType, gasMaterial.MaterialType.ToString());

            double value = gasMaterial.GetValue<double>(Core.MaterialParameter.DefaultThickness);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.DefaultThickness, value);
            }

            value = gasMaterial.GetValue<double>(MaterialParameter.VapourDiffusionFactor);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.VapourDiffusionFactor, value);
            }

            value = gasMaterial.GetValue<double>(GasMaterialParameter.HeatTransferCoefficient);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.HeatTransferCoefficient, value);
            }

            if (gasMaterial.TryGetValue(GasMaterialParameter.DefaultGasType, out string defaultGasType) && !string.IsNullOrWhiteSpace(defaultGasType))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.DefaultGasType, defaultGasType);
            }

            if (!double.IsNaN(gasMaterial.Density))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.Density, gasMaterial.Density);
            }

            if (!double.IsNaN(gasMaterial.ThermalConductivity))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.ThermalConductivity, gasMaterial.ThermalConductivity);
            }

            if (!double.IsNaN(gasMaterial.SpecificHeatCapacity))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.SpecificHeatCapacity, gasMaterial.SpecificHeatCapacity);
            }

            if (!double.IsNaN(gasMaterial.DynamicViscosity))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.DynamicViscosity, gasMaterial.DynamicViscosity);
            }
        }

        /// <summary>
        /// Writes the SAM-specific TransparentMaterial data that HoneybeeSchema cannot represent
        /// into namespaced Honeybee user_data so a SAM -> Honeybee -> SAM round trip is lossless.
        /// </summary>
        public static void SetUserData(this HoneybeeSchema.IIDdBaseModel dDBaseModel, TransparentMaterial transparentMaterial)
        {
            if (dDBaseModel == null || transparentMaterial == null)
            {
                return;
            }

            Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.Guid, transparentMaterial.Guid.ToString());
            Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.MaterialType, transparentMaterial.MaterialType.ToString());

            double value = transparentMaterial.GetValue<double>(Core.MaterialParameter.DefaultThickness);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.DefaultThickness, value);
            }

            value = transparentMaterial.GetValue<double>(MaterialParameter.VapourDiffusionFactor);
            if (!double.IsNaN(value))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.VapourDiffusionFactor, value);
            }

            if (transparentMaterial.TryGetValue(TransparentMaterialParameter.IsBlind, out bool isBlind))
            {
                Core.LadybugTools.Modify.SetUserData(dDBaseModel, Core.LadybugTools.UserDataKeys.IsBlind, isBlind);
            }
        }
    }
}
