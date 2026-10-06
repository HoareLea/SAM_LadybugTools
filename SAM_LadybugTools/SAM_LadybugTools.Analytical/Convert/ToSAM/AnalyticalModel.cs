// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using HoneybeeSchema;
using SAM.Core;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static AnalyticalModel ToSAM(this Model model)
        {
            if (model == null)
            {
                return null;
            }

            MaterialLibrary materialLibrary = null;
            ProfileLibrary profileLibrary = null;
            List<Construction> constructions = null;
            List<ApertureConstruction> apertureConstructions = null;
            List<InternalCondition> internalConditions = null;

            ModelEnergyProperties modelEnergyProperties = model.Properties?.Energy;
            
            if(modelEnergyProperties != null)
            {
                materialLibrary = modelEnergyProperties.ToSAM_MaterialLibrary();
                constructions = modelEnergyProperties.ToSAM_Constructions(materialLibrary);
                apertureConstructions = modelEnergyProperties.ToSAM_ApertureConstructions(materialLibrary);
                internalConditions = modelEnergyProperties. ToSAM_InternalConditions();
                profileLibrary = modelEnergyProperties.ToSAM_ProfileLibrary();
            }

            if (materialLibrary == null)
            {
                materialLibrary = new MaterialLibrary(string.Empty);
            }

            if (constructions == null)
            {
                constructions = new List<Construction>();
            }

            if (apertureConstructions == null)
            {
                apertureConstructions = new List<ApertureConstruction>();
            }

            List<Tuple<Panel, Geometry.Spatial.BoundingBox3D>> tuples = new List<Tuple<Panel, Geometry.Spatial.BoundingBox3D>>();

            AdjacencyCluster adjacencyCluster = new AdjacencyCluster();
            List<Room> rooms = model.Rooms;
            if (rooms != null)
            {
                foreach (Room room in rooms)
                {
                    room.IndoorShades?.ConvertAll(x => x.ToSAM(constructions))?.ForEach(x => adjacencyCluster.AddObject(x));
                    room.OutdoorShades?.ConvertAll(x => x.ToSAM(constructions))?.ForEach(x => adjacencyCluster.AddObject(x));

                    List<Face> faces = room.Faces;
                    if (faces == null)
                    {
                        continue;
                    }

                    List<Panel> panels = new List<Panel>();
                    foreach (Face face in faces)
                    {
                        Panel panel = face.ToSAM(constructions, apertureConstructions);
                        if (panel == null)
                        {
                            continue;
                        }

                        Geometry.Spatial.Point3D point3D = panel.GetFace3D().GetInternalPoint3D();
                        if (point3D == null)
                        {
                            continue;
                        }

                        Panel panel_Existing = tuples.FindAll(x => x.Item2.Inside(point3D, true, Tolerance.MacroDistance))?.Find(x => x.Item1.GetFace3D().On(point3D, Tolerance.MacroDistance))?.Item1;
                        if (panel_Existing != null)
                        {
                            panel = panel_Existing;
                        }
                        else
                        {
                            tuples.Add(new Tuple<Panel, Geometry.Spatial.BoundingBox3D>(panel, panel.GetFace3D().GetBoundingBox()));

                            face.IndoorShades?.ConvertAll(x => x.ToSAM(constructions))?.ForEach(x => adjacencyCluster.AddObject(x));
                            face.OutdoorShades?.ConvertAll(x => x.ToSAM(constructions))?.ForEach(x => adjacencyCluster.AddObject(x));

                            Construction construction = panel.Construction;
                            if (construction != null)
                            {
                                if (constructions.Find(x => x.Name.Equals(construction.Name)) == null)
                                {
                                    constructions.Add(construction);
                                }

                                materialLibrary.AddDefaultMaterials(construction);
                            }

                            List<Aperture> apertures = panel.Apertures;
                            if (apertures != null)
                            {
                                foreach (Aperture aperture in apertures)
                                {
                                    ApertureConstruction apertureConstruction = aperture.ApertureConstruction;
                                    if (apertureConstruction != null)
                                    {
                                        if (apertureConstructions.Find(x => x.Name.Equals(apertureConstruction.Name)) == null)
                                        {
                                            apertureConstructions.Add(apertureConstruction);
                                        }

                                        materialLibrary.AddDefaultMaterials(apertureConstruction);
                                    }
                                }
                            }

                            List<HoneybeeSchema.Aperture> apertures_HoneybeeSchema = face.Apertures;
                            if(apertures_HoneybeeSchema != null)
                            {
                                foreach (HoneybeeSchema.Aperture aperture_HoneybeeSchema in apertures_HoneybeeSchema)
                                {
                                    aperture_HoneybeeSchema.IndoorShades?.ConvertAll(x => x.ToSAM(constructions))?.ForEach(x => adjacencyCluster.AddObject(x));
                                    aperture_HoneybeeSchema.OutdoorShades?.ConvertAll(x => x.ToSAM(constructions))?.ForEach(x => adjacencyCluster.AddObject(x));
                                }
                            }

                            List<HoneybeeSchema.Door> doors_HoneybeeSchema = face.Doors;
                            if (doors_HoneybeeSchema != null)
                            {
                                foreach (HoneybeeSchema.Door door_HoneybeeSchema in doors_HoneybeeSchema)
                                {
                                    door_HoneybeeSchema.IndoorShades?.ConvertAll(x => x.ToSAM(constructions))?.ForEach(x => adjacencyCluster.AddObject(x));
                                    door_HoneybeeSchema.OutdoorShades?.ConvertAll(x => x.ToSAM(constructions))?.ForEach(x => adjacencyCluster.AddObject(x));
                                }
                            }

                        }

                        panels.Add(panel);
                    }

                    Space space = room.ToSAM(internalConditions);
                    adjacencyCluster.AddObject(space);

                    if (panels != null)
                    {
                        foreach(Panel panel in panels)
                        {
                            adjacencyCluster.AddObject(panel);
                            adjacencyCluster.AddRelation(space, panel);
                        }
                    }
                }
            }

            List<Shade> shades = model.OrphanedShades;
            if(shades != null && shades.Count != 0)
            {
                foreach(Shade shade in shades)
                {
                    Panel panel = shade?.ToSAM(constructions);
                    if(panel != null)
                    {
                        Construction construction = panel.Construction;
                        if (construction != null)
                        {
                            if (constructions.Find(x => x.Name.Equals(construction.Name)) == null)
                            {
                                constructions.Add(construction);
                            }

                            materialLibrary.AddDefaultMaterials(construction);
                        }

                        List<Aperture> apertures = panel.Apertures;
                        if (apertures != null)
                        {
                            foreach (Aperture aperture in apertures)
                            {
                                ApertureConstruction apertureConstruction = aperture.ApertureConstruction;
                                if (apertureConstruction != null)
                                {
                                    if (apertureConstructions.Find(x => x.Name.Equals(apertureConstruction.Name)) == null)
                                    {
                                        apertureConstructions.Add(apertureConstruction);
                                    }

                                    materialLibrary.AddDefaultMaterials(apertureConstruction);
                                }
                            }
                        }

                        adjacencyCluster.AddObject(panel);
                    }
                }
            }

            // Remove materials that are not referenced by the constructions and aperture
            // constructions actually used by the converted model (for example unreferenced
            // Honeybee defaults from the model or global construction set)
            if (materialLibrary != null)
            {
                HashSet<string> materialNames = new HashSet<string>();

                List<Construction> constructions_AdjacencyCluster = adjacencyCluster?.GetConstructions();
                if (constructions_AdjacencyCluster != null)
                {
                    foreach (Construction construction in constructions_AdjacencyCluster)
                    {
                        construction?.ConstructionLayers?.ForEach(x => { if (x != null && !string.IsNullOrWhiteSpace(x.Name)) materialNames.Add(x.Name); });
                    }
                }

                List<ApertureConstruction> apertureConstructions_AdjacencyCluster = adjacencyCluster?.GetApertureConstructions();
                if (apertureConstructions_AdjacencyCluster != null)
                {
                    foreach (ApertureConstruction apertureConstruction in apertureConstructions_AdjacencyCluster)
                    {
                        apertureConstruction?.PaneConstructionLayers?.ForEach(x => { if (x != null && !string.IsNullOrWhiteSpace(x.Name)) materialNames.Add(x.Name); });
                        apertureConstruction?.FrameConstructionLayers?.ForEach(x => { if (x != null && !string.IsNullOrWhiteSpace(x.Name)) materialNames.Add(x.Name); });
                    }
                }

                List<IMaterial> materials = materialLibrary.GetMaterials();
                if (materials != null)
                {
                    foreach (IMaterial material in materials)
                    {
                        if (material == null || string.IsNullOrWhiteSpace(material.Name))
                        {
                            continue;
                        }

                        if (!materialNames.Contains(material.Name))
                        {
                            materialLibrary.Remove(material);
                        }
                    }
                }
            }

            // Remove profiles that are not referenced by any assigned InternalCondition
            // (for example Honeybee default program type schedules)
            if (profileLibrary != null)
            {
                HashSet<string> profileNames = new HashSet<string>();

                List<Space> spaces = adjacencyCluster?.GetSpaces();
                if (spaces != null)
                {
                    foreach (Space space in spaces)
                    {
                        InternalCondition internalCondition = space?.InternalCondition;
                        if (internalCondition == null)
                        {
                            continue;
                        }

                        List<ParameterSet> parameterSets = internalCondition.GetParameterSets();
                        if (parameterSets == null)
                        {
                            continue;
                        }

                        foreach (ParameterSet parameterSet in parameterSets)
                        {
                            if (parameterSet?.Names == null)
                            {
                                continue;
                            }

                            foreach (string parameterName in parameterSet.Names)
                            {
                                if (parameterName == null || !parameterName.EndsWith("Profile Name"))
                                {
                                    continue;
                                }

                                if (parameterSet.ToObject(parameterName) is string profileName && !string.IsNullOrWhiteSpace(profileName))
                                {
                                    profileNames.Add(profileName);
                                }
                            }
                        }
                    }
                }

                List<Profile> profiles = profileLibrary.GetProfiles();
                if (profiles != null)
                {
                    foreach (Profile profile in profiles)
                    {
                        if (profile == null || string.IsNullOrWhiteSpace(profile.Name))
                        {
                            continue;
                        }

                        if (!profileNames.Contains(profile.Name))
                        {
                            profileLibrary.Remove(profile);
                        }
                    }
                }
            }

            AnalyticalModel result = new AnalyticalModel(model.DisplayName, null, null, null, adjacencyCluster, materialLibrary, profileLibrary);

            // Restore SAM model identity and metadata preserved in namespaced user_data
            if (Core.LadybugTools.Query.TryGetUserData(model, Core.LadybugTools.UserDataKeys.Name, out string name) && !string.IsNullOrWhiteSpace(name))
            {
                result = new AnalyticalModel(name, result.Description, result.Location, result.Address, result.AdjacencyCluster, result.MaterialLibrary, result.ProfileLibrary);
            }

            if (Core.LadybugTools.Query.TryGetUserData(model, Core.LadybugTools.UserDataKeys.Description, out string description))
            {
                result = new AnalyticalModel(result.Name, description, result.Location, result.Address, result.AdjacencyCluster, result.MaterialLibrary, result.ProfileLibrary);
            }

            if (Core.LadybugTools.Query.TryGetUserData(model, Core.LadybugTools.UserDataKeys.LocationLatitude, out double latitude)
                && Core.LadybugTools.Query.TryGetUserData(model, Core.LadybugTools.UserDataKeys.LocationLongitude, out double longitude))
            {
                Core.LadybugTools.Query.TryGetUserData(model, Core.LadybugTools.UserDataKeys.LocationName, out string locationName);
                Core.LadybugTools.Query.TryGetUserData(model, Core.LadybugTools.UserDataKeys.LocationElevation, out double elevation);

                Core.Location location = new Core.Location(locationName, longitude, latitude, double.IsNaN(elevation) ? 0 : elevation);
                result = new AnalyticalModel(result, location);
            }

            if (Core.LadybugTools.Query.TryGetUserData(model, Core.LadybugTools.UserDataKeys.ProfileLibraryName, out string profileLibraryName) && !string.IsNullOrWhiteSpace(profileLibraryName))
            {
                ProfileLibrary profileLibrary_Temp = result.ProfileLibrary;
                if (profileLibrary_Temp == null)
                {
                    profileLibrary_Temp = new ProfileLibrary(profileLibraryName);
                }
                else if (string.IsNullOrWhiteSpace(profileLibrary_Temp.Name))
                {
                    profileLibrary_Temp = new ProfileLibrary(profileLibraryName, profileLibrary_Temp.GetProfiles());
                }

                if (profileLibrary_Temp != result.ProfileLibrary)
                {
                    result = new AnalyticalModel(result, result.AdjacencyCluster, result.MaterialLibrary, profileLibrary_Temp);
                }
            }

            if (Query.TryGetSAMGuid(model, out Guid modelGuid) && modelGuid != result.Guid)
            {
                // AnalyticalModel exposes no safe (Guid, AnalyticalModel) constructor, so the
                // model-level GUID can currently only be restored via a full JSON round trip.
                // That reconstruction can fail on real production payloads; never let GUID
                // preservation destroy an otherwise valid converted model.
                AnalyticalModel result_Restored = TryRestoreGuid(result, modelGuid);
                if (result_Restored != null)
                {
                    result = result_Restored;
                }
            }

            return result;
        }

        /// <summary>
        /// Attempts to rebuild <paramref name="analyticalModel"/> with the preserved model-level
        /// <paramref name="guid"/> via a JSON round trip. Returns null when serialisation or
        /// reconstruction fails or produces an invalid model, in which case the caller must keep
        /// the original (valid) instance and accept that the model-level GUID is not restored.
        /// </summary>
        private static AnalyticalModel TryRestoreGuid(AnalyticalModel analyticalModel, Guid guid)
        {
            if (analyticalModel == null || guid == Guid.Empty)
            {
                return null;
            }

            try
            {
                System.Text.Json.Nodes.JsonObject jsonObject = analyticalModel.ToJsonObject();
                if (jsonObject == null)
                {
                    return null;
                }

                jsonObject["Guid"] = guid.ToString();

                AnalyticalModel result = new AnalyticalModel(jsonObject);
                if (result == null || result.Guid != guid)
                {
                    return null;
                }

                // Validate the reconstruction preserved the model content
                AdjacencyCluster adjacencyCluster_Original = analyticalModel.AdjacencyCluster;
                AdjacencyCluster adjacencyCluster_Restored = result.AdjacencyCluster;
                if (adjacencyCluster_Restored == null)
                {
                    return null;
                }

                if ((adjacencyCluster_Original?.GetPanels()?.Count ?? 0) != (adjacencyCluster_Restored.GetPanels()?.Count ?? 0)
                    || (adjacencyCluster_Original?.GetSpaces()?.Count ?? 0) != (adjacencyCluster_Restored.GetSpaces()?.Count ?? 0))
                {
                    return null;
                }

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}