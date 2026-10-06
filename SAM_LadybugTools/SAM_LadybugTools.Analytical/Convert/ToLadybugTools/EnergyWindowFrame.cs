// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using SAM.Core;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        /// <summary>
        /// Best-effort Honeybee frame material from the first SAM frame construction layer.
        /// The full SAM frame construction is preserved separately in user_data; this material
        /// keeps the Honeybee model thermally usable (conductance = conductivity / thickness).
        /// </summary>
        public static EnergyWindowFrame ToLadybugTools_EnergyWindowFrame(this ApertureConstruction apertureConstruction, MaterialLibrary materialLibrary)
        {
            if (apertureConstruction == null)
            {
                return null;
            }

            ConstructionLayer constructionLayer = apertureConstruction.FrameConstructionLayers?[0];
            if (constructionLayer == null || double.IsNaN(constructionLayer.Thickness) || constructionLayer.Thickness <= 0)
            {
                return null;
            }

            IMaterial material = constructionLayer.Material(materialLibrary);
            if (material == null)
            {
                return null;
            }

            double conductance = double.NaN;
            if (material is Material material_Core && !double.IsNaN(material_Core.ThermalConductivity) && material_Core.ThermalConductivity > 0)
            {
                conductance = material_Core.ThermalConductivity / constructionLayer.Thickness;
            }

            if (double.IsNaN(conductance) || conductance <= 0)
            {
                return null;
            }

            string identifier = Query.FrameMaterialIdentifier(apertureConstruction);
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return null;
            }

            EnergyWindowFrame result = new EnergyWindowFrame(identifier: identifier, width: constructionLayer.Thickness, conductance: conductance);

            // EnergyWindowFrame carries only width and conductance; preserve the remaining SAM
            // material data in user_data so it survives when Honeybee prunes the unreferenced
            // base material from the model material list
            if (material is OpaqueMaterial opaqueMaterial)
            {
                result.SetUserData(opaqueMaterial);
            }

            if (material is Material material_Base)
            {
                if (!double.IsNaN(material_Base.Density))
                {
                    Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.Density, material_Base.Density);
                }

                if (!double.IsNaN(material_Base.SpecificHeatCapacity))
                {
                    Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.SpecificHeatCapacity, material_Base.SpecificHeatCapacity);
                }

                if (!double.IsNaN(material_Base.ThermalConductivity))
                {
                    Core.LadybugTools.Modify.SetUserData(result, Core.LadybugTools.UserDataKeys.ThermalConductivity, material_Base.ThermalConductivity);
                }
            }

            return result;
        }
    }
}
