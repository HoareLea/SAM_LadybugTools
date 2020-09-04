using HoneybeeSchema;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Face ToLadybugTools_Face(this Panel panel, AdjacencyCluster adjacencyCluster = null, int index = -1)
        {
            if (panel == null || panel.PanelType == PanelType.Shade)
                return null;

            Face3D face3D = panel.PlanarBoundary3D.ToLadybugTools();
            if (face3D == null)
                return null;

            Space space_Adjacent = null;
            int index_Adjacent = -1;
            if(adjacencyCluster != null)
            {
                List<Space> spaces = adjacencyCluster.GetSpaces(panel);
                if(spaces != null && spaces.Count != 0)
                {
                    foreach(Space space in spaces)
                    {
                        index_Adjacent = adjacencyCluster.GetIndex(space);
                        space_Adjacent = space;
                        if (!index_Adjacent.Equals(index))
                            break;
                    }
                }
            }

            string adjacentPanelUniqueName = null;
            string adjacentSpaceUniqueName = null;

            if (space_Adjacent != null)
            {
                adjacentPanelUniqueName = Core.LadybugTools.Query.UniqueName(panel, index_Adjacent);
                adjacentSpaceUniqueName = Core.LadybugTools.Query.UniqueName(space_Adjacent, index_Adjacent);
            }

            AnyOf<Ground, Outdoors, Adiabatic, Surface> boundaryCondition = panel.ToLadybugTools_BoundaryCondition(adjacentPanelUniqueName, adjacentSpaceUniqueName);

            Face face = new Face(Core.LadybugTools.Query.UniqueName(panel, index), face3D, Query.FaceTypeEnum(panel.PanelType), boundaryCondition, new FacePropertiesAbridged() { Energy = new FaceEnergyPropertiesAbridged() }, panel.Name);

            List<Aperture> apertures = Analytical.Query.OffsetAperturesOnEdge(panel, 0.1);
            if (apertures != null && apertures.Count > 0)
            {
                face.Apertures = apertures.ConvertAll(x => x.ToLadybugTools(index, index_Adjacent, adjacentPanelUniqueName, adjacentSpaceUniqueName)).FindAll(x => x != null);
                face.Doors = apertures.ConvertAll(x => x.ToLadybugTools_Door(index, index_Adjacent, adjacentPanelUniqueName, adjacentSpaceUniqueName)).FindAll(x => x != null);
            }
                
            return face;
        }
    }
}