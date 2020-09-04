using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static AnyOf<Ground, Outdoors, Adiabatic, Surface> ToLadybugTools_BoundaryCondition(this Panel panel, string adjacentPanelUniqueName = null, string adjacentSpaceUniqueName = null)
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

                    if(!string.IsNullOrEmpty(adjacentPanelUniqueName))
                        uniqueNames.Add(adjacentPanelUniqueName);

                    if (!string.IsNullOrEmpty(adjacentSpaceUniqueName))
                        uniqueNames.Add(adjacentSpaceUniqueName);

                    return new Surface(uniqueNames); 
            }

            return null;
        }
    }
}