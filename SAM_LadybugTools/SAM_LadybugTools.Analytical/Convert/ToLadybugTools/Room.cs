using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Room ToLadybugTools(this Space space, AdjacencyCluster adjacencyCluster = null, double silverSpacing = Core.Tolerance.MacroDistance, double tolerance = Core.Tolerance.Distance)
        {
            if (space == null)
                return null;

            int index = -1;
            List<Panel> panels = null;
            if(adjacencyCluster != null)
            {
                index = adjacencyCluster.GetIndex(space);
                panels = adjacencyCluster.UpdateNormals(space, false, silverSpacing, tolerance);
            }

            List<Face> faces = null;
            if(panels != null)
            {
                faces = new List<Face>();
                foreach(Panel panel in panels)
                {
                    Face face = panel.ToLadybugTools_Face(adjacencyCluster, index);
                    if (face == null)
                        continue;

                    faces.Add(face);
                }
            }

            string uniqueName = Core.LadybugTools.Query.UniqueName(space, index);

            Room result = new Room(uniqueName, faces, new RoomPropertiesAbridged(), space.Name);

            return result;
        }
    }
}