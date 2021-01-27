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
                name = name.Replace("\n", "_");
                name = name.Replace("\t", "_");
                name = name.Replace(" ", "_");

                name = Regex.Replace(name, "[^A-Za-z0-9_-]", string.Empty);
                
                values.Add(name);
            }

            string result = string.Join("__", values);
            if (result.Length > 92)
                result = result.Substring(0, 70);  //Number of characters

            result = string.Format("{0}__{1}", result, sAMObject.Guid.ToString("N").Substring(0, 8));

            return result;
        }

        public static string UniqueName(Type type, string uniqueName)
        {
            if (type == null || string.IsNullOrWhiteSpace(uniqueName))
                return null;

            string typeName = type.Name;
            int count = typeName.Length + uniqueName.Length;
            if (count < 100)
                return string.Format("{0}__{1}", type.Name, uniqueName);

            return string.Format("{0}__{1}", type.Name, uniqueName.Substring(count - 101));
        }
    }
}