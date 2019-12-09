using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel.Types;


namespace SAM_Ladybug.Geometry.Grasshopper
{
    public static partial class Convert
    {
        public static SAM.Geometry.Spatial.Point3D ToSAM_Point3D(this object point, double z = 0)
        {
            return new SAM.Geometry.Spatial.Point3D((point as dynamic)._x, (point as dynamic)._y, z);
        }
    }
}
