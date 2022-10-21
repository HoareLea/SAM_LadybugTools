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
                panels = adjacencyCluster.UpdateNormals(space, false, true, false, silverSpacing, tolerance);
            }

            if(panels == null || panels.Count == 0)
            {
                return null;
            }

            string uniqueName = Query.UniqueName(space);

            List<Face> faces = null;
            if(panels != null)
            {
                faces = new List<Face>();
                foreach(Panel panel in panels)
                {
                    if (panel == null)
                        continue;

                    bool reverse = true;

                    List<Space> spaces = adjacencyCluster?.GetSpaces(panel);
                    if (spaces != null && spaces.Count > 1)
                    {
                        reverse = adjacencyCluster.GetIndex(spaces[0]) != index;
                    }
                    
                    Face face = panel.ToLadybugTools_Face(analyticalModel, index, reverse);
                    if (face == null)
                        continue;

                    faces.Add(face);
                }
            }

            RoomPropertiesAbridged roomPropertiesAbridged = new RoomPropertiesAbridged();

            Room result = new Room(uniqueName, faces, roomPropertiesAbridged, space.Name);

            InternalCondition internalCondition = space.InternalCondition;
            if(internalCondition != null)
            {
                string uniqueName_InternalCondition = Core.LadybugTools.Query.UniqueName(internalCondition);
                if (!string.IsNullOrWhiteSpace(uniqueName_InternalCondition))
                {
                    roomPropertiesAbridged = result.Properties;
                    RoomEnergyPropertiesAbridged roomEnergyPropertiesAbridged = roomPropertiesAbridged.Energy;
                    if (roomEnergyPropertiesAbridged == null)
                        roomEnergyPropertiesAbridged = new RoomEnergyPropertiesAbridged(programType: uniqueName_InternalCondition);

                    result.Properties.Energy = roomEnergyPropertiesAbridged;
                }
            }
            
            return result;
        }
    }
}