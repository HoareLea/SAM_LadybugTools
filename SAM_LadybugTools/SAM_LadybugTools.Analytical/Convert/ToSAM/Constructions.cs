using HoneybeeSchema;
using HoneybeeSchema.Energy;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<Construction> ToSAM_Constructions(this ModelEnergyProperties modelEnergyProperties, MaterialLibrary materialLibrary = null)
        {
            if(modelEnergyProperties == null)
            {
                return null;
            }

            IEnumerable<IConstruction> constructions_Honeybee = modelEnergyProperties.ConstructionList;
            if(constructions_Honeybee == null)
            {
                return null;
            }

            List<Construction> result = new List<Construction>();
            foreach(IConstruction construction_Honeybee in constructions_Honeybee)
            {
                Construction construction = construction_Honeybee?.ToSAM_Construction(materialLibrary);
                if(construction != null)
                {
                    result.Add(construction);
                }
            }

            return result;
        }
    }
}