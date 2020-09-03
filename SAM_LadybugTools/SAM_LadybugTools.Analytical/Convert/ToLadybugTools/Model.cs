using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Model ToLadybugTools(this AdjacencyCluster adjacencyCluster, double silverSpacing = Core.Tolerance.MacroDistance, double tolerance = Core.Tolerance.Distance)
        {
            if (adjacencyCluster == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(adjacencyCluster);

            List<Room> rooms = null;
            
            List<Space> spaces = adjacencyCluster.GetSpaces();
            if (spaces != null)
            {
                rooms = new List<Room>();
                
                foreach (Space space in spaces)
                {
                    List<Panel> panels = adjacencyCluster.UpdateNormals(space, false, silverSpacing, tolerance);
                    if (panels == null || panels.Count == 0)
                        continue;

                    Room room = space.ToLadybugTools(panels);
                    if (room == null)
                        continue;

                    rooms.Add(room);
                }
            }

            List<Shade> shades = null;
            List<Face> faces_Orphaned = null;

            List<Panel> panels_Shading = adjacencyCluster.GetShadingPanels();
            foreach(Panel panel_Shading in panels_Shading)
            {
                if (panels_Shading == null)
                    continue;

                if(panel_Shading.PanelType == PanelType.Shade)
                {
                    Shade shade = panel_Shading.ToLadybugTools_Shade();
                    if (shade == null)
                        continue;

                    if (shades == null)
                        shades = new List<Shade>();

                    shades.Add(shade);
                }
                else
                {
                    Face face_Orphaned = panel_Shading.ToLadybugTools_Face();
                    if (face_Orphaned == null)
                        continue;

                    if (faces_Orphaned == null)
                        faces_Orphaned = new List<Face>();

                    faces_Orphaned.Add(face_Orphaned);
                }


            }

            Model model = new Model(uniqueName, new ModelProperties(), adjacencyCluster.Name, null, "1.38.1",rooms, faces_Orphaned, shades);

            return model;
        }
    }
}