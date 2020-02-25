using static HoneybeeSchema.Face;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static FaceTypeEnum FaceTypeEnum(this PanelType panelType)
        {
            switch(panelType)
            {
                case PanelType.Ceiling:
                case PanelType.Roof:
                case PanelType.Shade:
                case PanelType.SolarPanel:
                case PanelType.UndergroundCeiling: //To be confirmed
                    return HoneybeeSchema.Face.FaceTypeEnum.RoofCeiling;
                case PanelType.Wall:
                case PanelType.CurtainWall:
                case PanelType.UndergroundWall:
                case PanelType.WallExternal:
                case PanelType.WallInternal:
                    return HoneybeeSchema.Face.FaceTypeEnum.Wall;
                case PanelType.Floor:
                case PanelType.FloorExposed:
                case PanelType.FloorInternal:
                case PanelType.FloorRaised:
                case PanelType.SlabOnGrade:
                case PanelType.UndergroundSlab:
                    return HoneybeeSchema.Face.FaceTypeEnum.Floor;
            }

            return HoneybeeSchema.Face.FaceTypeEnum.AirBoundary;
        }
    }
}
