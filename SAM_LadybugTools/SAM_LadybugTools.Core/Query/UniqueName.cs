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
                values.Add(name);

            values.Add(sAMObject.Guid.ToString());

            values.Add(System.Guid.NewGuid().ToString());

            return string.Join("__", values);
        }
    }
}

