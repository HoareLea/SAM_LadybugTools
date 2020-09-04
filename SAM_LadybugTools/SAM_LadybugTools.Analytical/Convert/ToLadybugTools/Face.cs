using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Face ToLadybugTools_Face(this Panel panel, Space space = null)
        {
            if (panel == null || panel.PanelType == PanelType.Shade)
                return null;

            Face3D face3D = panel.PlanarBoundary3D.ToLadybugTools();
            if (face3D == null)
                return null;

            AnyOf<Ground, Outdoors, Adiabatic, Surface> boundaryCondition = panel.ToLadybugTools_BoundaryCondition(space);

            Face face = new Face(Core.LadybugTools.Query.UniqueName(panel), face3D, Query.FaceTypeEnum(panel.PanelType), boundaryCondition, new FacePropertiesAbridged() { Energy = new FaceEnergyPropertiesAbridged() }, panel.Name);

            List<Aperture> apertures = Analytical.Query.OffsetAperturesOnEdge(panel, 0.1);
            if (apertures != null && apertures.Count > 0)
                face.Apertures = apertures.ConvertAll(x => x.ToLadybugTools(panel, space));

            return face;
        }
    }
}