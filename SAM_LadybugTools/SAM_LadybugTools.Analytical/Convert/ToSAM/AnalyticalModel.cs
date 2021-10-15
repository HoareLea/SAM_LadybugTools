using HoneybeeSchema;
using SAM.Core;
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
                        if(panel != null)
                        {
                            panels.Add(panel);
                        }
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
                        adjacencyCluster.AddObject(panel);
                    }
                }
            }

            AnalyticalModel result = new AnalyticalModel(model.DisplayName, null, null, null, adjacencyCluster, materialLibrary, profileLibrary);

            return result;
        }
    }
}