using HoneybeeSchema;
using SAM.Core;
using System;
using System.Collections.Generic;

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

            List<GasType> gasTypes = new List<GasType>();
            foreach(GasType gasType in Enum.GetValues(typeof(GasType)))
            {
                string name_GasType = gasType.ToString().ToUpper().Trim();
                if (name_gasMaterial.Contains(name_GasType))
                    gasTypes.Add(gasType);
            }

            if (gasTypes.Count == 0)
                return null;

            if (gasTypes.Count != 1)
                gasTypes.Sort((x, y) => y.ToString().Length.CompareTo(x.ToString().Length));

            return gasTypes[0];
        }
    }
}