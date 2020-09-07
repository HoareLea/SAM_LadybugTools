using HoneybeeSchema;
using SAM.Core;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        public static ConstructionSetAbridged StandardConstructionSetAbridged(string text, TextComparisonType textComparisonType, bool caseSensitive = true)
        {
           IEnumerable<ConstructionSetAbridged> constructionSetsAbridged = HoneybeeSchema.Helper.EnergyLibrary.DefaultConstructionSets;
            if (constructionSetsAbridged == null)
                return null;
            
            foreach(ConstructionSetAbridged constructionSetAbridged in constructionSetsAbridged)
            {
                if (Core.Query.Compare(constructionSetAbridged.Identifier, text, textComparisonType, caseSensitive))
                    return constructionSetAbridged;
            }

            return null;
        }
    }
}