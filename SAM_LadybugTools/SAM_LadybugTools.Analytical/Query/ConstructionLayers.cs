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

            List<IMaterial> materials = new List<IMaterial>();
            foreach (string name in names)
            {
                Material material = materialLibrary.GetMaterials()?.Find(x => x is Material && name == x.Name) as Material;
                if (material == null)
                {
                    continue;
                }

                materials.Add(material);
            }

            return ConstructionLayers(materials);
        }

        public static List<ConstructionLayer> ConstructionLayers(this IEnumerable<IMaterial> materials)
        {
            if(materials == null)
            {
                return null;
            }

            List<ConstructionLayer> result = new List<ConstructionLayer>();
            foreach (IMaterial material in materials)
            {
                double thickness = material.GetValue<double>(Core.MaterialParameter.DefaultThickness);
                result.Add(new ConstructionLayer(material.Name, thickness));
            }

            return result;
        }
    }
}