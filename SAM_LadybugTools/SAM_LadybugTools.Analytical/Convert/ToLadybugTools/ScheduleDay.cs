using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ScheduleDay ToLadybugTools_ScheduleDay(this Profile profile)
        {
            if (profile == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(profile);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            uniqueName = Core.LadybugTools.Query.UniqueName(typeof(ScheduleDay), uniqueName);

            List<double> values = new List<double>();
            for (int i = 0; i < 24; i++)
                values.Add(profile[i]);

            Dictionary<Core.Range<int>, double> dictionary = Core.Query.RangeDictionary(values);

            values = new List<double>();
            List<List<int>> times = new List<List<int>>();
            foreach(KeyValuePair<Core.Range<int>, double> keyValuePair in dictionary)
            {
                values.Add(keyValuePair.Value);
                times.Add(new List<int>() {keyValuePair.Key.Min, 0 });
            }

            ScheduleDay result = new ScheduleDay(uniqueName, values, profile.Name, times);

            return result;
        }
    }
}