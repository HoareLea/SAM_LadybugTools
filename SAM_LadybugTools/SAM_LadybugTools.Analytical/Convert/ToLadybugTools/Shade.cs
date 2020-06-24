using HoneybeeSchema;
using SAM.Geometry.LadybugTools;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Shade ToLadybugTools_Shade(this Panel panel)
        {
            if (panel == null || panel.PanelType != PanelType.Shade)
                return null;

            Face3D face3D = panel.GetFace3D(true)?.ToLadybugTools();
            if (face3D == null)
                return null;

            ShadePropertiesAbridged shadePropertiesAbridged = new ShadePropertiesAbridged();

            Shade shade = new Shade(Core.LadybugTools.Query.UniqueName(panel), face3D, shadePropertiesAbridged);

            return shade;
        }
    }
}