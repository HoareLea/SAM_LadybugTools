using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel.Types;

namespace SAM_LadybugTools.Geometry.Grasshopper
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
