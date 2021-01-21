using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        public static string UniqueName(this SAMObject sAMObject, int index = -1)
        {
            if (sAMObject == null)
                return null;

            List<string> values = new List<string>();
            if (index != -1)
                values.Add(index.ToString());

            string name = sAMObject.Name;
            if(!string.IsNullOrWhiteSpace(name))
            {
                name = name.Replace(" ", "_");
                name = name.Replace("\n", " ");
                name = name.Replace("\t", " ");

                name = Regex.Replace(name, "[^0-9A-Za-z _]", string.Empty);
                values.Add(name);
            }

            values.Add(sAMObject.Guid.ToString("N").Substring(0, 8));

            return string.Join("__", values);
        }

        public static string UniqueName(string prefix, string uniqueName)
        {
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            return string.Format("{0}__{1}", prefix, uniqueName);
        }

        public static string UniqueName(Type type, string uniqueName)
        {
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            return string.Format("{0}__{1}", type.Name, uniqueName);
        }
    }
}