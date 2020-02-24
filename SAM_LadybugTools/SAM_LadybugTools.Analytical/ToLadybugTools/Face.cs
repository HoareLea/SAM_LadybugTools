using System.Collections.Generic;

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

            AnyOf<Ground, Outdoors, Adiabatic, Surface> boundaryCondition = panel.ToLadybugTools_BoundaryCondition();

            Face face = new Face(panel.Construction.Name +"__" + panel.Guid.ToString(), face3D, Query.FaceTypeEnum(panel.PanelType), boundaryCondition, new FacePropertiesAbridged() { Energy = new FaceEnergyPropertiesAbridged() });

            List<Aperture> apertures = panel.Apertures;
            if (apertures != null && apertures.Count > 0)
                face.Apertures = apertures.ConvertAll(x => x.ToLadybugTools());

            return face;
        }
    }
}
