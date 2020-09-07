using System.Collections.Generic;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        public static string UniqueName(this SAMObject sAMObject, int index = -1)
        {
            if (sAMObject == null)
                return null;

            if (index == -1)
                return sAMObject.Guid.ToString("N");
            else
                return string.Format("{0}__{1}", index.ToString(), sAMObject.Guid.ToString("N"));
        }

        public static string UniqueName(string prefix, string uniqueName)
        {
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            return string.Format("{0}_{1}", prefix, uniqueName);
        }
    }
}