using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static ConstructionSetAbridged StandardConstructionSetAbridged(string text, TextComparisonType textComparisonType, bool caseSensitive = true)
        {
            Dictionary<string, ConstructionSetAbridged> dictionary = HoneybeeSchema.Helper.EnergyLibrary.StandardsConstructionSets;
            if (dictionary == null)
                return null;
            
            foreach(KeyValuePair<string, ConstructionSetAbridged> keyValyePair in dictionary)
            {
                if (Core.Query.Compare(keyValyePair.Key, text, textComparisonType, caseSensitive))
                    return keyValyePair.Value;
            }

            return null;
        }
    }
}