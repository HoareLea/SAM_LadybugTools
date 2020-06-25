using System.Collections.Generic;

namespace SAM.Core.LadybugTools
{
    public static partial class Query
    {
        public static string UniqueName(this SAMObject sAMObject)
        {
            if (sAMObject == null)
                return null;

            List<string> values = new List<string>();

            string name = sAMObject.Name;
            if (string.IsNullOrWhiteSpace(name) && sAMObject is SAMInstance)
            {
                SAMInstance sAMInstance = (SAMInstance)sAMObject;
                SAMType sAMType = sAMInstance.SAMType;
                if (sAMType != null)
                    name = sAMType.Name;
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (name.Length > 68)
                    name = name.Substring(0, 68);

                values.Add(name);
            }

            values.Add(System.Guid.NewGuid().ToString("N"));

            string result = string.Join("__", values);

            result = result.Replace(" ", "_");
            result = result.Replace(":", "_");

            if (result.Length > 100)
                result = result.Substring(0, 100);

            return result;
        }
    }
}