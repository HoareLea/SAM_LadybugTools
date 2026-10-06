// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Core;
using Xunit;

namespace SAM.Core.LadybugTools.Tests
{
    public static class UserDataTests
    {
        [Fact]
        public static void TryGetUserData_MissingDoubleKey_ShouldReturnFalseAndNaN()
        {
            HoneybeeSchema.EnergyWindowFrame frame = new HoneybeeSchema.EnergyWindowFrame("Frame", 0.05, 3.0);

            bool found = Query.TryGetUserData(frame, UserDataKeys.Density, out double value);

            Assert.False(found);
            Assert.True(double.IsNaN(value));
        }

        [Fact]
        public static void TryGetUserData_PresentDoubleKey_ShouldReturnValue()
        {
            HoneybeeSchema.EnergyWindowFrame frame = new HoneybeeSchema.EnergyWindowFrame("Frame", 0.05, 3.0);
            Modify.SetUserData(frame, UserDataKeys.Density, 7800.0);

            bool found = Query.TryGetUserData(frame, UserDataKeys.Density, out double value);

            Assert.True(found);
            Assert.Equal(7800.0, value, 9);
        }

        [Fact]
        public static void EnergyWindowFrame_NoUserData_ToSAM_ShouldLeaveDensityUndefined()
        {
            // A frame authored outside SAM carries no namespaced user_data; unknown physical
            // properties must stay NaN ("undefined") instead of defaulting to 0.
            HoneybeeSchema.EnergyWindowFrame frame = new HoneybeeSchema.EnergyWindowFrame("Frame", 0.05, 3.0);

            OpaqueMaterial material = SAM.Analytical.LadybugTools.Convert.ToSAM(frame);

            Assert.NotNull(material);
            Assert.True(double.IsNaN(material.Density));
            Assert.True(double.IsNaN(material.SpecificHeatCapacity));
        }

        [Fact]
        public static void EnergyWindowMaterialGas_NoUserData_ToSAM_ShouldFallBackToHoneybeeThickness()
        {
            // Regression: TryGetUserData left default(double) = 0 in the out value on a miss,
            // so the double.IsNaN fallback never fired and gas layers from non-SAM Honeybee
            // models imported with 0 thickness instead of the Honeybee layer thickness.
            HoneybeeSchema.EnergyWindowMaterialGas gas = new HoneybeeSchema.EnergyWindowMaterialGas(
                identifier: "Gas",
                displayName: null,
                userData: null,
                thickness: 0.0127,
                gasType: HoneybeeSchema.GasType.Air);

            GasMaterial material = SAM.Analytical.LadybugTools.Convert.ToSAM(gas);

            Assert.NotNull(material);
            Assert.Equal(0.0127, material.GetValue<double>(MaterialParameter.DefaultThickness), 9);
            Assert.True(double.IsNaN(material.Density));
        }
    }
}
