using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if(result.Count < 1)
                result.Add(new List<double>());

            if (result.Count < 2)
                result.Add(new List<double>());

            if (result.Count < 3)
                result.Add(new List<double>());

            for(int i=0; i < list.Count; i++)
            {
                if (i <= result.Count)
                    break;

                result[i].AddRange(list[i]);
            }

            return result;
        }
    }
}
