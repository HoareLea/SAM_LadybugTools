// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using HoneybeeSchema.Energy;
using SAM.Core;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<ApertureConstruction> ToSAM_ApertureConstructions(this ModelEnergyProperties modelEnergyProperties, MaterialLibrary materialLibrary = null)
        {
            if(modelEnergyProperties == null)
            {
                return null;
            }

            List<ApertureConstruction> result = new List<ApertureConstruction>();

            List<IConstruction> constructions_Honeybee = modelEnergyProperties.ConstructionList?.ToList();

            List<HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet>> constructionSets = modelEnergyProperties.ConstructionSets;
            if (constructionSets != null)
            {
                foreach (HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet> anyOf in constructionSets)
                {
                    List<ApertureConstruction> apertureConstructions_Temp = anyOf.Obj is ConstructionSetAbridged ? Query.ApertureConstructions((ConstructionSetAbridged)anyOf.Obj, constructions_Honeybee, materialLibrary) : Query.ApertureConstructions(anyOf.Obj as ConstructionSet, materialLibrary);
                    if (apertureConstructions_Temp != null && apertureConstructions_Temp.Count != 0)
                    {
                        result.AddRange(apertureConstructions_Temp);
                    }
                }
            }

            if (constructions_Honeybee != null)
            {
                foreach (IConstruction construction_Honeybee in constructions_Honeybee)
                {
                    ApertureConstruction apertureConstruction = construction_Honeybee?.ToSAM_ApertureConstruction(materialLibrary);
                    if (apertureConstruction != null && result.Find(x => x.Name == apertureConstruction.Name) == null)
                    {
                        result.Add(apertureConstruction);
                    }
                }
            }

            GlobalConstructionSet globalConstructionSet = modelEnergyProperties.GlobalConstructionSet;
            if (globalConstructionSet != null)
            {
                List<AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged>> constructionAbridges_Honeybee = modelEnergyProperties.GlobalConstructionSet.Constructions;
                if (constructionAbridges_Honeybee != null)
                {
                    materialLibrary.AddMaterials(globalConstructionSet.Materials?.ConvertAll(x => x.Obj as HoneybeeSchema.Energy.IMaterial));

                    foreach (AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, ShadeConstruction, AirBoundaryConstructionAbridged> @object in constructionAbridges_Honeybee)
                    {
                        WindowConstructionAbridged construction_Honeybee = @object.Obj as WindowConstructionAbridged;
                        if (construction_Honeybee == null)
                        {
                            continue;
                        }

                        ApertureConstruction apertureConstruction = construction_Honeybee?.ToSAM_ApertureConstruction(materialLibrary);
                        if (apertureConstruction != null)
                        {
                            PanelType? panelType = Query.PanelType(globalConstructionSet, apertureConstruction.Name);
                            if (panelType != null && panelType.HasValue && panelType.Value != PanelType.Undefined)
                            {
                                apertureConstruction.SetValue(ApertureConstructionParameter.DefaultPanelType, panelType);
                            }

                            result.Add(apertureConstruction);
                        }
                    }
                }
            }

            AddFrameMaterials(result, modelEnergyProperties, materialLibrary);

            return result;
        }

        /// <summary>
        /// Restores frame materials that went missing from the Honeybee material list. Honeybee
        /// drops materials not referenced by a construction, so the base SAM frame material can be
        /// lost in transit while the derived EnergyWindowFrame ("{layer name}_Frame", referenced by
        /// WindowConstructionAbridged.Frame) survives.
        /// </summary>
        private static void AddFrameMaterials(List<ApertureConstruction> apertureConstructions, ModelEnergyProperties modelEnergyProperties, MaterialLibrary materialLibrary)
        {
            if (apertureConstructions == null || modelEnergyProperties == null || materialLibrary == null)
            {
                return;
            }

            List<HoneybeeSchema.Energy.IMaterial> materials_Honeybee = modelEnergyProperties.MaterialList?.ToList();
            if (materials_Honeybee == null)
            {
                return;
            }

            foreach (ApertureConstruction apertureConstruction in apertureConstructions)
            {
                List<ConstructionLayer> frameConstructionLayers = apertureConstruction?.FrameConstructionLayers;
                if (frameConstructionLayers == null)
                {
                    continue;
                }

                foreach (ConstructionLayer constructionLayer in frameConstructionLayers)
                {
                    if (constructionLayer == null || string.IsNullOrWhiteSpace(constructionLayer.Name) || materialLibrary.GetMaterial(constructionLayer.Name) != null)
                    {
                        continue;
                    }

                    EnergyWindowFrame energyWindowFrame = materials_Honeybee.OfType<EnergyWindowFrame>().FirstOrDefault(x => x?.Identifier == constructionLayer.Name || x?.Identifier == string.Format("{0}_Frame", constructionLayer.Name));
                    if (energyWindowFrame == null)
                    {
                        continue;
                    }

                    Core.OpaqueMaterial opaqueMaterial = energyWindowFrame.ToSAM();
                    if (opaqueMaterial == null)
                    {
                        continue;
                    }

                    if (opaqueMaterial.Name != constructionLayer.Name)
                    {
                        opaqueMaterial = new Core.OpaqueMaterial(constructionLayer.Name, opaqueMaterial.Guid, opaqueMaterial, opaqueMaterial.DisplayName, opaqueMaterial.Description);
                    }

                    materialLibrary.Add(opaqueMaterial);
                }
            }
        }
    }
}