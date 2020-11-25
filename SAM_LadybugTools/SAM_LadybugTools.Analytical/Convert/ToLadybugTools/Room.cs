using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Room ToLadybugTools(this Space space, AnalyticalModel analyticalModel = null, double silverSpacing = Core.Tolerance.MacroDistance, double tolerance = Core.Tolerance.Distance)
        {
            if (space == null)
                return null;

            int index = -1;
            List<Panel> panels = null;

            AdjacencyCluster adjacencyCluster = analyticalModel?.AdjacencyCluster;
            if (adjacencyCluster != null)
            {
                index = adjacencyCluster.GetIndex(space);
                panels = adjacencyCluster.UpdateNormals(space, false, silverSpacing, tolerance);
            }

            string uniqueName = Core.LadybugTools.Query.UniqueName(space, index);

            List<Face> faces = null;
            if(panels != null)
            {
                faces = new List<Face>();
                foreach(Panel panel in panels)
                {
                    if (panel == null)
                        continue;
                    
                    Face face = panel.ToLadybugTools_Face(analyticalModel, index);
                    if (face == null)
                        continue;

                    faces.Add(face);
                }
            }

            Room result = new Room(uniqueName, faces, new RoomPropertiesAbridged(), space.Name);
            return result;
        }
    }
}