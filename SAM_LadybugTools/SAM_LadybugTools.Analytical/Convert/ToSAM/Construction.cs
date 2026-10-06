// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using HoneybeeSchema.Energy;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Construction ToSAM(this OpaqueConstructionAbridged opaqueConstructionAbridged, Core.MaterialLibrary materialLibrary = null)
        {
            if(opaqueConstructionAbridged == null)
            {
                return null;
            }

            // SAM round-trip metadata restores the original name, layer order and thicknesses
            if (!Core.LadybugTools.Query.TryGetUserData(opaqueConstructionAbridged, Core.LadybugTools.UserDataKeys.Name, out string name) || string.IsNullOrWhiteSpace(name))
            {
                name = opaqueConstructionAbridged.Identifier;
            }

            if (!Query.TryGetConstructionLayers(opaqueConstructionAbridged, Core.LadybugTools.UserDataKeys.ConstructionLayers, out List<ConstructionLayer> constructionLayers))
            {
                constructionLayers = Query.ConstructionLayers(materialLibrary, opaqueConstructionAbridged.Materials);
            }

            if (!Query.TryGetSAMGuid(opaqueConstructionAbridged, out System.Guid guid))
            {
                guid = System.Guid.NewGuid();
            }

            Construction result = new Construction(guid, name, constructionLayers);

            if (Core.LadybugTools.Query.TryGetUserData(opaqueConstructionAbridged, Core.LadybugTools.UserDataKeys.DefaultPanelType, out string panelType) && !string.IsNullOrWhiteSpace(panelType))
            {
                result.SetValue(ConstructionParameter.DefaultPanelType, panelType);
            }

            return result;
        }

        public static Construction ToSAM_Construction(this IConstruction construction, Core.MaterialLibrary materialLibrary = null)
        {
            if (construction == null)
            {
                return null;
            }

            if(construction is OpaqueConstructionAbridged)
            {
                return ((OpaqueConstructionAbridged)construction).ToSAM(materialLibrary);
            }

            return null;
        }
    
        public static Construction ToSAM_Construction(this AnyOf<AirBoundaryConstruction, OpaqueConstruction> construction, Core.MaterialLibrary materialLibrary = null)
        {
            if(construction == null)
            {
                return null;
            }

            string name = null;
            List<string> materialNames = null;
            
            if (construction.Obj is OpaqueConstructionAbridged)
            {
                name = ((OpaqueConstructionAbridged)construction.Obj).Identifier;
                materialNames = ((OpaqueConstructionAbridged)construction.Obj).Materials;
            }
            else if (construction.Obj is WindowConstructionAbridged)
            {
                name = ((WindowConstructionAbridged)construction.Obj).Identifier;
                materialNames = ((WindowConstructionAbridged)construction.Obj).Materials;
            }
            else if (construction.Obj is ShadeConstruction)
            {
                name = ((ShadeConstruction)construction.Obj).Identifier;
            }
            else if (construction.Obj is AirBoundaryConstructionAbridged)
            {
                name = ((AirBoundaryConstructionAbridged)construction.Obj).Identifier;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            List<ConstructionLayer> constructionLayers = null;
            if (materialNames != null)
            {
                constructionLayers = new List<ConstructionLayer>();
                foreach(string materialName in materialNames)
                {
                    double thickness = 0;

                    AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGlazing, EnergyWindowMaterialGas> material_Honeybee = SAM.Core.LadybugTools.Query.DefaultMaterial(materialName);
                    if(material_Honeybee != null)
                    {
                        if(material_Honeybee.Obj is EnergyMaterial)
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

            return new Construction(name, constructionLayers);
        }

        public static Construction ToSAM_Construction(this AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> construction, Core.MaterialLibrary materialLibrary = null)
        {
            if (construction == null)
            {
                return null;
            }

            string name = null;
            List<string> materialNames = null;

            if (construction.Obj is OpaqueConstructionAbridged)
            {
                name = ((OpaqueConstructionAbridged)construction.Obj).Identifier;
                materialNames = ((OpaqueConstructionAbridged)construction.Obj).Materials;
            }
            else if (construction.Obj is WindowConstructionAbridged)
            {
                name = ((WindowConstructionAbridged)construction.Obj).Identifier;
                materialNames = ((WindowConstructionAbridged)construction.Obj).Materials;
            }
            else if (construction.Obj is ShadeConstruction)
            {
                name = ((ShadeConstruction)construction.Obj).Identifier;
            }
            else if (construction.Obj is AirBoundaryConstructionAbridged)
            {
                name = ((AirBoundaryConstructionAbridged)construction.Obj).Identifier;
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

            return new Construction(name, constructionLayers);
        }
    }
}