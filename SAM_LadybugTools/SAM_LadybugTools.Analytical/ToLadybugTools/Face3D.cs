using System.Collections.Generic;

using HoneybeeDotNet;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static Face3D ToLadybugTools(this PlanarBoundary3D planarBoundary3D)
        {
            if (planarBoundary3D == null)
                return null;

            List<List<double>> boundary = ToLadybugTools(planarBoundary3D.GetEdge3DLoop());
            
            List<BoundaryEdge3DLoop> internalBoundaryEdge3DLoops = planarBoundary3D.GetInternalEdge3DLoops();
            List<List<List<double>>> holes = null;
            if (internalBoundaryEdge3DLoops != null)
            {
                holes = new List<List<List<double>>>();
                foreach(BoundaryEdge3DLoop internalBoundaryEdge3DLoop in internalBoundaryEdge3DLoops)
                    holes.Add(ToLadybugTools(internalBoundaryEdge3DLoop));
            }

            return new Face3D(boundary, "Face3D", holes);

        }
    }
}
