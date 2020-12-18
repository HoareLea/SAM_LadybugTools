﻿using HoneybeeSchema;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Convert
    {
        public static ScheduleFixedInterval ToLadybugTools(this Profile profile, ProfileType profileType = ProfileType.Undefined)
        {
            if (profile == null)
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(profile);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            ProfileType profileType_Temp = profileType;
            if (profileType_Temp == ProfileType.Undefined)
                profileType_Temp = profile.ProfileType;

            List<ScheduleUnitType> scheduleUnitTypes = Query.ScheduleUnitTypes(profileType_Temp);
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

            double upperLimit = 1;
            double lowerLimit = 0;
            if(scheduleUnitTypes[0] != ScheduleUnitType.Dimensionless)
            {
                upperLimit = values.Max();
                lowerLimit = values.Min();
            }

            ScheduleTypeLimit scheduleTypeLimit = new ScheduleTypeLimit(Core.LadybugTools.Query.UniqueName(typeof(ScheduleTypeLimit), uniqueName), profile.Name, lowerLimit: lowerLimit, upperLimit: upperLimit, unitType: scheduleUnitTypes[0]);

            ScheduleFixedInterval result = new ScheduleFixedInterval(uniqueName, values, profile.Name, scheduleTypeLimit);

            return result;
        }

        public static ScheduleFixedInterval ToLadybugTools_ActivityLevel(this Profile profile, double value)
        {
            if (profile == null || double.IsNaN(value))
                return null;

            string uniqueName = Core.LadybugTools.Query.UniqueName(profile);
            if (string.IsNullOrWhiteSpace(uniqueName))
                return null;

            uniqueName = Core.LadybugTools.Query.UniqueName(typeof(ActivityLevel), uniqueName);

            ScheduleTypeLimit scheduleTypeLimit = new ScheduleTypeLimit(Core.LadybugTools.Query.UniqueName(typeof(ScheduleTypeLimit), uniqueName + "_" + value.ToString()), profile.Name, lowerLimit: 0, upperLimit: value, unitType: ScheduleUnitType.ActivityLevel);

            List<double> values = Enumerable.Repeat(value, 8760).ToList();
            ScheduleFixedInterval result = new ScheduleFixedInterval(uniqueName, values, profile.Name, scheduleTypeLimit);

            return result;
        }
    }
}