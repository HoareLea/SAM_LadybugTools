using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Modify
    {
        public static List<IMaterial> AddMaterials(this MaterialLibrary materialLibrary, IEnumerable<HoneybeeSchema.Energy.IMaterial> materials)
        {
            if(materialLibrary == null || materials == null)
            {
                return null;
            }

            List<IMaterial> result = new List<IMaterial>();
            foreach (HoneybeeSchema.Energy.IMaterial material_Honeybee in materials)
            {
                IMaterial material = material_Honeybee.ToSAM();
                if (material != null)
                {
                    materialLibrary.Add(material);
                    result.Add(material);
                }
            }

            return result;
        }
    }
}
