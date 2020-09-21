using HoneybeeSchema;
using SAM.Core;
using System;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static GasType? GasType(this GasMaterial gasMaterial)
        {
            if (gasMaterial == null)
                return null;

            string name_gasMaterial = gasMaterial.Name;
            name_gasMaterial = name_gasMaterial?.ToUpper().Trim();

            foreach(GasType gasType in Enum.GetValues(typeof(GasType)))
            {
                string name_GasType = gasType.ToString().ToUpper().Trim();
                if (name_GasType.Contains(name_gasMaterial))
                    return gasType;
            }

            return null;
        }
    }
}