using HoneybeeSchema;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static PanelGroup ToSAM(this FaceType faceType)
        {
            switch(faceType)
            {
                case FaceType.AirBoundary:
                    return PanelGroup.Other;
                case FaceType.Floor:
                    return PanelGroup.Floor;
                case FaceType.RoofCeiling:
                    return PanelGroup.Roof;
                case FaceType.Wall:
                    return PanelGroup.Wall;
            }

            return PanelGroup.Undefined;
        }
    }
}