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
            
            List<Edge3DLoop> internalEdge3DLoops = planarBoundary3D.GetInternalEdge3DLoops();
            List<List<List<double>>> holes = null;
            if (internalEdge3DLoops != null)
            {
                holes = new List<List<List<double>>>();
                foreach(Edge3DLoop internalEdge3DLoop in internalEdge3DLoops)
                    holes.Add(ToLadybugTools(internalEdge3DLoop));
            }

            return new Face3D(boundary, "Face3D", holes);

        }
    }
}
