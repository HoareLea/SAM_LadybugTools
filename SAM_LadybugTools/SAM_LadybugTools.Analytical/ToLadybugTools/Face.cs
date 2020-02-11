using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoneybeeDotNet;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Face ToLadybugTools(this Panel panel)
        {
            if (panel == null)
                return null;

            Face3D face3D = panel.PlanarBoundary3D.ToLadybugTools();

            Surface surface = new Surface(new List<string>());

            return new Face(panel.Name, face3D, Query.FaceTypeEnum(panel.PanelType), new AnyOf<Ground, Outdoors, Adiabatic, Surface>(surface), new FacePropertiesAbridged());
        }
    }
}
