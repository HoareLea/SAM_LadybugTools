using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static AnyOf<Ground, Outdoors, Adiabatic, Surface> ToLadybugTools_BoundaryCondition(this Panel panel, Space space = null)
        {
            if (panel == null)
                return null;
            switch (panel.PanelType)
            {
                case PanelType.UndergroundWall:
                case PanelType.SlabOnGrade:
                case PanelType.UndergroundSlab:
                case PanelType.Floor:
                    return new Ground();

                case PanelType.Wall:
                case PanelType.CurtainWall:
                case PanelType.WallExternal:
                case PanelType.Roof:
                case PanelType.SolarPanel:
                case PanelType.Shade:
                case PanelType.FloorExposed:
                case PanelType.FloorRaised:
                    return new Outdoors();

                case PanelType.Undefined:
                    return new Adiabatic();

                case PanelType.FloorInternal:
                case PanelType.WallInternal:
                case PanelType.Ceiling:
                case PanelType.UndergroundCeiling:
                        //boundaryConditionObjects have to be provided
                        //https://www.ladybug.tools/honeybee-schema/model.html#tag/surface_model

                        List<string> uniqueNames = new List<string>();

                        uniqueNames.Add(Core.LadybugTools.Query.UniqueName(panel));

                        if (space != null)
                            uniqueNames.Add(Core.LadybugTools.Query.UniqueName(space));

                        return new Surface(uniqueNames); 
            }

            return null;
        }
    }
}