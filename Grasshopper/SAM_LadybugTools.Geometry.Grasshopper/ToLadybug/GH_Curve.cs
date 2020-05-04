using Grasshopper.Kernel.Types;

namespace SAM.Geometry.Grasshopper.LadybugTools
{
    public static partial class Convert
    {
        public static GH_Curve ToGrasshopper(this Spatial.Polygon3D polygon3D)
        {
            //return new GH_Curve(polygon3D.ToRhino());
            return null;
        }
    }
}