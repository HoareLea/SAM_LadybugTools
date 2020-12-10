using HoneybeeSchema;
using System.Collections.Generic;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ScheduleFixedInterval ToLadybugTools(this Profile profile)
        {
            if (profile == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(profile);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            List<ScheduleUnitType> scheduleUnitTypes = Query.ScheduleUnitTypes(profile.ProfileType);
            if (scheduleUnitTypes == null)
                return null;

            if (scheduleUnitTypes.Contains(ScheduleUnitType.ActivityLevel))
                scheduleUnitTypes.Remove(ScheduleUnitType.ActivityLevel);

            if (scheduleUnitTypes.Count == 0)
                return null;

            if (profile.Count == 0)
                return null;

            List<double> values = new List<double>();
            for(int i=0; i < 8760; i++)
                values.Add(profile[i]);

            ScheduleTypeLimit scheduleTypeLimit = new ScheduleTypeLimit(Core.LadybugTools.Query.UniqueName(typeof(ScheduleTypeLimit), uniqueName), profile.Name, unitType: scheduleUnitTypes[0]);

            ScheduleFixedInterval result = new ScheduleFixedInterval(uniqueName, values, profile.Name, scheduleTypeLimit);

            return result;
        }

        public static ScheduleFixedInterval ToLadybugTools_ActivityLevel(this Profile profile, double value)
        {
            if (profile == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(profile);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            uniqueName = Core.LadybugTools.Query.UniqueName(typeof(ActivityLevel), uniqueName);

            List<ScheduleUnitType> scheduleUnitTypes = Query.ScheduleUnitTypes(profile.ProfileType);
            if (scheduleUnitTypes == null || !scheduleUnitTypes.Contains(ScheduleUnitType.ActivityLevel))
                return null;

            ScheduleTypeLimit scheduleTypeLimit = new ScheduleTypeLimit(Core.LadybugTools.Query.UniqueName(typeof(ScheduleTypeLimit), uniqueName), profile.Name, unitType: scheduleUnitTypes[0]);

            List<double> values = new List<double>() { value };
            ScheduleFixedInterval result = new ScheduleFixedInterval(uniqueName, values, profile.Name, scheduleTypeLimit);

            return result;
        }
    }
}