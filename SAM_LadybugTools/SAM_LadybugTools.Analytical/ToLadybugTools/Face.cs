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

            AnyOf<Ground, Outdoors, Adiabatic, Surface> boundaryCondition = panel.ToLadybugTools_BoundaryCondition();

            return new Face((panel.Construction.Name +"__" + panel.Guid.ToString()), face3D, Query.FaceTypeEnum(panel.PanelType), boundaryCondition, new FacePropertiesAbridged() { Energy = new FaceEnergyPropertiesAbridged() });
        }
    }
}
