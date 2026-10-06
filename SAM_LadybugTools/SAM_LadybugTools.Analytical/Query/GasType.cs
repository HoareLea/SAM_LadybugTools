// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using SAM.Core;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static GasType? GasType(this GasMaterial gasMaterial)
        {
            if (gasMaterial == null)
                return null;

            // Explicit SAM gas type parameter wins over name-based detection
            if (gasMaterial.TryGetValue(GasMaterialParameter.DefaultGasType, out string defaultGasType) && !string.IsNullOrWhiteSpace(defaultGasType))
            {
                switch (defaultGasType.Trim().ToUpper())
                {
                    case "AIR":
                        return HoneybeeSchema.GasType.Air;

                    case "ARGON":
                        return HoneybeeSchema.GasType.Argon;

                    case "KRYPTON":
                        return HoneybeeSchema.GasType.Krypton;

                    case "XENON":
                        return HoneybeeSchema.GasType.Xenon;
                }
            }

            string name_gasMaterial = gasMaterial.Name;
            name_gasMaterial = name_gasMaterial?.ToUpper().Trim();

            List<GasType> gasTypes = new List<GasType>();
            foreach(GasType gasType in Enum.GetValues(typeof(GasType)))
            {
                string name_GasType = gasType.ToString().ToUpper().Trim();
                if (name_gasMaterial.Contains(name_GasType))
                    gasTypes.Add(gasType);
            }

            if (gasTypes.Count == 0)
                return null;

            if (gasTypes.Count != 1)
                gasTypes.Sort((x, y) => y.ToString().Length.CompareTo(x.ToString().Length));

            return gasTypes[0];
        }
    }
}