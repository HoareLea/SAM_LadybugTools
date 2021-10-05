using HoneybeeSchema;
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
                        Panel panel = face.ToSAM();
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

            AnalyticalModel result = new AnalyticalModel(model.DisplayName, null, null, null, adjacencyCluster, null, null);

            return result;
        }
    }
}