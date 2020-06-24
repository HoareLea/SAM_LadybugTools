using static HoneybeeSchema.Face;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static HoneybeeSchema.FaceType FaceTypeEnum(this PanelType panelType)
        {
            switch (panelType)
            {
                case PanelType.Ceiling:
                case PanelType.Roof:
                case PanelType.SolarPanel:
                case PanelType.UndergroundCeiling: //To be confirmed
                    return HoneybeeSchema.FaceType.RoofCeiling;

                case PanelType.Wall:
                case PanelType.CurtainWall:
                case PanelType.UndergroundWall:
                case PanelType.WallExternal:
                case PanelType.WallInternal:
                    return HoneybeeSchema.FaceType.Wall;

                case PanelType.Floor:
                case PanelType.FloorExposed:
                case PanelType.FloorInternal:
                case PanelType.FloorRaised:
                case PanelType.SlabOnGrade:
                case PanelType.UndergroundSlab:
                    return HoneybeeSchema.FaceType.Floor;
            }

            return HoneybeeSchema.FaceType.AirBoundary;
        }
    }
}