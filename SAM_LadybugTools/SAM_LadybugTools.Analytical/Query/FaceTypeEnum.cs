using static HoneybeeDotNet.Face;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static FaceTypeEnum FaceTypeEnum(this PanelType panelType)
        {
            switch(panelType)
            {
                //TOD: Finish all types
                case PanelType.Ceiling:
                case PanelType.Roof:
                    return HoneybeeDotNet.Face.FaceTypeEnum.RoofCeiling;
                case PanelType.Wall:
                    return HoneybeeDotNet.Face.FaceTypeEnum.Wall;
            }

            return HoneybeeDotNet.Face.FaceTypeEnum.AirWall;
        }
    }
}
