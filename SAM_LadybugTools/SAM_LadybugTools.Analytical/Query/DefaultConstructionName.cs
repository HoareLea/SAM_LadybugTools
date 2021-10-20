using HoneybeeSchema;
using SAM.Core;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static string DefaultConstructionName(this PanelType panelType)
        {
            if(panelType == PanelType.Undefined)
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
                case PanelType.Floor:
                    return globalConstructionSet.FloorSet?.InteriorConstruction;
                case PanelType.Air:
                    return globalConstructionSet.AirBoundaryConstruction;
                case PanelType.UndergroundSlab:
                    return globalConstructionSet.FloorSet.ExteriorConstruction;
                case PanelType.UndergroundWall:
                    return globalConstructionSet.WallSet.ExteriorConstruction;
                case PanelType.Undefined:
                    return null;
                case PanelType.FloorExposed:
                    return globalConstructionSet.FloorSet.ExteriorConstruction;
                case PanelType.Shade:
                    return globalConstructionSet.WallSet.InteriorConstruction;
                case PanelType.Roof:
                    return globalConstructionSet.RoofCeilingSet.ExteriorConstruction;
                case PanelType.WallExternal:
                    return globalConstructionSet.WallSet.ExteriorConstruction;
                case PanelType.Ceiling:
                    return globalConstructionSet.RoofCeilingSet.InteriorConstruction;
                case PanelType.WallInternal:
                    return globalConstructionSet.WallSet.InteriorConstruction;
            }

            return null;
        }

        public static string DefaultConstructionName(this ApertureType apertureType, bool @internal = false)
        {
            if (apertureType == ApertureType.Undefined)
            {
                return null;
            }

            GlobalConstructionSet globalConstructionSet = GlobalConstructionSet.Default;
            if (globalConstructionSet == null)
            {
                return null;
            }

            switch (apertureType)
            {
                case ApertureType.Window:
                    return @internal ? globalConstructionSet.ApertureSet.InteriorConstruction : globalConstructionSet.ApertureSet.OperableConstruction;
                case ApertureType.Door:
                    return @internal ? globalConstructionSet.DoorSet.InteriorConstruction : globalConstructionSet.DoorSet.ExteriorConstruction;
            }

            return null;
        }
    }
}