// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
namespace SAM.Core.LadybugTools
{
    /// <summary>
    /// Namespaced user_data keys used to carry SAM-specific data through Honeybee objects.
    /// Keys are prefixed with "SAM." so they cannot collide with native Honeybee user data.
    /// </summary>
    public static class UserDataKeys
    {
        public const string Guid = "SAM.Guid";
        public const string Name = "SAM.Name";
        public const string Description = "SAM.Description";
        public const string LocationName = "SAM.Location.Name";
        public const string LocationLatitude = "SAM.Location.Latitude";
        public const string LocationLongitude = "SAM.Location.Longitude";
        public const string LocationElevation = "SAM.Location.Elevation";
        public const string ProfileLibraryName = "SAM.ProfileLibraryName";

        public const string MaterialType = "SAM.MaterialType";
        public const string VapourDiffusionFactor = "SAM.VapourDiffusionFactor";
        public const string DefaultThickness = "SAM.DefaultThickness";
        public const string HeatTransferCoefficient = "SAM.HeatTransferCoefficient";
        public const string DefaultGasType = "SAM.DefaultGasType";
        public const string Density = "SAM.Density";
        public const string ThermalConductivity = "SAM.ThermalConductivity";
        public const string SpecificHeatCapacity = "SAM.SpecificHeatCapacity";
        public const string DynamicViscosity = "SAM.DynamicViscosity";
        public const string IgnoreThermalTransmittanceCalculations = "SAM.IgnoreThermalTransmittanceCalculations";
        public const string InternalEmissivity = "SAM.InternalEmissivity";
        public const string InternalSolarReflectance = "SAM.InternalSolarReflectance";
        public const string InternalLightReflectance = "SAM.InternalLightReflectance";
        public const string IsBlind = "SAM.IsBlind";

        public const string ConstructionLayers = "SAM.ConstructionLayers";
        public const string PaneConstructionLayers = "SAM.PaneConstructionLayers";
        public const string FrameConstructionLayers = "SAM.FrameConstructionLayers";
        public const string DefaultPanelType = "SAM.DefaultPanelType";
        public const string ConstructionName = "SAM.ConstructionName";

        public const string ConstructionLayerName = "Name";
        public const string ConstructionLayerThickness = "Thickness";
    }
}
