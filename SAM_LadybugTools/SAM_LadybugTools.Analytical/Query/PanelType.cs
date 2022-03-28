using HoneybeeSchema;
using SAM.Core;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static PanelType? PanelType(this GlobalConstructionSet globalConstructionSet, string name)
        {
            if (globalConstructionSet == null || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if(globalConstructionSet.AirBoundaryConstruction == name)
            {
                return Analytical.PanelType.Air;
            }

            if(globalConstructionSet.FloorSet.ExteriorConstruction == name)
            {
                return Analytical.PanelType.FloorExposed;
            }

            if (globalConstructionSet.FloorSet.InteriorConstruction == name)
            {
                return Analytical.PanelType.FloorInternal;
            }

            if (globalConstructionSet.FloorSet.GroundConstruction == name)
            {
                return Analytical.PanelType.SlabOnGrade;
            }

            if (globalConstructionSet.RoofCeilingSet.ExteriorConstruction == name)
            {
                return Analytical.PanelType.Roof;
            }

            if (globalConstructionSet.RoofCeilingSet.GroundConstruction == name)
            {
                return Analytical.PanelType.UndergroundCeiling;
            }

            if (globalConstructionSet.RoofCeilingSet.InteriorConstruction == name)
            {
                return Analytical.PanelType.Ceiling;
            }

            if (globalConstructionSet.ShadeConstruction == name)
            {
                return Analytical.PanelType.Shade;
            }

            if (globalConstructionSet.WallSet.ExteriorConstruction == name)
            {
                return Analytical.PanelType.WallExternal;
            }

            if (globalConstructionSet.WallSet.GroundConstruction == name)
            {
                return Analytical.PanelType.UndergroundWall;
            }

            if (globalConstructionSet.WallSet.InteriorConstruction == name)
            {
                return Analytical.PanelType.WallInternal;
            }

            if(globalConstructionSet.ApertureSet.InteriorConstruction == name)
            {
                return Analytical.PanelType.WallInternal;
            }

            if (globalConstructionSet.ApertureSet.OperableConstruction == name)
            {
                return Analytical.PanelType.WallExternal;
            }

            if (globalConstructionSet.ApertureSet.SkylightConstruction == name)
            {
                return Analytical.PanelType.Roof;
            }

            if (globalConstructionSet.ApertureSet.WindowConstruction == name)
            {
                return Analytical.PanelType.WallExternal;
            }

            if (globalConstructionSet.DoorSet.ExteriorConstruction == name)
            {
                return Analytical.PanelType.WallExternal;
            }

            if (globalConstructionSet.DoorSet.ExteriorGlassConstruction == name)
            {
                return Analytical.PanelType.CurtainWall;
            }

            if (globalConstructionSet.DoorSet.InteriorConstruction == name)
            {
                return Analytical.PanelType.WallInternal;
            }

            if (globalConstructionSet.DoorSet.InteriorGlassConstruction == name)
            {
                return Analytical.PanelType.WallInternal;
            }

            if (globalConstructionSet.DoorSet.OverheadConstruction == name)
            {
                return Analytical.PanelType.Roof;
            }

            return null;
        }
    }
}