using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static string DefaultConstructionName(this PanelType panelType)
        {
            if(panelType == Analytical.PanelType.Undefined)
            {
                return null;
            }

            GlobalConstructionSet globalConstructionSet = GlobalConstructionSet.Default;
            if(globalConstructionSet == null)
            {
                return null;
            }

            switch(panelType)
            {
                case Analytical.PanelType.Floor:
                    return globalConstructionSet.FloorSet?.InteriorConstruction;
                case Analytical.PanelType.Air:
                    return globalConstructionSet.AirBoundaryConstruction;
                case Analytical.PanelType.UndergroundSlab:
                    return globalConstructionSet.FloorSet.ExteriorConstruction;
                case Analytical.PanelType.UndergroundWall:
                    return globalConstructionSet.WallSet.ExteriorConstruction;
                case Analytical.PanelType.Undefined:
                    return null;
                case Analytical.PanelType.FloorExposed:
                    return globalConstructionSet.FloorSet.ExteriorConstruction;
                case Analytical.PanelType.Shade:
                    return globalConstructionSet.WallSet.InteriorConstruction;
                case Analytical.PanelType.Roof:
                    return globalConstructionSet.RoofCeilingSet.ExteriorConstruction;
                case Analytical.PanelType.WallExternal:
                    return globalConstructionSet.WallSet.ExteriorConstruction;
                case Analytical.PanelType.Ceiling:
                    return globalConstructionSet.RoofCeilingSet.InteriorConstruction;
                case Analytical.PanelType.WallInternal:
                    return globalConstructionSet.WallSet.InteriorConstruction;
                case Analytical.PanelType.SlabOnGrade:
                    return globalConstructionSet.FloorSet.GroundConstruction;
            }

            return null;
        }
    }
}