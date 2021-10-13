using HoneybeeSchema;
using HoneybeeSchema.Energy;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static List<ApertureConstruction> ToSAM_ApertureConstructions(this ModelEnergyProperties modelEnergyProperties, MaterialLibrary materialLibrary = null)
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

            List<ApertureConstruction> result = new List<ApertureConstruction>();
            foreach(IConstruction construction_Honeybee in constructions_Honeybee)
            {
                ApertureConstruction apertureConstruction = construction_Honeybee?.ToSAM_ApertureConstruction(materialLibrary);
                if(apertureConstruction != null)
                {
                    result.Add(apertureConstruction);
                }
            }

            return result;
        }
    }
}