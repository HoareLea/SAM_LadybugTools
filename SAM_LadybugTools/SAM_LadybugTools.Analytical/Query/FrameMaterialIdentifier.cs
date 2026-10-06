// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        /// <summary>
        /// Deterministic Honeybee identifier for the EnergyWindowFrame material generated
        /// from an ApertureConstruction frame layer.
        /// </summary>
        public static string FrameMaterialIdentifier(this ApertureConstruction apertureConstruction)
        {
            if (apertureConstruction == null)
            {
                return null;
            }

            ConstructionLayer constructionLayer = apertureConstruction.FrameConstructionLayers?[0];
            if (constructionLayer == null || string.IsNullOrWhiteSpace(constructionLayer.Name))
            {
                return null;
            }

            return string.Format("{0}_Frame", constructionLayer.Name);
        }
    }
}
