namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static HoneybeeSchema.FaceType FaceTypeEnum(this PanelType panelType)
        {
            switch (panelType)
            {
                case Analytical.PanelType.Ceiling:
                case Analytical.PanelType.Roof:
                case Analytical.PanelType.SolarPanel:
                case Analytical.PanelType.UndergroundCeiling: //To be confirmed
                    return HoneybeeSchema.FaceType.RoofCeiling;

                case Analytical.PanelType.Wall:
                case Analytical.PanelType.CurtainWall:
                case Analytical.PanelType.UndergroundWall:
                case Analytical.PanelType.WallExternal:
                case Analytical.PanelType.WallInternal:
                    return HoneybeeSchema.FaceType.Wall;

                case Analytical.PanelType.Floor:
                case Analytical.PanelType.FloorExposed:
                case Analytical.PanelType.FloorInternal:
                case Analytical.PanelType.FloorRaised:
                case Analytical.PanelType.SlabOnGrade:
                case Analytical.PanelType.UndergroundSlab:
                    return HoneybeeSchema.FaceType.Floor;

                case Analytical.PanelType.Air:
                    return HoneybeeSchema.FaceType.AirBoundary;
            }

            return HoneybeeSchema.FaceType.AirBoundary;
        }
    }
}