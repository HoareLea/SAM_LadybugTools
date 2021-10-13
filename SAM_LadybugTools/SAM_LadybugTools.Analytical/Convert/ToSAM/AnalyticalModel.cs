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
            List<Construction> constructions = null;
            List<ApertureConstruction> apertureConstructions = null;

            ModelEnergyProperties modelEnergyProperties = model.Properties?.Energy;
            if(modelEnergyProperties != null)
            {
                materialLibrary = modelEnergyProperties.ToSAM_MaterialLibrary();
                constructions = modelEnergyProperties.ToSAM_Constructions();
                apertureConstructions = modelEnergyProperties.ToSAM_ApertureConstructions();
                
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

                    Space space = room.ToSAM();
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



            AnalyticalModel result = new AnalyticalModel(model.DisplayName, null, null, null, adjacencyCluster, materialLibrary, null);

            return result;
        }
    }
}