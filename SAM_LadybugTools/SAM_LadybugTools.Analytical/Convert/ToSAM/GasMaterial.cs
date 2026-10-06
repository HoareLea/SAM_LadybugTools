// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Core.GasMaterial ToSAM(this EnergyWindowMaterialGas energyWindowMaterialGas)
        {
            if (energyWindowMaterialGas == null)
            {
                return null;
            }

            DefaultGasType defaultGasType = Query.DefaultGasType(energyWindowMaterialGas.GasType);

            // SAM round-trip values preserved in namespaced user_data take precedence
            bool hasSAMUserData = Query.TryGetSAMMaterialType(energyWindowMaterialGas, out _);

            Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGas, Core.LadybugTools.UserDataKeys.ThermalConductivity, out double thermalConductivity);
            Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGas, Core.LadybugTools.UserDataKeys.SpecificHeatCapacity, out double specificHeatCapacity);
            Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGas, Core.LadybugTools.UserDataKeys.Density, out double density);
            Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGas, Core.LadybugTools.UserDataKeys.DynamicViscosity, out double dynamicViscosity);
            Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGas, Core.LadybugTools.UserDataKeys.DefaultThickness, out double defaultThickness);
            Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGas, Core.LadybugTools.UserDataKeys.VapourDiffusionFactor, out double vapourDiffusionFactor);

            if (double.IsNaN(defaultThickness))
            {
                defaultThickness = energyWindowMaterialGas.Thickness;
            }

            double heatTransferCoefficient;
            if (!Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGas, Core.LadybugTools.UserDataKeys.HeatTransferCoefficient, out heatTransferCoefficient))
            {
                heatTransferCoefficient = energyWindowMaterialGas.UValue;
            }

            Core.GasMaterial result = Create.GasMaterial(
                energyWindowMaterialGas.Identifier,
                energyWindowMaterialGas.GetType().Name,
                energyWindowMaterialGas.DisplayName,
                null,
                thermalConductivity,
                specificHeatCapacity,
                density,
                dynamicViscosity,
                defaultThickness,
                vapourDiffusionFactor,
                heatTransferCoefficient,
                defaultGasType
                );

            // SAM-origin materials only carry Default Gas Type when it existed on the source material
            if (hasSAMUserData && !Core.LadybugTools.Query.TryGetUserData(energyWindowMaterialGas, Core.LadybugTools.UserDataKeys.DefaultGasType, out string _))
            {
                result.RemoveValue(GasMaterialParameter.DefaultGasType);
            }

            if (Query.TryGetSAMGuid(energyWindowMaterialGas, out System.Guid guid))
            {
                result = new Core.GasMaterial(result.Name, guid, result, result.DisplayName, result.Description);
            }

            return result;
        }

        public static Core.GasMaterial ToSAM_GasMaterial(this EnergyMaterial energyMaterial)
        {
            if (energyMaterial == null)
            {
                return null;
            }

            DefaultGasType defaultGasType = Analytical.Query.DefaultGasType(energyMaterial.Identifier, energyMaterial.DisplayName);

            bool hasSAMUserData = Query.TryGetSAMMaterialType(energyMaterial, out _);

            Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.DynamicViscosity, out double dynamicViscosity);
            Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.DefaultThickness, out double defaultThickness);
            Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.VapourDiffusionFactor, out double vapourDiffusionFactor);

            if (double.IsNaN(defaultThickness))
            {
                defaultThickness = energyMaterial.Thickness;
            }

            double heatTransferCoefficient;
            if (!Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.HeatTransferCoefficient, out heatTransferCoefficient))
            {
                // Heat transfer coefficient [W/m2K] is the reciprocal of the layer resistance [m2K/W]
                heatTransferCoefficient = double.IsNaN(energyMaterial.Thickness) || energyMaterial.Thickness == 0 || double.IsNaN(energyMaterial.Conductivity)
                    ? double.NaN
                    : energyMaterial.Conductivity / energyMaterial.Thickness;
            }

            Core.GasMaterial result = Create.GasMaterial(
                energyMaterial.Identifier,
                energyMaterial.GetType().Name,
                energyMaterial.DisplayName,
                null,
                energyMaterial.Conductivity,
                energyMaterial.SpecificHeat,
                energyMaterial.Density,
                dynamicViscosity,
                defaultThickness,
                vapourDiffusionFactor,
                heatTransferCoefficient,
                defaultGasType
                );

            if (hasSAMUserData && !Core.LadybugTools.Query.TryGetUserData(energyMaterial, Core.LadybugTools.UserDataKeys.DefaultGasType, out string _))
            {
                result.RemoveValue(GasMaterialParameter.DefaultGasType);
            }

            if (Query.TryGetSAMGuid(energyMaterial, out System.Guid guid))
            {
                result = new Core.GasMaterial(result.Name, guid, result, result.DisplayName, result.Description);
            }

            return result;
        }

        /// <summary>
        /// Restores a SAM air gap that travelled through Honeybee as EnergyMaterialNoMass.
        /// Requires SAM round-trip metadata in user_data; physical properties are read from
        /// the namespaced values because EnergyMaterialNoMass carries only an R-value.
        /// </summary>
        public static Core.GasMaterial ToSAM_GasMaterial(this EnergyMaterialNoMass energyMaterialNoMass)
        {
            if (energyMaterialNoMass == null)
            {
                return null;
            }

            if (!Query.TryGetSAMMaterialType(energyMaterialNoMass, out Core.MaterialType materialType) || materialType != Core.MaterialType.Gas)
            {
                return null;
            }

            Core.LadybugTools.Query.TryGetUserData(energyMaterialNoMass, Core.LadybugTools.UserDataKeys.ThermalConductivity, out double thermalConductivity);
            Core.LadybugTools.Query.TryGetUserData(energyMaterialNoMass, Core.LadybugTools.UserDataKeys.SpecificHeatCapacity, out double specificHeatCapacity);
            Core.LadybugTools.Query.TryGetUserData(energyMaterialNoMass, Core.LadybugTools.UserDataKeys.Density, out double density);
            Core.LadybugTools.Query.TryGetUserData(energyMaterialNoMass, Core.LadybugTools.UserDataKeys.DynamicViscosity, out double dynamicViscosity);
            Core.LadybugTools.Query.TryGetUserData(energyMaterialNoMass, Core.LadybugTools.UserDataKeys.DefaultThickness, out double defaultThickness);
            Core.LadybugTools.Query.TryGetUserData(energyMaterialNoMass, Core.LadybugTools.UserDataKeys.VapourDiffusionFactor, out double vapourDiffusionFactor);
            Core.LadybugTools.Query.TryGetUserData(energyMaterialNoMass, Core.LadybugTools.UserDataKeys.HeatTransferCoefficient, out double heatTransferCoefficient);

            DefaultGasType defaultGasType = DefaultGasType.Undefined;
            if (Core.LadybugTools.Query.TryGetUserData(energyMaterialNoMass, Core.LadybugTools.UserDataKeys.DefaultGasType, out string defaultGasTypeText) && !string.IsNullOrWhiteSpace(defaultGasTypeText))
            {
                if (!System.Enum.TryParse(defaultGasTypeText, true, out defaultGasType))
                {
                    defaultGasType = DefaultGasType.Undefined;
                }
            }
            else
            {
                defaultGasType = Analytical.Query.DefaultGasType(energyMaterialNoMass.Identifier, energyMaterialNoMass.DisplayName);
            }

            Core.GasMaterial result = Create.GasMaterial(
                energyMaterialNoMass.Identifier,
                energyMaterialNoMass.GetType().Name,
                energyMaterialNoMass.DisplayName,
                null,
                thermalConductivity,
                specificHeatCapacity,
                density,
                dynamicViscosity,
                defaultThickness,
                vapourDiffusionFactor,
                heatTransferCoefficient,
                defaultGasType
                );

            if (!Core.LadybugTools.Query.TryGetUserData(energyMaterialNoMass, Core.LadybugTools.UserDataKeys.DefaultGasType, out string _))
            {
                result.RemoveValue(GasMaterialParameter.DefaultGasType);
            }

            if (Query.TryGetSAMGuid(energyMaterialNoMass, out System.Guid guid))
            {
                result = new Core.GasMaterial(result.Name, guid, result, result.DisplayName, result.Description);
            }

            return result;
        }
    }
}
