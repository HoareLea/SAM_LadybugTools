using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Face ToLadybugTools_Face(this Panel panel)
        {
            if (panel == null || panel.PanelType == PanelType.Shade)
                return null;

            Face3D face3D = panel.PlanarBoundary3D.ToLadybugTools();
            if (face3D == null)
                return null;

            AnyOf<Ground, Outdoors, Adiabatic, Surface> boundaryCondition = panel.ToLadybugTools_BoundaryCondition();

            Face face = new Face(Core.LadybugTools.Query.UniqueName(panel), face3D, Query.FaceTypeEnum(panel.PanelType), boundaryCondition, new FacePropertiesAbridged() { Energy = new FaceEnergyPropertiesAbridged() });

            List<Aperture> apertures = panel.Apertures;
            if (apertures != null && apertures.Count > 0)
                face.Apertures = apertures.ConvertAll(x => x.ToLadybugTools());

            return face;
        }
    }
}