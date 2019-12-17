using Grasshopper.Kernel.Types;

namespace SAM.Geometry.Grasshopper.LadybugTools
{
    public static partial class Convert
    {
        public static GH_Line ToGrasshopper(this SAM.Geometry.Spatial.Segment3D segment3D)
        {
            //return new GH_Line(segment3D.ToRhino());
            return null;
        }
    }
}
