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
            List<AnyOf<IdealAirSystemAbridged, VAV, PVAV, PSZ, PTAC, ForcedAirFurnace, FCUwithDOAS, WSHPwithDOAS, VRFwithDOAS, FCU, WSHP, VRF, Baseboard, EvaporativeCooler, Residential, WindowAC, GasUnitHeater>> hvacs = null;

            List<Space> spaces = adjacencyCluster.GetSpaces();
            if (spaces != null)
            {
                hvacs = new List<AnyOf<IdealAirSystemAbridged, VAV, PVAV, PSZ, PTAC, ForcedAirFurnace, FCUwithDOAS, WSHPwithDOAS, VRFwithDOAS, FCU, WSHP, VRF, Baseboard, EvaporativeCooler, Residential, WindowAC, GasUnitHeater>>();
                rooms = new List<Room>();

                for(int i=0; i < spaces.Count; i++)
                {
                    Space space = spaces[i];
                    if (space == null)
                        continue;

                    IdealAirSystemAbridged idealAirSystemAbridged = new IdealAirSystemAbridged(string.Format("{0}_{1}", "IASA", uniqueName), string.Format("Ideal Air System Abridged {0}", space.Name));

                    Room room = space.ToLadybugTools(adjacencyCluster, silverSpacing, tolerance);
                    if (room == null)
                        continue;

                    if(room.Properties == null)
                        room.Properties = new RoomPropertiesAbridged();

                    if (room.Properties.Energy == null)
                        room.Properties.Energy = new RoomEnergyPropertiesAbridged();

                    room.Properties.Energy.Hvac = idealAirSystemAbridged.Identifier;

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

            ModelEnergyProperties modelEnergyProperties = new ModelEnergyProperties(null, null, null, hvacs);

            ModelProperties modelProperties = new ModelProperties(modelEnergyProperties);

            Model model = new Model(uniqueName, modelProperties, adjacencyCluster.Name, null, "1.38.1", rooms, faces_Orphaned, shades);

            return model;
        }
    }
}