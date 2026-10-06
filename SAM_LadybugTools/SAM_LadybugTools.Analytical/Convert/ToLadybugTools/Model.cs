// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using SAM.Architectural;
using SAM.Core;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Model ToLadybugTools(this AnalyticalModel analyticalModel, double silverSpacing = Tolerance.MacroDistance, double tolerance = Tolerance.Distance)
        {
            if (analyticalModel == null)
                return null;

            AnalyticalModel analyticalModel_Temp = new AnalyticalModel(analyticalModel);
            
            analyticalModel_Temp.OffsetAperturesOnEdge(0.1, tolerance);
            analyticalModel_Temp.ReplaceTransparentPanels(0.1);

            string uniqueName = Core.LadybugTools.Query.UniqueName(analyticalModel_Temp);

            AdjacencyCluster adjacencyCluster = analyticalModel_Temp.AdjacencyCluster;

            List<Room> rooms = null;
            List<AnyOf<IdealAirSystemAbridged, VAV, PVAV, PSZ, PTAC, ForcedAirFurnace, FCUwithDOASAbridged, WSHPwithDOASAbridged, VRFwithDOASAbridged, RadiantwithDOASAbridged, FCU, WSHP, VRF, Baseboard, EvaporativeCooler, Residential, WindowAC, GasUnitHeater, Radiant, DetailedHVAC>> hvacs = null;

            List<Space> spaces = adjacencyCluster?.GetSpaces();
            if (spaces != null)
            {
                hvacs = new List<AnyOf<IdealAirSystemAbridged, VAV, PVAV, PSZ, PTAC, ForcedAirFurnace, FCUwithDOASAbridged, WSHPwithDOASAbridged, VRFwithDOASAbridged, RadiantwithDOASAbridged, FCU, WSHP, VRF, Baseboard, EvaporativeCooler, Residential, WindowAC, GasUnitHeater, Radiant, DetailedHVAC>>();
                rooms = new List<Room>();

                Dictionary<double, List<IPanel>> dictionary_elevations = Analytical.Query.MinElevationDictionary(adjacencyCluster.GetPanels(), true);
                List<Level> levels = dictionary_elevations?.Keys.ToList().ConvertAll(x => Architectural.Create.Level(x));

                for (int i = 0; i < spaces.Count; i++)
                {
                    Space space = spaces[i];
                    if (space == null)
                        continue;

                    Room room = space.ToLadybugTools(analyticalModel_Temp, silverSpacing, tolerance);
                    if (room == null)
                        continue;

                    if (levels != null && levels.Count > 0)
                    {
                        double elevation_Min = space.MinElevation(adjacencyCluster);
                        if (!double.IsNaN(elevation_Min))
                        {
                            double difference_Min = double.MaxValue;
                            Level level_Min = null;
                            foreach(Level level in levels)
                            {
                                double difference = System.Math.Abs(elevation_Min - level.Elevation);
                                if(difference < difference_Min)
                                {
                                    difference_Min = difference;
                                    level_Min = level;
                                }
                            }

                            room.Story = level_Min.Name;
                        }
                    }

                    InternalCondition internalCondition = space.InternalCondition;
                    if(internalCondition != null)
                    {
                        //Michal Idea of filtering Uncondition Spaces
                        string name_InternalCondition = internalCondition.Name;

                        if(name_InternalCondition == null || (name_InternalCondition != null && !name_InternalCondition.ToLower().Contains("unconditioned") && !name_InternalCondition.ToLower().Contains("external")))
                        {

                            IdealAirSystemAbridged idealAirSystemAbridged = new IdealAirSystemAbridged(string.Format("{0}__{1}", i.ToString(), "IdealAir"), string.Format("Ideal Air System Abridged {0}", space.Name));
                            hvacs.Add(idealAirSystemAbridged);

                            if (room.Properties == null)
                                room.Properties = new RoomPropertiesAbridged();

                            if (room.Properties.Energy == null)
                                room.Properties.Energy = new RoomEnergyPropertiesAbridged();

                            room.Properties.Energy.Hvac = idealAirSystemAbridged.Identifier;
                        }

                    }

                    rooms.Add(room);
                }

            }

            // OffsetAperturesOnEdge may rebuild aperture objects with new Guids; restore the
            // original SAM aperture Guids into user_data using deterministic geometry pairing
            if (rooms != null)
            {
                List<Panel> panels_Original = analyticalModel.AdjacencyCluster?.GetPanels();
                if (panels_Original != null)
                {
                    foreach (Room room in rooms)
                    {
                        List<Face> faces_Room = room?.Faces;
                        if (faces_Room == null)
                            continue;

                        foreach (Face face in faces_Room)
                        {
                            string identifier = face?.Identifier;
                            if (string.IsNullOrWhiteSpace(identifier))
                                continue;

                            int index = identifier.LastIndexOf("__", System.StringComparison.Ordinal);
                            string guidText = index == -1 ? identifier : identifier.Substring(index + 2);
                            if (!System.Guid.TryParse(guidText, out System.Guid guid_Panel))
                                continue;

                            Panel panel_Original = panels_Original.Find(x => x.Guid == guid_Panel);
                            List<Aperture> apertures_Original = panel_Original?.Apertures;
                            if (apertures_Original == null || apertures_Original.Count == 0)
                                continue;

                            List<HoneybeeSchema.IDdBaseModel> openings = new List<HoneybeeSchema.IDdBaseModel>();
                            face.Apertures?.ForEach(x => { if (x != null) openings.Add(x); });
                            face.Doors?.ForEach(x => { if (x != null) openings.Add(x); });

                            if (openings.Count != apertures_Original.Count)
                                continue;

                            apertures_Original = apertures_Original.OrderBy(x => Query.ApertureSortKey(x)).ToList();
                            openings = openings.OrderBy(x => Query.ApertureSortKey(x)).ToList();

                            for (int i = 0; i < openings.Count; i++)
                            {
                                Core.LadybugTools.Modify.SetUserData(openings[i], Core.LadybugTools.UserDataKeys.Guid, apertures_Original[i].Guid.ToString());
                            }
                        }
                    }
                }
            }

            List<Shade> shades = null;
            List<Face> faces_Orphaned = null;

            List<Panel> panels_Shading = adjacencyCluster.GetShadingPanels();
            if(panels_Shading != null)
            {
                foreach (Panel panel_Shading in panels_Shading)
                {
                    if (panels_Shading == null)
                    {
                        continue;
                    }

                    if (panel_Shading.PanelType == PanelType.Shade)
                    {
                        List<Shade> shades_Temp = panel_Shading.ToLadybugTools_Shades();
                        if (shades_Temp == null)
                        {
                            continue;
                        }

                        if (shades == null)
                        {
                            shades = new List<Shade>();
                        }

                        shades.AddRange(shades_Temp);
                    }
                    else
                    {
                        Face face_Orphaned = panel_Shading.ToLadybugTools_Face();
                        if (face_Orphaned == null)
                        {
                            continue;
                        }

                        if (faces_Orphaned == null)
                        {
                            faces_Orphaned = new List<Face>();
                        }

                        faces_Orphaned.Add(face_Orphaned);
                    }
                }
            }

            MaterialLibrary materialLibrary = analyticalModel_Temp?.MaterialLibrary;

            List<Construction> constructions_AdjacencyCluster = adjacencyCluster.GetConstructions();
            List<ApertureConstruction> apertureConstructions_AdjacencyCluster = adjacencyCluster.GetApertureConstructions();

            ConstructionSetAbridged constructionSetAbridged = Core.LadybugTools.Query.DefaultConstructionSetAbridged();
            List<HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet>> constructionSets = new List<HoneybeeSchema.AnyOf<ConstructionSetAbridged, ConstructionSet>>();// { constructionSetAbridged  };

            List<AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, WindowConstructionShadeAbridged, AirBoundaryConstructionAbridged, OpaqueConstruction, WindowConstruction, WindowConstructionShade, WindowConstructionDynamicAbridged, WindowConstructionDynamic, AirBoundaryConstruction, ShadeConstruction>> constructions = new List<AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, WindowConstructionShadeAbridged, AirBoundaryConstructionAbridged, OpaqueConstruction, WindowConstruction, WindowConstructionShade, WindowConstructionDynamicAbridged, WindowConstructionDynamic, AirBoundaryConstruction, ShadeConstruction>>();

            Dictionary<string, HoneybeeSchema.Energy.IMaterial> dictionary_Materials = new Dictionary<string, HoneybeeSchema.Energy.IMaterial>();
            if(constructions_AdjacencyCluster != null)
            {
                foreach(Construction construction in constructions_AdjacencyCluster)
                {
                    List<ConstructionLayer> constructionLayers = construction.ConstructionLayers;
                    if (constructionLayers == null)
                        continue;

                    constructions.Add(construction.ToLadybugTools());
                    constructions.Add(construction.ToLadybugTools(false));

                    foreach (ConstructionLayer constructionLayer in constructionLayers)
                    {
                        IMaterial material = constructionLayer.Material(materialLibrary);
                        if (material == null)
                            continue;

                        if (dictionary_Materials.ContainsKey(material.Name))
                            continue;

                        if (material is GasMaterial)
                        {
                            List<Panel> panels = Analytical.Query.Panels(adjacencyCluster, construction);
                            List<double> tilts = panels.ConvertAll(x => Analytical.Query.Tilt(x).Round(Tolerance.MacroDistance));
                            double tilt = tilts.Distinct().ToList().Average();

                            tilt = Units.Convert.ToRadians(tilt);

                            dictionary_Materials[material.Name] = ((GasMaterial)material).ToLadybugTools(tilt, constructionLayer.Thickness);
                        }
                        else if(material is OpaqueMaterial)
                        {
                            EnergyMaterial energyMaterial = ((OpaqueMaterial)material).ToLadybugTools();
                            dictionary_Materials[material.Name] = energyMaterial;
                            if (!double.IsNaN(constructionLayer.Thickness))
                                energyMaterial.Thickness = constructionLayer.Thickness;
                        }
                    }
                }
            }

            if(apertureConstructions_AdjacencyCluster != null)
            {
                foreach (ApertureConstruction apertureConstruction in apertureConstructions_AdjacencyCluster)
                {
                    List<ConstructionLayer> constructionLayers = null;

                    constructionLayers = apertureConstruction.PaneConstructionLayers;
                    if (constructionLayers != null)
                    {
                        MaterialType materialType = Analytical.Query.MaterialType(constructionLayers, materialLibrary);
                        if(materialType != MaterialType.Undefined && materialType != MaterialType.Gas)
                        {
                            if(materialType == MaterialType.Opaque)
                            {
                                constructions.Add(apertureConstruction.ToLadybugTools());
                                constructions.Add(apertureConstruction.ToLadybugTools(false));
                            }
                            else
                            {
                                constructions.Add(apertureConstruction.ToLadybugTools_WindowConstructionAbridged());
                                constructions.Add(apertureConstruction.ToLadybugTools_WindowConstructionAbridged(false));
                            }

                            foreach (ConstructionLayer constructionLayer in constructionLayers)
                            {
                                IMaterial material = constructionLayer.Material(materialLibrary);
                                if (material == null)
                                    continue;

                                string name = material.Name;

                                if (dictionary_Materials.ContainsKey(name))
                                    continue;

                                if (material is TransparentMaterial)
                                    dictionary_Materials[name] = ((TransparentMaterial)material).ToLadybugTools();
                                else if (material is GasMaterial)
                                    dictionary_Materials[name] = ((GasMaterial)material).ToLadybugTools_EnergyWindowMaterialGas();
                                else
                                    dictionary_Materials[name] = ((OpaqueMaterial)material).ToLadybugTools();
                            }
                        }
                    }

                    List<ConstructionLayer> frameConstructionLayers = apertureConstruction.FrameConstructionLayers;
                    if (frameConstructionLayers != null && frameConstructionLayers.Count != 0)
                    {
                        foreach (ConstructionLayer constructionLayer in frameConstructionLayers)
                        {
                            IMaterial material = constructionLayer.Material(materialLibrary);
                            if (material == null)
                                continue;

                            if (dictionary_Materials.ContainsKey(material.Name))
                                continue;

                            if (material is OpaqueMaterial)
                                dictionary_Materials[material.Name] = ((OpaqueMaterial)material).ToLadybugTools();
                            else if (material is GasMaterial)
                                dictionary_Materials[material.Name] = ((GasMaterial)material).ToLadybugTools_EnergyWindowMaterialGas();
                            else if (material is TransparentMaterial)
                                dictionary_Materials[material.Name] = ((TransparentMaterial)material).ToLadybugTools();
                        }

                        EnergyWindowFrame energyWindowFrame = apertureConstruction.ToLadybugTools_EnergyWindowFrame(materialLibrary);
                        if (energyWindowFrame != null && !dictionary_Materials.ContainsKey(energyWindowFrame.Identifier))
                        {
                            dictionary_Materials[energyWindowFrame.Identifier] = energyWindowFrame;
                        }
                    }
                }
            }

            
            ProfileLibrary profileLibrary = analyticalModel.ProfileLibrary;

            Dictionary<System.Guid, ProgramType> dictionary_InternalConditions = new Dictionary<System.Guid, ProgramType>();
            if (spaces != null)
            {
                foreach (Space space in spaces)
                {
                    InternalCondition internalCondition = space?.InternalCondition;
                    if (internalCondition == null)
                        continue;

                    if (dictionary_InternalConditions.ContainsKey(internalCondition.Guid))
                        continue;

                    ProgramType programType = space.ToLadybugTools(adjacencyCluster, profileLibrary);
                    if (programType != null)
                        dictionary_InternalConditions[internalCondition.Guid] = programType;
                }
            }

            List<AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyMaterialVegetation, EnergyWindowMaterialGlazing, EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialGas, EnergyWindowMaterialGasMixture, EnergyWindowMaterialGasCustom, EnergyWindowFrame, EnergyWindowMaterialBlind, EnergyWindowMaterialShade>> materials = new List<AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyMaterialVegetation, EnergyWindowMaterialGlazing, EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialGas, EnergyWindowMaterialGasMixture, EnergyWindowMaterialGasCustom, EnergyWindowFrame, EnergyWindowMaterialBlind, EnergyWindowMaterialShade>>();
            HoneybeeSchema.Helper.EnergyLibrary.DefaultMaterials?.ToList().ForEach(x => materials.Add(x as dynamic));
            dictionary_Materials.Values.ToList().ForEach(x => materials.Add(x as dynamic));

            List<AnyOf<ScheduleRulesetAbridged, ScheduleFixedIntervalAbridged, ScheduleRuleset, ScheduleFixedInterval>> schedules = new List<AnyOf<ScheduleRulesetAbridged, ScheduleFixedIntervalAbridged, ScheduleRuleset, ScheduleFixedInterval>>();
            HoneybeeSchema.Helper.EnergyLibrary.DefaultScheduleRuleset?.ToList().ForEach(x => schedules.Add(x));

            List<HoneybeeSchema.AnyOf<ProgramTypeAbridged, ProgramType>> programTypes = new List<HoneybeeSchema.AnyOf<ProgramTypeAbridged, ProgramType>>();
            HoneybeeSchema.Helper.EnergyLibrary.DefaultProgramTypes?.ToList().ForEach(x => programTypes.Add(x));
            dictionary_InternalConditions.Values.ToList().ForEach(x => programTypes.Add(x));

            List<ScheduleTypeLimit> scheduleTypeLimits = new List<ScheduleTypeLimit>();
            HoneybeeSchema.Helper.EnergyLibrary.DefaultScheduleTypeLimit?.ToList().ForEach(x => scheduleTypeLimits.Add(x));

            constructionSets.RemoveAll(x => x == null);
            constructions.RemoveAll(x => x == null);
            materials.RemoveAll(x => x == null);

            ModelEnergyProperties modelEnergyProperties = new ModelEnergyProperties(constructionSets, constructions, materials, hvacs, null, programTypes, schedules, scheduleTypeLimits);
            
            ModelProperties modelProperties = new ModelProperties(modelEnergyProperties);

            Model model = new Model(uniqueName, modelProperties, adjacencyCluster.Name, null, rooms, faces_Orphaned, shades);
            model.AngleTolerance = Units.Convert.ToDegrees(Tolerance.Angle);// 2;
            model.Tolerance = Tolerance.MacroDistance;

            Core.LadybugTools.Modify.SetUserData(model, Core.LadybugTools.UserDataKeys.Guid, analyticalModel.Guid.ToString());
            Core.LadybugTools.Modify.SetUserData(model, Core.LadybugTools.UserDataKeys.Name, analyticalModel.Name);
            Core.LadybugTools.Modify.SetUserData(model, Core.LadybugTools.UserDataKeys.Description, analyticalModel.Description);

            Core.Location location = analyticalModel.Location;
            if (location != null)
            {
                Core.LadybugTools.Modify.SetUserData(model, Core.LadybugTools.UserDataKeys.LocationName, location.Name);
                Core.LadybugTools.Modify.SetUserData(model, Core.LadybugTools.UserDataKeys.LocationLatitude, location.Latitude);
                Core.LadybugTools.Modify.SetUserData(model, Core.LadybugTools.UserDataKeys.LocationLongitude, location.Longitude);
                Core.LadybugTools.Modify.SetUserData(model, Core.LadybugTools.UserDataKeys.LocationElevation, location.Elevation);
            }

            if (profileLibrary != null && !string.IsNullOrWhiteSpace(profileLibrary.Name))
            {
                Core.LadybugTools.Modify.SetUserData(model, Core.LadybugTools.UserDataKeys.ProfileLibraryName, profileLibrary.Name);
            }

            return model;
        }
    }
}