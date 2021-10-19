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

            return AddDefaultMaterials(materialLibrary, construction.ConstructionLayers);
        }

        public static List<IMaterial> AddDefaultMaterials(this MaterialLibrary materialLibrary, ApertureConstruction apertureConstruction)
        {
            if (materialLibrary == null || apertureConstruction == null)
            {
                return null;
            }

            List<ConstructionLayer> constructionLayers = apertureConstruction.PaneConstructionLayers;
            if(apertureConstruction.FrameConstructionLayers != null)
            {
                if(constructionLayers == null)
                {
                    constructionLayers = new List<ConstructionLayer>();
                }

                constructionLayers.AddRange(apertureConstruction.FrameConstructionLayers);
            }


            return AddDefaultMaterials(materialLibrary, constructionLayers);
        }

        public static List<IMaterial> AddDefaultMaterials(this MaterialLibrary materialLibrary, IEnumerable<ConstructionLayer> constructionLayers)
        {
            if (materialLibrary == null || constructionLayers == null)
            {
                return null;
            }

            List<IMaterial> result = new List<IMaterial>();
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

                if (materialLibrary.Add(material))
                {
                    result.Add(material);
                }
            }

            return result;
        }
    }
}
