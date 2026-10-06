// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using HoneybeeSchema.Energy;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ApertureConstruction ToSAM(this WindowConstructionAbridged windowConstructionAbridged, Core.MaterialLibrary materialLibrary = null)
        {
            if(windowConstructionAbridged == null)
            {
                return null;
            }

            // SAM round-trip metadata restores the original name, pane layers and frame layers
            if (!Core.LadybugTools.Query.TryGetUserData(windowConstructionAbridged, Core.LadybugTools.UserDataKeys.Name, out string name) || string.IsNullOrWhiteSpace(name))
            {
                name = windowConstructionAbridged.Identifier;
            }

            if (!Query.TryGetConstructionLayers(windowConstructionAbridged, Core.LadybugTools.UserDataKeys.PaneConstructionLayers, out List<ConstructionLayer> constructionLayers))
            {
                constructionLayers = Query.ConstructionLayers(materialLibrary, windowConstructionAbridged.Materials);
            }

            List<ConstructionLayer> frameConstructionLayers = null;
            if (!Query.TryGetConstructionLayers(windowConstructionAbridged, Core.LadybugTools.UserDataKeys.FrameConstructionLayers, out frameConstructionLayers))
            {
                // Honeybee-native frame reference: keep it as a single frame layer instead of dropping it
                if (!string.IsNullOrWhiteSpace(windowConstructionAbridged.Frame))
                {
                    frameConstructionLayers = Query.ConstructionLayers(materialLibrary, new List<string> { windowConstructionAbridged.Frame });
                    if (frameConstructionLayers != null && frameConstructionLayers.Count == 0)
                    {
                        frameConstructionLayers = null;
                    }
                }
            }

            if (!Query.TryGetSAMGuid(windowConstructionAbridged, out System.Guid guid))
            {
                guid = System.Guid.NewGuid();
            }

            ApertureConstruction result = new ApertureConstruction(guid, name, ApertureType.Window, constructionLayers, frameConstructionLayers);

            if (Core.LadybugTools.Query.TryGetUserData(windowConstructionAbridged, Core.LadybugTools.UserDataKeys.DefaultPanelType, out string panelType) && !string.IsNullOrWhiteSpace(panelType))
            {
                result.SetValue(ApertureConstructionParameter.DefaultPanelType, panelType);
            }

            return result;
        }

        public static ApertureConstruction ToSAM_ApertureConstruction(this IConstruction construction, Core.MaterialLibrary materialLibrary = null)
        {
            if (construction == null)
            {
                return null;
            }

            if(construction is WindowConstructionAbridged)
            {
                return ((WindowConstructionAbridged)construction).ToSAM(materialLibrary);
            }

            if(construction is OpaqueConstruction)
            {
                return ((OpaqueConstruction)construction).ToSAM_ApertureConstruction();
            }

            if (construction is OpaqueConstructionAbridged)
            {
                return ((OpaqueConstructionAbridged)construction).ToSAM_ApertureConstruction(materialLibrary);
            }

            return null;
        }

        public static ApertureConstruction ToSAM_ApertureConstruction(this OpaqueConstruction opaqueConstruction)
        {
            if (opaqueConstruction == null)
            {
                return null;
            }

            List<ConstructionLayer> constructionLayers = Query.ConstructionLayers(opaqueConstruction.Materials.ConvertAll(x => (x.Obj as IMaterial).ToSAM()));

            ApertureConstruction result = new ApertureConstruction(System.Guid.NewGuid(), opaqueConstruction.Identifier, ApertureType.Door, constructionLayers);
            return result;
        }

        public static ApertureConstruction ToSAM_ApertureConstruction(this OpaqueConstructionAbridged opaqueConstructionAbridged, Core.MaterialLibrary materialLibrary = null)
        {
            if (opaqueConstructionAbridged == null)
            {
                return null;
            }

            if (!Core.LadybugTools.Query.TryGetUserData(opaqueConstructionAbridged, Core.LadybugTools.UserDataKeys.Name, out string name) || string.IsNullOrWhiteSpace(name))
            {
                name = opaqueConstructionAbridged.Identifier;
            }

            if (!Query.TryGetConstructionLayers(opaqueConstructionAbridged, Core.LadybugTools.UserDataKeys.PaneConstructionLayers, out List<ConstructionLayer> constructionLayers))
            {
                constructionLayers = Query.ConstructionLayers(materialLibrary, opaqueConstructionAbridged.Materials);
            }

            Query.TryGetConstructionLayers(opaqueConstructionAbridged, Core.LadybugTools.UserDataKeys.FrameConstructionLayers, out List<ConstructionLayer> frameConstructionLayers);

            if (!Query.TryGetSAMGuid(opaqueConstructionAbridged, out System.Guid guid))
            {
                guid = System.Guid.NewGuid();
            }

            ApertureConstruction result = new ApertureConstruction(guid, name, ApertureType.Door, constructionLayers, frameConstructionLayers);

            if (Core.LadybugTools.Query.TryGetUserData(opaqueConstructionAbridged, Core.LadybugTools.UserDataKeys.DefaultPanelType, out string panelType) && !string.IsNullOrWhiteSpace(panelType))
            {
                result.SetValue(ApertureConstructionParameter.DefaultPanelType, panelType);
            }

            return result;
        }

        public static ApertureConstruction ToSAM_ApertureConstruction(this AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> construction, Core.MaterialLibrary materialLibrary = null)
        {
            if (construction == null)
            {
                return null;
            }

            string name = null;
            List<string> materialNames = null;
            ApertureType apertureType = ApertureType.Undefined;


            if (construction.Obj is OpaqueConstructionAbridged)
            {
                name = ((OpaqueConstructionAbridged)construction.Obj).Identifier;
                materialNames = ((OpaqueConstructionAbridged)construction.Obj).Materials;
                apertureType = ApertureType.Door;
            }
            else if (construction.Obj is WindowConstructionAbridged)
            {
                name = ((WindowConstructionAbridged)construction.Obj).Identifier;
                materialNames = ((WindowConstructionAbridged)construction.Obj).Materials;
                apertureType = ApertureType.Window;
            }
            else if (construction.Obj is ShadeConstruction)
            {
                name = ((ShadeConstruction)construction.Obj).Identifier;
                apertureType = ApertureType.Door;
            }
            else if (construction.Obj is AirBoundaryConstructionAbridged)
            {
                name = ((AirBoundaryConstructionAbridged)construction.Obj).Identifier;
                apertureType = ApertureType.Window;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            List<ConstructionLayer> constructionLayers = null;
            if (materialNames != null)
            {
                constructionLayers = new List<ConstructionLayer>();
                foreach (string materialName in materialNames)
                {
                    double thickness = 0;

                    AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas> material_Honeybee = SAM.Core.LadybugTools.Query.DefaultMaterial(materialName);
                    if (material_Honeybee != null)
                    {
                        if (material_Honeybee.Obj is EnergyMaterial)
                        {
                            thickness = ((EnergyMaterial)material_Honeybee.Obj).Thickness;
                        }
                        else if (material_Honeybee.Obj is EnergyMaterialNoMass)
                        {

                        }
                        else if (material_Honeybee.Obj is EnergyWindowMaterialGlazing)
                        {
                            thickness = ((EnergyWindowMaterialGlazing)material_Honeybee.Obj).Thickness;
                        }
                        else if (material_Honeybee.Obj is EnergyWindowMaterialGas)
                        {
                            thickness = ((EnergyWindowMaterialGas)material_Honeybee.Obj).Thickness;
                        }
                    }

                    constructionLayers.Add(new ConstructionLayer(materialName, thickness));
                }
            }

            return new ApertureConstruction(System.Guid.NewGuid(), name, apertureType, constructionLayers);
        }
    }
}