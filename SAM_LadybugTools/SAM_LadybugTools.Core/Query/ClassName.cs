using System.Collections.Generic;
using System.Reflection;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        public static string ClassName(this object @object)
        {
            if(@object == null)
            {
                return null;
            }

            if(!Core.Query.TryGetFieldValue(@object, ".class", out object @class))
            {
                return null;
            }

            if (!Core.Query.TryInvokeMethod(@class, "Get__name__", out string result, new object[] { @class }))
            {
                return null;
            }

            return result;
        }
    }
}