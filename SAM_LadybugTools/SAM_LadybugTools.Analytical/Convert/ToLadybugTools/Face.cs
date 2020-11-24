using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Face ToLadybugTools_Face(this Panel panel, AnalyticalModel analyticalModel = null, int index = -1)
        {
            if (panel == null || panel.PanelType == PanelType.Shade)
                return null;

            Face3D face3D = panel.PlanarBoundary3D.ToLadybugTools();
            if (face3D == null)
                return null;

            AdjacencyCluster adjacencyCluster = analyticalModel?.AdjacencyCluster;

            Space space_Adjacent = null;
            int index_Adjacent = -1;
            if (adjacencyCluster != null && index != -1)
            {
                List<Space> spaces = adjacencyCluster.GetSpaces(panel);
                if (spaces != null && spaces.Count != 0)
                {
                    foreach (Space space in spaces)
                    {
                        int index_Temp = adjacencyCluster.GetIndex(space);
                        if (!index_Temp.Equals(index))
                        {
                            space_Adjacent = space;
                            index_Adjacent = index_Temp;
                            break;
                        }
                    }
                }
            }

            string adjacentPanelUniqueName = null;
            string adjacentSpaceUniqueName = null;

            if (space_Adjacent != null && index_Adjacent != -1)
            {
                adjacentPanelUniqueName = Core.LadybugTools.Query.UniqueName(panel, index_Adjacent);
                adjacentSpaceUniqueName = Core.LadybugTools.Query.UniqueName(space_Adjacent, index_Adjacent);
            }

            AnyOf<Ground, Outdoors, Adiabatic, Surface> boundaryCondition = panel.ToLadybugTools_BoundaryCondition(adjacentPanelUniqueName, adjacentSpaceUniqueName);

            FaceType faceType;

            PanelType panelType = panel.PanelType;
            PanelGroup panelGroup = panelType.PanelGroup();
            if(panelGroup == PanelGroup.Floor && Analytical.Query.PanelType(panel.Normal) == PanelType.Roof)
                faceType = FaceType.RoofCeiling;
            else
                faceType = Query.FaceTypeEnum(panelType);

            FaceEnergyPropertiesAbridged faceEnergyPropertiesAbridged = new FaceEnergyPropertiesAbridged();
            if (faceType != FaceType.AirBoundary)
                faceEnergyPropertiesAbridged.Construction = Core.LadybugTools.Query.UniqueName(panel.Construction);

            Face face = new Face(Core.LadybugTools.Query.UniqueName(panel, index), face3D, faceType, boundaryCondition, new FacePropertiesAbridged(faceEnergyPropertiesAbridged), panel.Name);

            List<Aperture> apertures = panel.Apertures;//Analytical.Query.OffsetAperturesOnEdge(panel, 0.1);
            if (apertures != null && apertures.Count > 0)
            {
                MaterialLibrary materialLibrary = analyticalModel?.MaterialLibrary;
                
                face.Apertures = apertures.ConvertAll(x => x.ToLadybugTools(materialLibrary, index, index_Adjacent, adjacentPanelUniqueName, adjacentSpaceUniqueName)).FindAll(x => x != null);
                face.Doors = apertures.ConvertAll(x => x.ToLadybugTools_Door(materialLibrary, index, index_Adjacent, adjacentPanelUniqueName, adjacentSpaceUniqueName)).FindAll(x => x != null);
            }

            return face;
        }
    }
}