using HoneybeeSchema;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ScheduleFixedIntervalAbridged ToLadybugTools(this Profile profile)
        {
            if (profile == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(profile);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            if (profile.Count == 0)
                return null;

            List<double> values = new List<double>();
            for(int i=0; i < 8760; i++)
                values.Add(profile[i]);

            ScheduleFixedIntervalAbridged result = new ScheduleFixedIntervalAbridged(uniqueName, values, profile.Name);

            return result;
        }
    }
}