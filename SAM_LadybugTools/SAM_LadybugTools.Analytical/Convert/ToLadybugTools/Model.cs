using HoneybeeSchema;
using System.Collections.Generic;
using System.Linq;

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

                    Room room = space.ToLadybugTools(adjacencyCluster, silverSpacing, tolerance);
                    if (room == null)
                        continue;

                    IdealAirSystemAbridged idealAirSystemAbridged = new IdealAirSystemAbridged(string.Format("{0}_{1}", "IASA", room.Identifier), string.Format("Ideal Air System Abridged {0}", space.Name));
                    hvacs.Add(idealAirSystemAbridged);

                    if (room.Properties == null)
                        room.Properties = new RoomPropertiesAbridged();

                    if (room.Properties.Energy == null)
                        room.Properties.Energy = new RoomEnergyPropertiesAbridged();

                    room.Properties.Energy.Hvac = idealAirSystemAbridged.Identifier;
                    room.Properties.Energy.ConstructionSet = "Default Generic Construction Set";
                    room.Properties.Energy.ProgramType = "Generic Office Program";

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

            ConstructionSetAbridged constructionSetAbridged = Query.StandardConstructionSetAbridged("Default Generic Construction Set", Core.TextComparisonType.Equals, true);
            List<AnyOf<ConstructionSetAbridged, ConstructionSet>> constructionSets = new List<AnyOf<ConstructionSetAbridged, ConstructionSet>>() { constructionSetAbridged  };


            List<AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, WindowConstructionShadeAbridged, AirBoundaryConstructionAbridged, OpaqueConstruction, WindowConstruction, WindowConstructionShade, AirBoundaryConstruction, ShadeConstruction>> constructions = new List<AnyOf<OpaqueConstructionAbridged, WindowConstructionAbridged, WindowConstructionShadeAbridged, AirBoundaryConstructionAbridged, OpaqueConstruction, WindowConstruction, WindowConstructionShade, AirBoundaryConstruction, ShadeConstruction>>();
            HoneybeeSchema.Helper.EnergyLibrary.DefaultConstructions?.ToList().ForEach(x => constructions.Add(x as dynamic));

            List<AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGas, EnergyWindowMaterialGasCustom, EnergyWindowMaterialGasMixture, EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialBlind, EnergyWindowMaterialGlazing, EnergyWindowMaterialShade>> materials = new List<AnyOf<EnergyMaterial, EnergyMaterialNoMass, EnergyWindowMaterialGas, EnergyWindowMaterialGasCustom, EnergyWindowMaterialGasMixture, EnergyWindowMaterialSimpleGlazSys, EnergyWindowMaterialBlind, EnergyWindowMaterialGlazing, EnergyWindowMaterialShade>>();
            HoneybeeSchema.Helper.EnergyLibrary.DefaultMaterials?.ToList().ForEach(x => materials.Add(x as dynamic));

            List<AnyOf<ScheduleRulesetAbridged, ScheduleFixedIntervalAbridged, ScheduleRuleset, ScheduleFixedInterval>> schedules = new List<AnyOf<ScheduleRulesetAbridged, ScheduleFixedIntervalAbridged, ScheduleRuleset, ScheduleFixedInterval>>();
            HoneybeeSchema.Helper.EnergyLibrary.DefaultScheduleRuleset?.ToList().ForEach(x => schedules.Add(x as dynamic));

            List<AnyOf<ProgramTypeAbridged, ProgramType>> programTypes = new List<AnyOf<ProgramTypeAbridged, ProgramType>>();
            HoneybeeSchema.Helper.EnergyLibrary.DefaultProgramTypes?.ToList().ForEach(x => programTypes.Add(x as dynamic));

            List<ScheduleTypeLimit> scheduleTypeLimits = new List<ScheduleTypeLimit>();
            HoneybeeSchema.Helper.EnergyLibrary.DefaultScheduleTypeLimit?.ToList().ForEach(x => scheduleTypeLimits.Add(x));

            ModelEnergyProperties modelEnergyProperties = new ModelEnergyProperties(constructionSets, constructions, materials, hvacs, programTypes, schedules, scheduleTypeLimits);

            ModelProperties modelProperties = new ModelProperties(modelEnergyProperties);

            Model model = new Model(uniqueName, modelProperties, adjacencyCluster.Name, null, "1.38.1", rooms, faces_Orphaned, shades);

            return model;
        }
    }
}