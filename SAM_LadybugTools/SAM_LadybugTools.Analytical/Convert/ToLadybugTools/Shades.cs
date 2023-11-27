using HoneybeeSchema;
using SAM.Geometry.LadybugTools;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<Shade> ToLadybugTools_Shades(this Panel panel)
        {
            if (panel == null || panel.PanelType != PanelType.Shade)
            {
                return null;
            }

            List<Geometry.Spatial.Face3D> face3Ds = panel.GetFace3Ds(true);
            if (face3Ds == null || face3Ds.Count == 0)
            {
                return null;
            }

            List<Face3D> face3s_LadybugTools = new List<Face3D>();
            foreach(Geometry.Spatial.Face3D face3D_Temp in face3Ds)
            {
                Face3D face3D_LadybugTools = face3D_Temp?.ToLadybugTools();
                if (face3D_LadybugTools == null)
                {
                    continue;

                }

                face3s_LadybugTools.Add(face3D_LadybugTools);
            }

            ShadePropertiesAbridged shadePropertiesAbridged = new ShadePropertiesAbridged();

            List<Shade> result = new List<Shade>();
            for (int i = 0; i < face3s_LadybugTools.Count; i++)
            {
                string name = Query.UniqueName(panel, face3s_LadybugTools.Count == 1 ? -1 : i + 1);

                Shade shade = new Shade(name, face3s_LadybugTools[i], shadePropertiesAbridged, panel.Name);

                result.Add(shade);
            }

            return result;
        }
    }
}