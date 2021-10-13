using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static List<ConstructionLayer> ConstructionLayers(this MaterialLibrary materialLibrary, IEnumerable<string> names)
        {
            if (materialLibrary == null || names == null)
                return null;

            List<ConstructionLayer> result = new List<ConstructionLayer>();
            foreach (string name in names)
            {
                
                Material material = materialLibrary.GetMaterials()?.Find(x => x.Name == null && x is Material) as Material;
                if (material == null)
                {
                    continue;
                }

                double thickness = material.GetValue<double>(MaterialParameter.DefaultThickness);
                result.Add(new ConstructionLayer(material.Name, thickness));
            }

            return result;
        }
    }
}