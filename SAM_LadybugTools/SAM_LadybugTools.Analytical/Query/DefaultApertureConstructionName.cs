using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static string DefaultApertureConstructionName(this ApertureType apertureType, bool @internal = false)
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

        public static string DefaultApertureConstructionName(this ApertureType apertureType, PanelType panelType)
        {
            if (panelType == Analytical.PanelType.Undefined)
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
                case ApertureType.Door:
                    switch(panelType)
                    {
                        case Analytical.PanelType.Air:
                            return null;

                        case Analytical.PanelType.Ceiling:
                            return null;

                        case Analytical.PanelType.CurtainWall:
                            return globalConstructionSet.DoorSet.ExteriorGlassConstruction;

                        case Analytical.PanelType.Floor:
                            return null;

                        case Analytical.PanelType.FloorExposed:
                            return null;

                        case Analytical.PanelType.FloorInternal:
                            return null;

                        case Analytical.PanelType.FloorRaised:
                            return null;

                        case Analytical.PanelType.Roof:
                            return null;

                        case Analytical.PanelType.Shade:
                            return globalConstructionSet.DoorSet.ExteriorConstruction;

                        case Analytical.PanelType.SlabOnGrade:
                            return null;

                        case Analytical.PanelType.SolarPanel:
                            return null;

                        case Analytical.PanelType.Undefined:
                            return null;

                        case Analytical.PanelType.UndergroundCeiling:
                            return null;

                        case Analytical.PanelType.UndergroundSlab:
                            return null;

                        case Analytical.PanelType.UndergroundWall:
                            return globalConstructionSet.DoorSet.ExteriorConstruction;

                        case Analytical.PanelType.Wall:
                            return globalConstructionSet.DoorSet.ExteriorConstruction;

                        case Analytical.PanelType.WallExternal:
                            return globalConstructionSet.DoorSet.ExteriorConstruction;

                        case Analytical.PanelType.WallInternal:
                            return globalConstructionSet.DoorSet.InteriorConstruction;
                    }
                    break;


                case ApertureType.Window:
                    switch (panelType)
                    {
                        case Analytical.PanelType.Air:
                            return null;

                        case Analytical.PanelType.Ceiling:
                            return null;

                        case Analytical.PanelType.CurtainWall:
                            return globalConstructionSet.ApertureSet.WindowConstruction;

                        case Analytical.PanelType.Floor:
                            return null;

                        case Analytical.PanelType.FloorExposed:
                            return null;

                        case Analytical.PanelType.FloorInternal:
                            return null;

                        case Analytical.PanelType.FloorRaised:
                            return null;

                        case Analytical.PanelType.Roof:
                            return globalConstructionSet.ApertureSet.SkylightConstruction;

                        case Analytical.PanelType.Shade:
                            return globalConstructionSet.ApertureSet.WindowConstruction;

                        case Analytical.PanelType.SlabOnGrade:
                            return null;

                        case Analytical.PanelType.SolarPanel:
                            return null;

                        case Analytical.PanelType.Undefined:
                            return null;

                        case Analytical.PanelType.UndergroundCeiling:
                            return null;

                        case Analytical.PanelType.UndergroundSlab:
                            return null;

                        case Analytical.PanelType.UndergroundWall:
                            return globalConstructionSet.ApertureSet.WindowConstruction;

                        case Analytical.PanelType.Wall:
                            return globalConstructionSet.ApertureSet.OperableConstruction;

                        case Analytical.PanelType.WallExternal:
                            return globalConstructionSet.ApertureSet.OperableConstruction;

                        case Analytical.PanelType.WallInternal:
                            return globalConstructionSet.ApertureSet.InteriorConstruction;
                    }
                    break;
            }

            return null;
        }
    }
}