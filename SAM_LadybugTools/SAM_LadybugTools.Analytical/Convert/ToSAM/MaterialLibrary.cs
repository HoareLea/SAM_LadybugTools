﻿using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static MaterialLibrary ToSAM_MaterialLibrary(this ModelEnergyProperties modelEnergyProperties)
        {
            if (modelEnergyProperties == null)
            {
                return null;
            }

            IEnumerable<HoneybeeSchema.Energy.IMaterial> materials_Honeybee = modelEnergyProperties.MaterialList;
            if (materials_Honeybee == null)
            {
                return null;
            }

            MaterialLibrary result = new MaterialLibrary(string.Empty);
            result.AddMaterials(materials_Honeybee);

            return result;
        }
    }
}