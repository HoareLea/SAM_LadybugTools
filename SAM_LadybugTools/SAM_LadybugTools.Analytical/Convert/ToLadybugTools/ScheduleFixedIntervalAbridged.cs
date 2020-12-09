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

            List<double> values = profile.Values?.ToList();
            if (values == null || values.Count == 0)
                return null;

            ScheduleFixedIntervalAbridged result = new ScheduleFixedIntervalAbridged(uniqueName, values, profile.Name);

            return result;
        }
    }
}