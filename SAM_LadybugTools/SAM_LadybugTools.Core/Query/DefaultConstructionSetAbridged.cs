using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        public static ConstructionSetAbridged DefaultConstructionSetAbridged(string text, TextComparisonType textComparisonType, bool caseSensitive = true)
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

        public static ConstructionSetAbridged DefaultConstructionSetAbridged()
        {
            return DefaultConstructionSetAbridged("Default Generic Construction Set", TextComparisonType.Equals, true);
        }
    }
}