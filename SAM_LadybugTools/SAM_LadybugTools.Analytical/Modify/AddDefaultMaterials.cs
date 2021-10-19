using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Modify
    {
        public static List<IMaterial> AddDefaultMaterials(this MaterialLibrary materialLibrary, Construction construction)
        {
            if(materialLibrary == null || construction == null)
            {
                return null;
            }

            List<IMaterial> result = new List<IMaterial>();

            List<ConstructionLayer> constructionLayers = construction.ConstructionLayers;
            if (constructionLayers != null && constructionLayers.Count != 0)
            {
                foreach (ConstructionLayer constructionLayer in constructionLayers)
                {
                    if (materialLibrary.GetMaterial(constructionLayer.Name) != null)
                    {
                        continue;
                    }

                    IMaterial material = Convert.ToSAM(Core.LadybugTools.Query.DefaultMaterial(constructionLayer.Name));
                    if (material == null)
                    {
                        continue;
                    }

                    if(materialLibrary.Add(material))
                    {
                        result.Add(material);
                    }
                }
            }

            return result;
        }
    }
}
