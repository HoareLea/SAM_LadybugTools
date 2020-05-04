using System.Collections.Generic;

namespace SAM.Geometry.LadybugTools
{
    public static partial class Modify
    {
        public static List<List<double>> AppendList(this List<List<double>> list_base, List<List<double>> list)
        {
            List<List<double>> result = list_base;

            if (result == null && list == null)
                return null;

            if (result == null)
                result = new List<List<double>>();

            if (list.Count > 0)
                result.AddRange(list);

            return result;
        }
    }
}