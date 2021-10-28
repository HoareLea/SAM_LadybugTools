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
                constructions = modelEnergyProperties.ToSAM_Constructions();
                apertureConstructions = modelEnergyProperties.ToSAM_ApertureConstructions();
                internalConditions = modelEnergyProperties.ToSAM_InternalConditions();
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

                        Panel panel_Existing = tuples.FindAll(x => x.Item2.Inside(point3D))?.Find(x => x.Item1.GetFace3D().On(point3D))?.Item1;
                        if (panel_Existing != null)
                        {
                            panel = panel_Existing;
                        }
                        else
                        {
                            tuples.Add(new Tuple<Panel, Geometry.Spatial.BoundingBox3D>(panel, panel.GetFace3D().GetBoundingBox()));
                            
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

            AnalyticalModel result = new AnalyticalModel(model.DisplayName, null, null, null, adjacencyCluster, materialLibrary, profileLibrary);

            return result;
        }
    }
}